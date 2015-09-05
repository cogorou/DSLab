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
	/// 動画再生クラス (DirectShow)
	/// </summary>
	[TypeConverter(typeof(SelfConverter))]
	public class CxDSMediaPlayer : System.Object
		, IDisposable
		, IxDSGraphBuilderProvider
		, IxRunnable
		, IxParam
	{
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDSMediaPlayer()
		{
		}

		/// <summary>
		/// コンストラクタ (ソースファイル指定)
		/// </summary>
		/// <param name="source_file">ソースファイル</param>
		public CxDSMediaPlayer(string source_file)
		{
			SourceFile = source_file;
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// ソースファイル
		/// </summary>
		[Browsable(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:DSLab.CxDSMovie.SourceFile")]
		public virtual string SourceFile
		{
			get { return m_SourceFile; }
			set { m_SourceFile = value; }
		}
		private string m_SourceFile = "";

		/// <summary>
		/// タイムアウト (msec) [初期値:5000、範囲:-1=無限、0~=有限]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:DSLab.CxDSMovie.Timeout")]
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
		[CxDescription("P:DSLab.CxDSMovie.IsValid")]
		public virtual bool IsValid
		{
			get { return (GraphBuilder != null); }
		}

		/// <summary>
		/// フレームサイズ
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		[ReadOnly(true)]
		[Category("State")]
		[CxDescription("P:DSLab.CxDSMovie.FrameSize")]
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
		private IGraphBuilder GraphBuilder = null;

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
		/// サンプルグラバの入力ピン
		/// </summary>
		internal IPin SampleGrabberInPin = null;

		/// <summary>
		/// サンプルグラバフィルタの出力ピン
		/// </summary>
		internal IPin SampleGrabberOutPin = null;

		/// <summary>
		/// レンダラー
		/// </summary>
		internal IBaseFilter Renderer = null;

		/// <summary>
		/// レンダラーの入力ピン
		/// </summary>
		internal IPin RendererInPin = null;

		/// <summary>
		/// シーク操作用
		/// </summary>
		internal IMediaSeeking Seeking = null;

		#endregion

		#region 初期化:

		/// <summary>
		/// グラフの生成
		/// </summary>
		public virtual void Setup()
		{
			this.Dispose();

			try
			{
				// グラフ.
				// CoCreateInstance
				GraphBuilder = (IGraphBuilder)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_FilterGraph)));

				#region フィルタ追加.
				// ファイル入力.
				IBaseFilter capture = null;
				GraphBuilder.AddSourceFilter(SourceFile, "CaptureFilter", ref capture);
				if (capture == null)
					throw new System.IO.IOException();

#if false
				// DMO ラッパーフィルタ.
				// https://msdn.microsoft.com/ja-jp/library/cc371140.aspx
				IBaseFilter dmo = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_DMOWrapperFilter)));
				if (dmo != null)
				{
					//// Mpeg4 Decoder DMO
					//// F371728A-6052-4D47-827C-D039335DFE0A
					//// 4A69B442-28BE-4991-969C-B500ADF5D8A8
					//// mpg4decd.dll [C:\Windows\System32, C:\Windows\SysWOW64]

					var idmo = (IDMOWrapperFilter)dmo;
					idmo.Init(new Guid("F371728A-6052-4D47-827C-D039335DFE0A"), new Guid("4A69B442-28BE-4991-969C-B500ADF5D8A8"));
					idmo = null;

					this.GraphBuilder.AddFilter(dmo, "Mpeg4 Decoder DMO");
				}
#endif

#if false
				// Avi Splitter
				IBaseFilter splitter = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_AVISplitter)));
				if (splitter == null)
					throw new System.IO.IOException();
				this.GraphBuilder.AddFilter(splitter, "Avi Splitter");

				// Avi Decompressor
				IBaseFilter decompressor = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_AVIDec)));
				if (decompressor == null)
					throw new System.IO.IOException();
				this.GraphBuilder.AddFilter(decompressor, "Avi Decompressor");
