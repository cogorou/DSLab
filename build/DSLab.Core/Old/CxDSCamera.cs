/*
	DSLab
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

using HANDLE = System.IntPtr;
using HWND = System.IntPtr;
using HDC = System.IntPtr;
using HPEN = System.IntPtr;
using HGDIOBJ = System.IntPtr;
using HBITMAP = System.IntPtr;
using COLORREF = System.UInt32;
using LPPOINT = System.IntPtr;
using BYTE = System.Byte;
using WORD = System.UInt16;
using DWORD = System.UInt32;
using BOOL = System.Boolean;

namespace DSLab
{
	/// <summary>
	/// カメラクラス (DirectShow)
	/// </summary>
	[TypeConverter(typeof(SelfConverter))]
	public class CxDSCamera : System.Object
		, IDisposable
		, IxDSGraphBuilderProvider
		, IxRunnable
		, IxGrabber
		, IxParam
	{
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDSCamera()
		{
		}

		/// <summary>
		/// コンストラクタ (パラメータ指定)
		/// </summary>
		/// <param name="param">イメージグラバーパラメータ</param>
		public CxDSCamera(CxDSCameraParam param)
		{
			Param = param;
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// パラメータ
		/// </summary>
		[Browsable(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:DSLab.CxDSGrabber.Param")]
		public virtual CxDSCameraParam Param
		{
			get { return m_Param; }
			set { m_Param = value; }
		}
		private CxDSCameraParam m_Param = new CxDSCameraParam();

		/// <summary>
		/// タイムアウト (msec) [初期値:5000、範囲:-1=無限、0~=有限]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:DSLab.CxDSGrabber.Timeout")]
		public virtual int Timeout
		{
			get { return m_Timeout; }
			set
			{
				if (value >= -1)
					m_Timeout = value;
			}
		}
		private int m_Timeout = 5000;

		#endregion

		#region プロパティ: (状態)

		/// <summary>
		/// 有効性
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		[ReadOnly(true)]
		[Category("State")]
		[CxDescription("P:DSLab.CxDSGrabber.IsValid")]
		public virtual bool IsValid
		{
			get { return (GraphBuilder != null); }
		}

		/// <summary>
		/// 録画可能状態か否か
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		[ReadOnly(true)]
		[Category("State")]
		[CxDescription("P:DSLab.CxDSGrabber.Recordable")]
		public virtual bool Recordable
		{
			get { return (this.Sync != null); }
		}

		/// <summary>
		/// デバイス名称
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		[ReadOnly(true)]
		[Category("State")]
		[CxDescription("P:DSLab.CxDSGrabber.DeviceName")]
		public virtual string DeviceName
		{
			get { return m_DeviceName; }
			private set { m_DeviceName = value; }
		}
		private string m_DeviceName = "";

		/// <summary>
		/// フレームサイズ
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		[ReadOnly(true)]
		[Category("State")]
		[CxDescription("P:DSLab.CxDSGrabber.FrameSize")]
		public virtual Size FrameSize
		{
			get { return SampleGrabberCB.FrameSize; }
		}

		#endregion

		#region プロパティ: (インターフェース関連)

		/// <summary>
		/// グラフ
		/// </summary>
		[XmlIgnore]
		[Browsable(false)]
		IGraphBuilder IxDSGraphBuilderProvider.GraphBuilder
		{
			get { return GraphBuilder; }
		}

		/// <summary>
		/// グラフビルダー
		/// </summary>
		private IGraphBuilder GraphBuilder = null;

		/// <summary>
		/// キャプチャビルダー
		/// </summary>
		internal ICaptureGraphBuilder2 CaptureBuilder = null;

		/// <summary>
		/// 画像入力フィルタ
		/// </summary>
		internal IBaseFilter CaptureFilter = null;

		/// <summary>
		/// 画像入力フィルタの出力ピン
		/// </summary>
		internal IPin CaptureOutPin = null;

		/// <summary>
		/// サンプルグラバフィルタ
		/// </summary>
		internal ISampleGrabber SampleGrabber = null;

		/// <summary>
		/// レンダラー
		/// </summary>
		internal IBaseFilter Renderer = null;

		/// <summary>
		/// マルチプレクサー
		/// </summary>
		internal IBaseFilter Mux = null;

		/// <summary>
		/// ファイルシンク
		/// </summary>
		internal IFileSinkFilter Sync = null;

		#endregion

		#region プロパティダイアログ

		/// <summary>
		/// プロパティダイアログ
		/// </summary>
		/// <param name="owner">ダイアログのオーナーコントロール</param>
		/// <param name="caption">ダイアログのタイトル</param>
		/// <param name="type">プロパティダイアログ種別</param>
		/// <returns>
		///		正常に開かれた場合は true を返します。
		///		利用できないか、異常がある場合は false を返します。
		/// </returns>
		public virtual bool OpenPropertyDialog(System.Windows.Forms.Control owner, string caption, PropertyDialogType type)
		{
			switch (type)
			{
				case PropertyDialogType.Capture:
					return DSLab.Axi.OpenPropertyDialog(owner.Handle, caption, this.CaptureFilter);
				case PropertyDialogType.Outpin:
					return DSLab.Axi.OpenPropertyDialog(owner.Handle, caption, this.CaptureOutPin);
			}
			return false;
		}

		#endregion

		#region 初期化:

		/// <summary>
		/// グラフの生成
		/// </summary>
		public virtual void Setup()
		{
			this.Setup(string.Empty);
		}

		/// <summary>
		/// グラフの生成
		/// </summary>
		/// <param name="output_file">出力ファイル</param>
		public virtual void Setup(string output_file)
		{
			this.Dispose();

			try
			{
				CxDSCameraParam param = this.Param;

				// グラフビルダー.
				// CoCreateInstance
				GraphBuilder = (IGraphBuilder)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_FilterGraph)));

				#region フィルタ追加.
				// 画像入力フィルタ.
				IBaseFilter capture = CreateVideoCapture(param);
				if (capture == null)
					throw new System.IO.IOException();
				this.GraphBuilder.AddFilter(capture, "CaptureFilter");
				IPin capture_out = DSLab.Axi.FindPin(capture, 0, PIN_DIRECTION.PINDIR_OUTPUT);
				this.CaptureFilter = capture;
				this.CaptureOutPin = capture_out;

				// サンプルグラバー.
				IBaseFilter grabber = (IBaseFilter)CreateSampleGrabber();
				if (grabber == null)
					throw new System.IO.IOException();
				this.GraphBuilder.AddFilter(grabber, "SampleGrabber");
				this.SampleGrabber = (ISampleGrabber)grabber;
				#endregion

				#region キャプチャビルダー:
				{
					int hr = 0;
					CaptureBuilder = (ICaptureGraphBuilder2)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_CaptureGraphBuilder2)));
					hr = CaptureBuilder.SetFiltergraph(GraphBuilder);

					if (string.IsNullOrEmpty(output_file))
					{
						// レンダラー.
						IBaseFilter renderer = null;
						renderer = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_NullRenderer)));
						if (renderer == null)
							throw new System.IO.IOException();
						this.GraphBuilder.AddFilter(renderer, "Renderer");
						this.Renderer = renderer;

#if true
						// IGraphBuilder.Connect の代わりに ICaptureGraphBuilder2.RenderStream を使用する.
						// fig) [capture]-out->-in-[sample grabber]-out->-in-[null render]
						hr = CaptureBuilder.RenderStream(new Guid(GUID.PIN_CATEGORY_CAPTURE), new Guid(GUID.MEDIATYPE_Video), capture, grabber, renderer);

#else
						// ピンの取得.
						IPin grabber_in = DSLab.Axi.FindPin(grabber, 0, PIN_DIRECTION.PINDIR_INPUT);
						IPin grabber_out = DSLab.Axi.FindPin(grabber, 0, PIN_DIRECTION.PINDIR_OUTPUT);
						IPin renderer_in = DSLab.Axi.FindPin(renderer, 0, PIN_DIRECTION.PINDIR_INPUT);

						// ピンの接続.
						GraphBuilder.Connect(capture_out, grabber_in);
						GraphBuilder.Connect(grabber_out, renderer_in);

						// ピンの保管.
						//SampleGrabberInPin = grabber_in;
						//SampleGrabberOutPin = grabber_out;
						//RendererInPin = renderer_in;
#endif
					}
					else
					{
						IBaseFilter mux = null;
						IFileSinkFilter sync = null;
						hr = CaptureBuilder.SetOutputFileName(new Guid(GUID.MEDIASUBTYPE_Avi), output_file, ref mux, ref sync);
						hr = CaptureBuilder.RenderStream(new Guid(GUID.PIN_CATEGORY_CAPTURE), new Guid(GUID.MEDIATYPE_Video), capture, grabber, mux);
						this.Mux = mux;
						this.Sync = sync;
					}
				}
				#endregion

				#region 保管: フレームサイズ.
				VIDEOINFOHEADER vinfo = DSLab.Axi.GetVideoInfo(SampleGrabber);
				this.SampleGrabberCB.BitmapInfo = vinfo.bmiHeader;
				this.SampleGrabberCB.FrameSize = new Size(
					System.Math.Abs(this.SampleGrabberCB.BitmapInfo.biWidth),
					System.Math.Abs(this.SampleGrabberCB.BitmapInfo.biHeight)
					);
				#endregion

				#region 保管: デバイス名称.
				try
				{
					if (string.IsNullOrEmpty(param.FilterInfo.Name) == false)
					{
						this.DeviceName = param.FilterInfo.Name;
					}
					else
					{
						int filter_index = param.FilterInfo.Index;
						List<DSLab.CxDSFilterInfo> filters = DSLab.Axi.GetFilterList(DSLab.GUID.CLSID_VideoInputDeviceCategory);
						if (0 <= filter_index && filter_index < filters.Count)
						{
							this.DeviceName = filters[filter_index].Name;
						}
					}
				}
				catch (System.Exception)
				{
					this.DeviceName = "";
				}
				#endregion

				// DEBUG
#if DEBUG
				DebugPrint(this.GraphBuilder);
#endif
			}
			catch (Exception ex)
			{
				this.Dispose();
				throw new DSLab.CxDSException(ex);
			}
		}

		#endregion

		#region 解放:

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			try
			{
				Stop();
			}
			catch (Exception)
			{
			}

			DeviceName = "";
			SampleGrabberCB.FrameSize = new Size(0, 0);
			SampleGrabberCB.Notify -= SampleGrabberCB_Notify;

			CaptureFilter = null;
			CaptureOutPin = null;
			SampleGrabber = null;
			Renderer = null;

			Mux = null;
			Sync = null;

			if (CaptureBuilder != null)
				Marshal.ReleaseComObject(CaptureBuilder);
			CaptureBuilder = null;

			if (GraphBuilder != null)
				Marshal.ReleaseComObject(GraphBuilder);
			GraphBuilder = null;
		}

		#endregion

		#region フィルタ生成:

		/// <summary>
		/// 画像入力フィルタの生成
		/// </summary>
		/// <param name="param">イメージグラバーパラメータ</param>
		/// <returns>
		///		 生成されたインスタンスを返します。
		/// </returns>
		internal virtual IBaseFilter CreateVideoCapture(CxDSCameraParam param)
		{
			IBaseFilter capture = DSLab.Axi.CreateFilter(GUID.CLSID_VideoInputDeviceCategory, param.FilterInfo.Name, param.FilterInfo.Index);
			if (capture != null)
			{
				// ビデオ出力フォーマットの設定.
				// Width,Height に 0,0 が指定されている場合は既定値を使用する.
				// 指定されている場合は、VIDEOINFOHEADER を書き換えて SetFormat を行う.
				//
				// http://msdn.microsoft.com/ja-jp/library/cc353344.aspx
				// 
				if (0 < param.FormatInfo.VideoSize.Width &&
					0 < param.FormatInfo.VideoSize.Height)
				{
					// 出力ピン.
					IPin pin = null;
					if (param.PinInfo.Name != "")
						pin = DSLab.Axi.FindPin(capture, param.PinInfo.Name);
					else
						pin = DSLab.Axi.FindPin(capture, 0, param.PinInfo.Direction);

					#region 手段1) IAMStreamConfig.GetNumberOfCapabilities で列挙して、該当する AM_MEDIA_TYPE を SetFormat する方法.
					if (pin is IAMStreamConfig)
					{
						IAMStreamConfig config = pin as IAMStreamConfig;
						int count = 0;
						int size = 0;
						config.GetNumberOfCapabilities(ref count, ref size);

						if (size == Marshal.SizeOf(new VIDEO_STREAM_CONFIG_CAPS()))
						{
							for (int i = 0; i < count; i++)
							{
								AM_MEDIA_TYPE media_type = new AM_MEDIA_TYPE();
								VIDEOINFOHEADER video_info = new VIDEOINFOHEADER();
								IntPtr addr = IntPtr.Zero;

								try
								{
									addr = Marshal.AllocCoTaskMem(size);
									int status = config.GetStreamCaps(i, ref media_type, addr);
									if (status == (int)HRESULT.S_OK &&
										DS.GUID.Compare(media_type.majortype.ToString(), DS.GUID.MEDIATYPE_Video) &&
									//	Function.GuidCompare(media_type.subtype.ToString(), DS.GUID.MEDIASUBTYPE_RGB24) &&
										DS.GUID.Compare(media_type.formattype.ToString(), DS.GUID.FORMAT_VideoInfo) &&
										media_type.cbFormat >= Marshal.SizeOf(video_info) &&
										media_type.pbFormat != IntPtr.Zero
										)
									{
										video_info = (VIDEOINFOHEADER)Marshal.PtrToStructure(media_type.pbFormat, typeof(VIDEOINFOHEADER));

										// --- ビデオ入力サイズの確認.
										if (video_info.bmiHeader.biWidth == param.FormatInfo.VideoSize.Width &&
											video_info.bmiHeader.biHeight == param.FormatInfo.VideoSize.Height)
										{
											config.SetFormat(media_type);
											return capture;
										}
									}
								}
								finally
								{
									if (addr != IntPtr.Zero)
										Marshal.FreeCoTaskMem(addr);
									Axi.DeleteMediaType(ref media_type);
								}
							}
						}
					}
					#endregion

					#region 手段2) VIDEOINFOHEADER の Width,Height を書き換えて SetFormat を行う方法.
					//
					// この手段は、多くのカメラで有効だが、LifeCam (x86) では失敗する.
					//
					{
						AM_MEDIA_TYPE media_type = new AM_MEDIA_TYPE();
						VIDEOINFOHEADER video_info = new VIDEOINFOHEADER();

						media_type = Axi.GetFormat(pin);
						video_info = (VIDEOINFOHEADER)Marshal.PtrToStructure(media_type.pbFormat, typeof(VIDEOINFOHEADER));

						// --- ビデオ入力サイズ.
						video_info.bmiHeader.biWidth = param.FormatInfo.VideoSize.Width;
						video_info.bmiHeader.biHeight = param.FormatInfo.VideoSize.Height;

						// 2013.09.18(Wed): LifeCam (x86) でエラーが発生するので試したが効果は無かった.
						//video_info.bmiHeader.biBitCount = (short)BppIn;

						// 2013.09.18(Wed): lSampleSize を変更すると LifeCam (x86) でエラーが発生する.
						// --- サンプルサイズ.
						//int horz = System.Math.Abs(param.FilterFormatInfo.VideoSize.Width);
						//int vert = System.Math.Abs(param.FilterFormatInfo.VideoSize.Height);
						//int bpp = BppIn;
						//media_type.lSampleSize = FVIL.Data.CFviImage.CalcHorzByte(horz, bpp) * (uint)vert;

						Marshal.StructureToPtr(video_info, media_type.pbFormat, true);
						Axi.SetFormat(pin, media_type);
					}
					#endregion
				}
			}
			return capture;
		}

		/// <summary>
		/// サンプルグラバーの生成
		/// </summary>
		/// <returns>
		///		生成されたサンプルグラバーを返します。
		///	</returns>
		internal virtual ISampleGrabber CreateSampleGrabber()
		{
			ISampleGrabber grabber = (ISampleGrabber)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_SampleGrabber)));
			if (grabber != null)
			{
				// サンプルグラバフィルタの入力形式設定.
				// SetMediaType で必要なメディア タイプを指定します。
				//   http://msdn.microsoft.com/ja-jp/library/cc369546.aspx
				// ※AM_MEDIA_TYPE 構造体のメンバをすべて設定する必要はない。
				// ※デフォルトでは、サンプル グラバに優先メディア タイプはない。
				// ※サンプル グラバを正しいフィルタに確実に接続するには、フィルタ グラフを作成する前にこのメソッドを呼び出す。
				// majortype: http://msdn.microsoft.com/ja-jp/library/cc370108.aspx
				// subtype  : http://msdn.microsoft.com/ja-jp/library/cc371040.aspx
				AM_MEDIA_TYPE media_type = new AM_MEDIA_TYPE();
				media_type.majortype = new Guid(GUID.MEDIATYPE_Video);		// Video
				media_type.subtype = new Guid(GUID.MEDIASUBTYPE_RGB24);	// RGB24
				media_type.formattype = new Guid(GUID.FORMAT_VideoInfo);		// VideoInfo
				grabber.SetMediaType(media_type);
				grabber.SetBufferSamples(false);					// サンプルコピー 無効.
				grabber.SetOneShot(false);							// One Shot 無効.
				//grabber.SetCallback(this.SampleGrabberCB, 0);		// 0:SampleCB メソッドを呼び出すよう指示する.
				grabber.SetCallback(this.SampleGrabberCB, 1);		// 1:BufferCB メソッドを呼び出すよう指示する.
				SampleGrabberCB.Notify += SampleGrabberCB_Notify;
			}
			return grabber;
		}

		#endregion

		#region デバッグ

		/// <summary>
		/// デバッグ
		/// </summary>
		/// <param name="graph"></param>
		private void DebugPrint(IGraphBuilder graph)
		{
			IEnumFilters filters = null;
			IBaseFilter filter = null;
			int fetched = 0;

			int hr = graph.EnumFilters(ref filters);
			while (filters.Next(1, ref filter, ref fetched) == (int)DSLab.HRESULT.S_OK)
			{
				if (fetched == 0) break;

				FILTER_INFO info = new FILTER_INFO();
				hr = filter.QueryFilterInfo(info);
				Console.WriteLine("{0}", info.achName);

				// フィルタ解放.
				Marshal.ReleaseComObject(filter);
				filter = null;
			}

			// 解放.
			Marshal.ReleaseComObject(filters);

			// サンプルグラバフィルタの入力形式設定.
			// majortype: http://msdn.microsoft.com/ja-jp/library/cc370108.aspx
			// subtype  : http://msdn.microsoft.com/ja-jp/library/cc371040.aspx
			AM_MEDIA_TYPE media_type = new AM_MEDIA_TYPE();
			SampleGrabber.GetConnectedMediaType(media_type);

			Debug.WriteLine("{0}:{1}", "majortype", media_type.majortype);
			Debug.WriteLine("{0}:{1}", "subtype", media_type.subtype);
			Debug.WriteLine("{0}:{1}", "formattype", media_type.formattype);
			Debug.WriteLine("{0}:{1}", "bFixedSizeSamples", media_type.bFixedSizeSamples);
			Debug.WriteLine("{0}:{1}", "bTemporalCompression", media_type.bTemporalCompression);
			Debug.WriteLine("{0}:{1}", "cbFormat", media_type.cbFormat);
			Debug.WriteLine("{0}:{1}", "lSampleSize", media_type.lSampleSize);
			Debug.WriteLine("{0}:{1}", "pbFormat", media_type.pbFormat);
			Debug.WriteLine("{0}:{1}", "pUnk", media_type.pUnk);

			// 映像形式の場合、サイズを取得する.
			if (DS.GUID.Compare(media_type.formattype.ToString(), GUID.FORMAT_VideoInfo))
			{
				VIDEOINFOHEADER vinfo = new VIDEOINFOHEADER();
				vinfo = (VIDEOINFOHEADER)Marshal.PtrToStructure(media_type.pbFormat, typeof(VIDEOINFOHEADER));

				Debug.WriteLine("{0}:{1}", "Width", vinfo.bmiHeader.biWidth);
				Debug.WriteLine("{0}:{1}", "Height", vinfo.bmiHeader.biHeight);
				Debug.WriteLine("{0}:{1}", "BitCount", vinfo.bmiHeader.biBitCount);
				Debug.WriteLine("{0}:{1}", "Size", vinfo.bmiHeader.biSize);
				Debug.WriteLine("{0}:{1}", "ImageSize", vinfo.bmiHeader.biSizeImage);
				Debug.WriteLine("{0}:{1}", "ClrImportant", vinfo.bmiHeader.biClrImportant);
				Debug.WriteLine("{0}:{1}", "ClrUsed", vinfo.bmiHeader.biClrUsed);
				Debug.WriteLine("{0}:{1}", "Compression", vinfo.bmiHeader.biCompression);
				Debug.WriteLine("{0}:{1}", "Planes", vinfo.bmiHeader.biPlanes);
				Debug.WriteLine("{0}:{1}", "XPelsPerMeter", vinfo.bmiHeader.biXPelsPerMeter);
				Debug.WriteLine("{0}:{1}", "YPelsPerMeter", vinfo.bmiHeader.biYPelsPerMeter);
			}
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 状態取得
		/// </summary>
		/// <param name="timeout">タイムアウト(msec) [-1:無限]</param>
		/// <returns>
		///		Pause 中に IMediaControl.GetState を行うとステータスを取得できないので、例外(HRESULT.VFW_S_CANT_CUE)を返します。
		///		その為、このクラスでは Graph の Pause の代わりに IsPaused プロパティを使用しています。
		/// </returns>
		public virtual FILTER_STATE GetState(int timeout)
		{
			if (GraphBuilder == null)
				return FILTER_STATE.Stopped;

			timeout = (timeout < 0) ? -1 : timeout;
			int state = (int)FILTER_STATE.Stopped;
			int result = ((IMediaControl)GraphBuilder).GetState(-1, ref state);
			if (result != (int)HRESULT.S_OK)
				throw new DSLab.CxDSException((HRESULT)result);

			return (FILTER_STATE)state;
		}

		#endregion

		#region イベント:

		/// <summary>
		/// 通知イベント  
		/// </summary>
		public virtual event CxDSGrabberEventHandler Notify;

		/// <summary>
		/// サンプルグラバーコールバック
		/// </summary>
		private CxSampleGrabberCB SampleGrabberCB = new CxSampleGrabberCB();

		/// <summary>
		/// サンプルグラバーコールバックからの通知
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SampleGrabberCB_Notify(object sender, CxDSGrabberEventArgs e)
		{
			if (this.Notify != null)
				this.Notify(this, e);
		}

		#endregion

		#region IxRunnable の実装: (グラフ操作)

		/// <summary>
		/// リセット
		/// </summary>
		public virtual void Reset()
		{
			// no job
		}

		/// <summary>
		/// 開始
		/// </summary>
		/// <remarks>
		///		グラフの再生を開始します。
		///		開始に成功すると、IsRunning プロパティが true になります。
		///		状態が変化するまで(FILTER_STATE.Running になるまで) 待機します。
		///		タイムアウトすると例外(HRESULT.VFW_E_TIMEOUT)を発行します。
		///		待機時間は Timeout プロパティで変更可能です。
		///		<para>
		///		exception: http://msdn.microsoft.com/ja-jp/library/cc356971.aspx
		///		</para>
		/// </remarks>
		public virtual void Start()
		{
			if (SampleGrabber == null)
				throw new DSLab.CxDSException(HRESULT.E_FAIL);
			if (GraphBuilder == null)
				throw new DSLab.CxDSException(HRESULT.E_FAIL);

			try
			{
				IsPaused = false;

				if (IsRunning == false)
				{
					((IMediaControl)GraphBuilder).Run();
					if (FILTER_STATE.Running != GetState(Timeout))
						throw new DSLab.CxDSException(HRESULT.VFW_E_TIMEOUT);
				}
			}
			catch (DSLab.CxDSException ex)
			{
				// VFW_S_STATE_INTERMEDIATE は無視します.
				if (ex.ComError != HRESULT.VFW_S_STATE_INTERMEDIATE)
					throw new DSLab.CxDSException(ex);
			}
		}

		/// <summary>
		/// 停止
		/// </summary>
		/// <remarks>
		///		グラフが再生中の場合、その再生を停止します。
		///		停止に成功すると、IsRunning プロパティが false になります。
		///		状態が変化するまで(FILTER_STATE.Stopped になるまで) 待機します。
		///		タイムアウトすると例外(HRESULT.VFW_E_TIMEOUT)を発行します。
		///		待機時間は Timeout プロパティで変更可能です。
		///		<para>
		///		exception: http://msdn.microsoft.com/ja-jp/library/cc356971.aspx
		///		</para>
		/// </remarks>
		public virtual void Stop()
		{
			if (GraphBuilder == null) return;

			try
			{
				if (IsRunning)
				{
					((IMediaControl)GraphBuilder).Stop();
					if (FILTER_STATE.Stopped != GetState(Timeout))
						throw new DSLab.CxDSException(HRESULT.VFW_E_TIMEOUT);
				}
			}
			catch (DSLab.CxDSException ex)
			{
				// VFW_S_STATE_INTERMEDIATE は無視します.
				if (ex.ComError != HRESULT.VFW_S_STATE_INTERMEDIATE)
					throw new DSLab.CxDSException(ex);
			}

			IsPaused = false;
		}

		/// <summary>
		/// 待機
		/// </summary>
		/// <param name="timeout">タイムアウト (msec) [-1,0~]</param>
		/// <returns>
		///		停止を検知すると true を返します。
		///		タイムアウトすると false を返します。
		/// </returns>
		public virtual bool Wait(int timeout)
		{
			var watch = new DSLab.CxStopwatch();
			watch.Start();
			while (IsRunning)
			{
				watch.Stop();
				if (0 <= timeout && timeout <= watch.Elapsed)
					return false;
				System.Threading.Thread.Sleep(1);
			}
			return true;
		}

		/// <summary>
		/// 動作状態
		/// </summary>
		/// <returns>
		///		非同期処理が動作中の場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		[XmlIgnore]
		[Browsable(true)]
		[ReadOnly(true)]
		[Category("State")]
		[CxDescription("P:DSLab.CxDSGrabber.IsRunning")]
		public virtual bool IsRunning
		{
			get
			{
				if (GraphBuilder == null) return false;
				try
				{
					FILTER_STATE state = GetState(1);
					return (state == FILTER_STATE.Running || state == FILTER_STATE.Paused);
				}
				catch (System.Exception)
				{
				}
				return false;
			}
		}

		/// <summary>
		/// 一時停止状態
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		[Category("State")]
		[CxDescription("P:DSLab.CxDSGrabber.IsPaused")]
		public virtual bool IsPaused
		{
			get { return SampleGrabberCB.IsPaused; }
			set { SampleGrabberCB.IsPaused = value; }
		}

		#endregion

		#region IxGrabber の実装:

		/// <summary>
		/// 取り込み
		/// </summary>
		/// <param name="dst">取り込み先のオブジェクト</param>
		/// <param name="continuous">連続取り込みの指示</param>
		public void Capture(object dst, bool continuous = false)
		{
			if (SampleGrabber == null)
				throw new DSLab.CxDSException(HRESULT.E_FAIL);
			if (GraphBuilder == null)
				throw new DSLab.CxDSException(HRESULT.E_FAIL);

			// 取り込み用バッファ.
			List<CxImage> buffer = new List<CxImage>();
			if (dst is CxImage)
			{
				var _dst = (CxImage)dst;
				buffer.Add(_dst);
			}
			else if (dst is IEnumerable<CxImage>)
			{
				var _dst = (IEnumerable<CxImage>)dst;
				buffer.AddRange(_dst);
			}

			// 1) 現在時刻.(UTC)
			ulong timestamp = DSLab.Axi.GetTime();

			// 2) 周期処理の開始.
			SampleGrabberCB.FrameIndex = new TxFrameIndex(0, 0, 0, 0.0, timestamp);
			SampleGrabberCB.Buffer = buffer;
			SampleGrabberCB.Continuous = continuous;
			SampleGrabberCB.Enable = true;
		}

		/// <summary>
		/// 取り込み完了後の後処理
		/// </summary>
		public void Release()
		{
			if (SampleGrabber == null)
				throw new DSLab.CxDSException(HRESULT.E_FAIL);
			if (GraphBuilder == null)
				throw new DSLab.CxDSException(HRESULT.E_FAIL);

			// 1) 周期処理の停止.
			SampleGrabberCB.Enable = false;

			// 2) 待機.
			{
				CxStopwatch watch = new CxStopwatch();
				watch.Start();
				while (SampleGrabberCB.IsBusy)
				{
					watch.Stop();
					if (m_Timeout <= watch.Elapsed) break;
					System.Threading.Thread.Sleep(1);
				}
			}

			// 3) 解放.
			if (SampleGrabberCB.IsBusy == false)
				SampleGrabberCB.Buffer.Clear();
		}

		/// <summary>
		/// 1フレーム取り込み完了待機
		/// </summary>
		/// <param name="timeout">タイムアウト (msec) [範囲:-1=無限、0~=有限]</param>
		/// <returns>
		///		1フレームの取り込みが完了するまで処理をブロックします。<br/>
		///		取り込みが完了した場合は取り込み完了後のフレーム指標を返します。<br/>
		///		それ以外は例外を発行します。<br/>
		/// </returns>
		public TxFrameIndex WaitFrame(int timeout)
		{
			if (SampleGrabber == null)
				throw new DSLab.CxDSException(HRESULT.E_FAIL);
			if (GraphBuilder == null)
				throw new DSLab.CxDSException(HRESULT.E_FAIL);

			TxFrameIndex current = SampleGrabberCB.FrameIndex;

			if (SampleGrabberCB.FrameIndex.Flag == 1)
			{
				// Continuous != true:
				// 

				return current;	// reached
			}
			else
			{
				// Continuous == true: 
				// 

				CxStopwatch watch = new CxStopwatch();
				watch.Start();
				while(true)
				{
					if (SampleGrabberCB.FrameIndex.Frame != current.Frame)
					{
						return current;	// reached
					}

					// interrupt (stop or abort)
					if (SampleGrabberCB.Enable == false)
						throw new CxException(ExStatus.Interrupted);

					watch.Stop();
					if (0 <= timeout && timeout <= watch.Elapsed)
						throw new CxException(ExStatus.Interrupted);

					System.Threading.Thread.Sleep(1);
				}
			}
		}

		/// <summary>
		/// 取り込み中のフレーム指標の取得 [0~]
		/// </summary>
		/// <returns>
		///		取り込み中のフレームの指標を返します。
		/// </returns>
		public TxFrameIndex FrameIndex()
		{
			return SampleGrabberCB.FrameIndex;
		}

		#endregion

		#region IxParam の実装:

		/// <summary>
		/// パラメータの取得
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		/// <returns>
		///		取得した値を返します。
		///	</returns>
		public object GetParam(string name)
		{
			switch (name)
			{
				case "FrameSize":
					{
						TxSizeI value = new TxSizeI();
						value.Width = this.SampleGrabberCB.FrameSize.Width;
						value.Height = this.SampleGrabberCB.FrameSize.Height;
						return value;
					}
				default:
					throw new NotSupportedException("Specified name is not supported.");
			}
		}

		/// <summary>
		/// パラメータの設定
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		/// <param name="value">設定値</param>
		public void SetParam(string name, object value)
		{
			TxModel model = ModelOf.From(value.GetType());
			switch (model.Type)
			{
				case ExType.None:
					throw new CxException(ExStatus.Unsupported);
				case ExType.Ptr:
					break;
				default:
					break;
			}
		}

		#endregion

		#region SelfConverter

		/// <summary>
		/// 型変換クラス
		/// </summary>
		internal class SelfConverter : ExpandableObjectConverter
		{
			/// <summary>
			/// コンバータがオブジェクトを指定した型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(CxDSCamera))
					return true;
				return base.CanConvertTo(context, destinationType);
			}

			/// <summary>
			/// 指定されたオブジェクトを指定した型に変換します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		インスタンスの内容を文字列に変換して返します。
			/// </returns>
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}

			/// <summary>
			/// コンバータが指定した型のオブジェクトから自身の型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="sourceType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return base.CanConvertFrom(context, sourceType);
			}

			/// <summary>
			/// 指定された型のオブジェクトから自身の型への変換
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <returns>
			///		変換後のインスタンスを返します。
			/// </returns>
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion

		#region ISampleGrabberCB

		/// <summary>
		/// サンプルグラバーコールバック
		/// </summary>
		class CxSampleGrabberCB : System.Object
			, ISampleGrabberCB
		{
			public CxSampleGrabberCB()
			{
			}

			#region イベント:

			/// <summary>
			/// 通知イベント  
			/// </summary>
			public virtual event CxDSGrabberEventHandler Notify;

			#endregion

			#region フィールド:

			public BITMAPINFOHEADER BitmapInfo = new BITMAPINFOHEADER();
			public Size FrameSize = new Size();

			public bool IsBusy = false;
			public bool IsPaused = false;
			public bool Enable = false;
			public bool Continuous = false;
			public List<CxImage> Buffer = new List<CxImage>();
			public TxFrameIndex FrameIndex = new TxFrameIndex();

			#endregion

			#region ISampleGrabberCB の実装: (画像取り込み処理)

			/// <summary>
			/// フレームキャプチャ完了時に呼び出されるコールバック関数
			/// </summary>
			/// <param name="sample_time">タイムスタンプ</param>
			/// <param name="pSample">サンプルデータ</param>
			/// <returns>
			///		DSLab.HRESULT.S_OK を返します。
			/// </returns>
			int ISampleGrabberCB.SampleCB(double sample_time, IMediaSample pSample)
			{
				return (int)DSLab.HRESULT.S_OK;
			}

			/// <summary>
			/// フレームキャプチャ完了時に呼び出されるコールバック関数
			/// </summary>
			/// <param name="progress">タイムスタンプ</param>
			/// <param name="sample_addr">サンプルデータの先頭アドレス</param>
			/// <param name="sample_size">サンプルデータ長 (bytes)</param>
			/// <returns>
			///		DSLab.HRESULT.S_OK を返します。
			/// </returns>
			int ISampleGrabberCB.BufferCB(double progress, IntPtr sample_addr, int sample_size)
			{
				// サンプリングデータが不正.
				if (sample_addr == IntPtr.Zero) return (int)DSLab.HRESULT.S_OK;
				if (sample_size <= 0) return (int)DSLab.HRESULT.S_OK;

				// Notify 用.
				try
				{
					var args = new CxDSGrabberEventArgs(progress, sample_addr, sample_size, this.BitmapInfo);
					if (Notify != null)
						Notify.Invoke(this, args);
				}
				catch (System.Exception)
				{
					System.Threading.Thread.Sleep(1);
				}

				#region IxGrabber 用.
				// 取り込み中の指標.
				TxFrameIndex current = FrameIndex;
				if (Enable && !IsPaused && (current.Frame < Buffer.Count))
				{
					IsBusy = true;

					try
					{
						var args = new CxDSGrabberEventArgs(progress, sample_addr, sample_size, this.BitmapInfo);
						args.CopyTo(Buffer[current.Frame]);
					}
					catch (System.Exception)
					{
						System.Threading.Thread.Sleep(1);
					}

					// 現在時刻.(UTC)
					ulong timestamp = DSLab.Axi.GetTime();

					// 次のフレームの指標.
					TxFrameIndex next = new TxFrameIndex(current.Track, current.Frame + 1, 0, progress, timestamp);
					if (next.Frame >= Buffer.Count)
					{
						if (Continuous)
						{
							next.Frame = 0;
							next.Track += 1;
						}
						else
						{
							next.Flag = 1;	// reached
						}
					}
					FrameIndex = next;

					IsBusy = false;
				}
				#endregion

				return (int)DSLab.HRESULT.S_OK;
			}

			#endregion
		}

		#endregion
	}
}