#endif

				// サンプルグラバー.
				IBaseFilter grabber = (IBaseFilter)CreateSampleGrabber();
				if (grabber == null)
					throw new System.IO.IOException();
				this.GraphBuilder.AddFilter(grabber, "SampleGrabber");

				// レンダラー.
				IBaseFilter renderer = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_NullRenderer)));
				if (renderer == null)
					throw new System.IO.IOException();
				this.GraphBuilder.AddFilter(renderer, "Renderer");
				#endregion

				#region ピンの取得.
				IPin capture_out = DSLab.Axi.FindPin(capture, 0, PIN_DIRECTION.PINDIR_OUTPUT);
				IPin grabber_in = DSLab.Axi.FindPin(grabber, 0, PIN_DIRECTION.PINDIR_INPUT);
				IPin grabber_out = DSLab.Axi.FindPin(grabber, 0, PIN_DIRECTION.PINDIR_OUTPUT);
				IPin renderer_in = DSLab.Axi.FindPin(renderer, 0, PIN_DIRECTION.PINDIR_INPUT);
				#endregion

				#region ピンの接続.
				GraphBuilder.Connect(capture_out, grabber_in);
				GraphBuilder.Connect(grabber_out, renderer_in);
				#endregion

				#region 保管: インターフェース.
				CaptureFilter = capture;
				CaptureOutPin = capture_out;
				SampleGrabber = (ISampleGrabber)grabber;
				SampleGrabberInPin = grabber_in;
				SampleGrabberOutPin = grabber_out;
				Renderer = renderer;
				RendererInPin = renderer_in;
				#endregion

				#region 保管: フレームサイズ.
				VIDEOINFOHEADER vinfo = DSLab.Axi.GetVideoInfo(SampleGrabber);
				this.SampleGrabberCB.BitmapInfo = vinfo.bmiHeader;
				this.SampleGrabberCB.FrameSize = new Size(
					System.Math.Abs(this.SampleGrabberCB.BitmapInfo.biWidth),
					System.Math.Abs(this.SampleGrabberCB.BitmapInfo.biHeight)
					);
				#endregion

				#region インタフェースの抽出:
				{
					DSLab.IGraphBuilder graph = this.GraphBuilder;
					DSLab.IEnumFilters filters = null;
					DSLab.IBaseFilter filter = null;
					int fetched = 0;

					int hr = graph.EnumFilters(ref filters);
					while (filters.Next(1, ref filter, ref fetched) == (int)DSLab.HRESULT.S_OK)
					{
						if (fetched == 0) break;

						if (filter is DSLab.IMediaSeeking)
						{
							// シーク操作用.
							Seeking = (DSLab.IMediaSeeking)filter;
						}
						else
						{
							// フィルタ解放.
							Marshal.ReleaseComObject(filter);
							filter = null;
						}
					}

					// 解放.
					Marshal.ReleaseComObject(filters);
				}
				#endregion

				// DEBUG
#if DEBUG
				DebugPrint(this.GraphBuilder);
#endif
			}
			catch (Exception ex)
			{
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

			SampleGrabberCB.FrameSize = new Size(0, 0);
			SampleGrabberCB.Notify -= SampleGrabberCB_Notify;

			CaptureFilter = null;
			CaptureOutPin = null;
			SampleGrabber = null;
			SampleGrabberInPin = null;
			SampleGrabberOutPin = null;
			Renderer = null;
			RendererInPin = null;

			if (Seeking != null)
				Marshal.ReleaseComObject(Seeking);
			Seeking = null;

			if (GraphBuilder != null)
				Marshal.ReleaseComObject(GraphBuilder);
			GraphBuilder = null;
		}

		#endregion

		#region フィルタ生成:

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
				media_type.subtype = new Guid(GUID.MEDIASUBTYPE_RGB24);		// RGB24
				media_type.formattype = new Guid(GUID.FORMAT_VideoInfo);	// VideoInfo
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
			if (GUID.Compare(media_type.formattype.ToString(), GUID.FORMAT_VideoInfo))
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
		///		入力フィルタの状態を返します。
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

		#region IMediaSeeking 関連:

		/// <summary>
		/// ストリームの時間幅
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[Category("Seeking")]
		[CxDescription("P:DSLab.CxDSMovie.Duration")]
		public virtual long Duration
		{
			get
			{
				if (Seeking == null)
					return 0;
				else
				{
					long value = 0;
					int status = Seeking.GetDuration(ref value);
					return value;
				}
			}
		}

		/// <summary>
		/// 現在位置
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[Category("Seeking")]
		[CxDescription("P:DSLab.CxDSMovie.CurrentPosition")]
		public virtual long CurrentPosition
		{
			get
			{
				if (Seeking == null)
					return 0;
				else
				{
					long current = 0;
					int status = Seeking.GetCurrentPosition(ref current);
					return current;
				}
			}
		}

		/// <summary>
		/// 開始位置
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[Category("Seeking")]
		[CxDescription("P:DSLab.CxDSMovie.StartPosition")]
		public virtual long StartPosition
		{
			get
			{
				if (Seeking == null)
					return 0;
				else
				{
					long st = 0;
					long ed = 0;
					int status = Seeking.GetPositions(ref st, ref ed);
					return st;
				}
			}
			set
			{
				if (Seeking == null)
					return;
				else
				{
					long st = value;
					long ed = 0;
					int status = Seeking.SetPositions(
						ref st, AM_SEEKING_SEEKING_FLAGS.AbsolutePositioning,
						ref ed, AM_SEEKING_SEEKING_FLAGS.NoPositioning
						);
				}
			}
		}

		/// <summary>
		/// 停止位置
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[Category("Seeking")]
		[CxDescription("P:DSLab.CxDSMovie.StopPosition")]
		public virtual long StopPosition
		{
			get
			{
				if (Seeking == null)
					return 0;
				else
				{
					long st = 0;
					long ed = 0;
					int status = Seeking.GetPositions(ref st, ref ed);
					return ed;
				}
			}
			set
			{
				if (Seeking == null)
					return;
				else
				{
					long st = 0;
					long ed = value;
					int status = Seeking.SetPositions(
						ref st, AM_SEEKING_SEEKING_FLAGS.NoPositioning,
						ref ed, AM_SEEKING_SEEKING_FLAGS.AbsolutePositioning
						);
				}
			}
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
			StopPosition = Duration;
			StartPosition = 0;
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
				((IMediaControl)GraphBuilder).Run();
				if (FILTER_STATE.Running != GetState(Timeout))
					throw new DSLab.CxDSException(HRESULT.VFW_E_TIMEOUT);
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
				((IMediaControl)GraphBuilder).Stop();
				if (FILTER_STATE.Stopped != GetState(Timeout))
					throw new DSLab.CxDSException(HRESULT.VFW_E_TIMEOUT);
			}
			catch (DSLab.CxDSException ex)
			{
				// VFW_S_STATE_INTERMEDIATE は無視します.
				if (ex.ComError != HRESULT.VFW_S_STATE_INTERMEDIATE)
					throw new DSLab.CxDSException(ex);
			}
		}

		/// <summary>
		/// 一時停止
		/// </summary>
		/// <remarks>
		///		グラフが再生中の場合、その再生を一時停止します。
		///		一時停止に成功すると、IsRunning プロパティが false になり、IsPaused が true になります。
		///		状態が変化するまで(FILTER_STATE.Paused になるまで) 待機します。
		///		タイムアウトすると例外(HRESULT.VFW_E_TIMEOUT)を発行します。
		///		待機時間は Timeout プロパティで変更可能です。
		///		<para>
		///		exception: http://msdn.microsoft.com/ja-jp/library/cc356971.aspx
		///		</para>
		/// </remarks>
		public virtual void Pause()
		{
			if (GraphBuilder == null) return;

			try
			{
				((IMediaControl)GraphBuilder).Pause();
				if (FILTER_STATE.Paused != GetState(Timeout))
					throw new DSLab.CxDSException(HRESULT.VFW_E_TIMEOUT);
			}
			catch (DSLab.CxDSException ex)
			{
				// VFW_S_STATE_INTERMEDIATE は無視します.
				if (ex.ComError != HRESULT.VFW_S_STATE_INTERMEDIATE)
					throw new DSLab.CxDSException(ex);
			}
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
			while(IsRunning)
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
		[CxDescription("P:DSLab.CxDSMovie.IsRunning")]
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
		[CxDescription("P:DSLab.CxDSMovie.IsPaused")]
		public virtual bool IsPaused
		{
			get
			{
				if (GraphBuilder == null) return false;
				try
				{
					FILTER_STATE state = GetState(1);
					return (state == FILTER_STATE.Paused);
				}
				catch (System.Exception)
				{
				}
				return false;
			}
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
				while (true)
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
				if (destinationType == typeof(CxDSMediaPlayer))
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
