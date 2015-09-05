/*
	DSLab
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace DSLab
{
	/// <summary>
	/// DirectShow 関連 補助関数群
	/// </summary>
	public static class Axi
	{
		#region 汎用関数:

		/// <summary>
		/// COMオブジェクトの生成
		/// </summary>
		/// <param name="clsid">CLSID</param>
		/// <returns>
		///		生成されたインスタンスを返します。
		/// </returns>
		public static object CoCreateInstance(string clsid)
		{
			return Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(clsid)));
		}

		/// <summary>
		/// インスタンスを開放します。
		/// </summary>
		/// <param name="obj">解放するCOMオブジェクト</param>
		public static void ReleaseInstance(object obj)
		{
			if (obj != null)
				Marshal.ReleaseComObject(obj);
		}

		#endregion

		#region フィルタ生成:

		/// <summary>
		/// フィルタ生成
		/// </summary>
		/// <param name="category">フィルタのカテゴリ</param>
		/// <param name="clsid">フィルタの CLSID (省略する場合は null を指定してください。)</param>
		/// <param name="index">指標 [0~]</param>
		/// <returns>
		///		生成されたインスタンスを返します。
		///	</returns>
		public static IBaseFilter CreateFilter(string category, string clsid, int index)
		{
			return CreateFilter(new Guid(category), clsid, index);
		}

		/// <summary>
		/// フィルタ生成
		/// </summary>
		/// <param name="category">フィルタのカテゴリ</param>
		/// <param name="clsid">フィルタの CLSID (省略する場合は null を指定してください。)</param>
		/// <param name="index">指標 [0~]</param>
		/// <returns>
		///		生成されたフィルタを返します。
		/// </returns>
		/// <remarks>
		///		seealso: https://msdn.microsoft.com/en-us/library/windows/desktop/dd407292(v=vs.85).aspx <br/>
		/// </remarks>
		public static IBaseFilter CreateFilter(Guid category, string clsid, int index)
		{
			System.Runtime.InteropServices.ComTypes.IEnumMoniker enumerator = null;
			ICreateDevEnum device = null;

			try
			{
				// ICreateDevEnum インターフェース取得.
				device = (ICreateDevEnum)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_SystemDeviceEnum)));

				// EnumMonikerの作成.
				device.CreateClassEnumerator(ref category, ref enumerator, 0);

				// 列挙.
				var monikers = new System.Runtime.InteropServices.ComTypes.IMoniker[1];
				IntPtr fetched = IntPtr.Zero;

				while (enumerator.Next(monikers.Length, monikers, fetched) == 0)
				{
					System.Runtime.InteropServices.ComTypes.IMoniker moniker = monikers[0];

					// プロパティバッグへのバインド.
					IPropertyBag propbag = null;
					{
						object tmp = null;
						Guid guid = new Guid(GUID.IID_IPropertyBag);
						moniker.BindToStorage(null, null, ref guid, out tmp);
						propbag = (IPropertyBag)tmp;
					}

					try
					{
						// フィルタ名の判定.
						if (!string.IsNullOrEmpty(clsid))
						{
							object tmp = null;
							propbag.Read("CLSID", ref tmp, 0);
							if ((string)tmp != clsid) continue;
						}

						// 指標の判定.
						if (index > 0)
						{
							index--;
							continue;
						}

						// フィルタインスタンス取得.
						object filter = null;
						{
							Guid guid = new Guid(GUID.IID_IBaseFilter);
							moniker.BindToObject(null, null, ref guid, out filter);
						}
						if (filter == null)
							throw new System.NotSupportedException();
						return (IBaseFilter)filter;
					}
					finally
					{
						// プロパティバッグの解放
						Marshal.ReleaseComObject(propbag);

						// 列挙したモニカの解放.
						for (int mmm = 0; mmm < monikers.Length; mmm++)
						{
							if (monikers[mmm] != null)
								Marshal.ReleaseComObject(monikers[mmm]);
							monikers[mmm] = null;
						}
					}
				}
				throw new System.NotSupportedException();
			}
			finally
			{
				if (enumerator != null)
					Marshal.ReleaseComObject(enumerator);
				if (device != null)
					Marshal.ReleaseComObject(device);
			}
		}

		#endregion

		#region ピン名称の取得:

		/// <summary>
		/// ピン名称の取得
		/// </summary>
		/// <param name="Pin">ピン</param>
		/// <returns>
		///		ピン名称を返します。
		///	</returns>
		public static string GetPinName(IPin Pin)
		{
			var info = new PIN_INFO();

			try
			{
				Pin.QueryPinInfo(info);
				return info.achName;
			}
			finally
			{
				if (info.pFilter != null)
					Marshal.ReleaseComObject(info.pFilter);
			}
		}

		#endregion

		#region ピンの検索:

		/// <summary>
		/// ピンの検索
		/// </summary>
		/// <param name="filter">フィルタ</param>
		/// <param name="name">ピン名称</param>
		/// <returns>
		///		見つかった場合は、ピンの IPin インターフェースを返します。
		///		見つからなかった場合は、 null を返します。
		///	</returns>
		public static IPin FindPin(IBaseFilter filter, string name)
		{
			IEnumPins enumpins = null;
			IPin pin = null;

			try
			{
				filter.EnumPins(ref enumpins);

				int fetched = 0;
				while (enumpins.Next(1, ref pin, ref fetched) == 0)
				{
					if (fetched == 0) break;

					var info = new PIN_INFO();

					try
					{
						pin.QueryPinInfo(info);
						if (info.achName == name)
							return pin;

						if (pin != null)
							Marshal.ReleaseComObject(pin);
						pin = null;
					}
					finally
					{
						if (info.pFilter != null)
							Marshal.ReleaseComObject(info.pFilter);
					}
				}
			}
			catch
			{
				if (pin != null)
					Marshal.ReleaseComObject(pin);
				throw;
			}
			finally
			{
				if (enumpins != null)
					Marshal.ReleaseComObject(enumpins);
			}

			return null;
		}

		/// <summary>
		/// ピンの検索
		/// </summary>
		/// <param name="filter">フィルタ</param>
		/// <param name="index">番号 [0~]</param>
		/// <param name="direction">方向 (Unknown を指定した場合、方向に関係なく列挙されます)</param>
		/// <returns>
		///		見つかった場合は、ピンの IPin インターフェースを返します。
		///		見つからなかった場合は、 null を返します。
		///	</returns>
		public static IPin FindPin(IBaseFilter filter, int index, PIN_DIRECTION direction)
		{
			IEnumPins enumpins = null;
			IPin pin = null;

			try
			{
				filter.EnumPins(ref enumpins);

				int fetched = 0;
				while (enumpins.Next(1, ref pin, ref fetched) == 0)
				{
					if (fetched == 0) break;

					var info = new PIN_INFO();

					try
					{
						pin.QueryPinInfo(info);
						if (info.dir == direction)
						{
							if (index <= 0)
								return pin;
							index--;
						}

						if (pin != null)
							Marshal.ReleaseComObject(pin);
						pin = null;
					}
					finally
					{
						if (info.pFilter != null)
							Marshal.ReleaseComObject(info.pFilter);
					}
				}
			}
			catch
			{
				if (pin != null)
					Marshal.ReleaseComObject(pin);
				throw;
			}
			finally
			{
				if (enumpins != null)
					Marshal.ReleaseComObject(enumpins);
			}

			return null;
		}

		#endregion

		#region ビデオ情報の取得と設定:

		/// <summary>
		/// ビデオ情報の取得
		/// </summary>
		/// <param name="grabber">対象のサンプルグラバー</param>
		/// <returns>
		///		取得したビデオ情報(VIDEOINFOHEADER)を返します。
		/// </returns>
		public static VIDEOINFOHEADER GetVideoInfo(ISampleGrabber grabber)
		{
			var mt = new AM_MEDIA_TYPE();
			grabber.GetConnectedMediaType(mt);
			var vih = (VIDEOINFOHEADER)Marshal.PtrToStructure(mt.pbFormat, typeof(VIDEOINFOHEADER));
			return vih;
		}

		/// <summary>
		/// ビデオ情報の取得
		/// </summary>
		/// <param name="pin">対象のピン</param>
		/// <returns>
		///		取得したビデオ情報(VIDEOINFOHEADER)を返します。
		/// </returns>
		public static VIDEOINFOHEADER GetVideoInfo(IPin pin)
		{
			var mt = GetFormat(pin);
			var vih = (VIDEOINFOHEADER)Marshal.PtrToStructure(mt.pbFormat, typeof(VIDEOINFOHEADER));
			return vih;
		}

		/// <summary>
		/// ビデオ情報の設定
		/// </summary>
		/// <param name="pin">対象のピン</param>
		/// <param name="vih">ビデオ情報</param>
		public static void SetVideoInfo(IPin pin, VIDEOINFOHEADER vih)
		{
			var mt = GetFormat(pin);
			Marshal.StructureToPtr(vih, mt.pbFormat, true);
			SetFormat(pin, mt);
		}

		#endregion

		#region フォーマット形式の取得:

		/// <summary>
		/// サポートしているフォーマット形式の個数取得
		/// </summary>
		/// <param name="pin">ピン</param>
		/// <returns>
		///		サポートしているフォーマットの個数を返します。
		///	</returns>
		public static int GetFormatCount(IPin pin)
		{
			var config = pin as IAMStreamConfig;
			if (config == null)
				throw new DSLab.CxDSException(HRESULT.E_NOINTERFACE);

			int count = 0;
			int size = 0;
			config.GetNumberOfCapabilities(ref count, ref size);
			return count;
		}

		/// <summary>
		/// フォーマット形式の取得
		/// </summary>
		/// <param name="pin">ピン</param>
		/// <returns>
		///		現在のフォーマット形式を返します。
		///		取得に成功した場合、取得したデータを DeleteMediaType で解放する必要があります。
		///	</returns>
		public static AM_MEDIA_TYPE GetFormat(IPin pin)
		{
			var config = pin as IAMStreamConfig;
			if (config == null)
				throw new DSLab.CxDSException(HRESULT.E_NOINTERFACE);

			AM_MEDIA_TYPE mt = null;
			config.GetFormat(ref mt);
			return mt;
		}

		/// <summary>
		/// フォーマット形式の取得
		/// </summary>
		/// <param name="pin">ピン</param>
		/// <param name="index">フォーマット形式の番号</param>
		/// <returns>
		///		フォーマット情報を返します。
		///		取得に成功した場合、取得したデータを DeleteMediaType で解放する必要があります。
		/// </returns>
		public static AM_MEDIA_TYPE GetFormat(IPin pin, int index)
		{
			var config = pin as IAMStreamConfig;
			if (config == null)
				throw new DSLab.CxDSException(HRESULT.E_NOINTERFACE);

			IntPtr dataptr = IntPtr.Zero;
			try
			{
				// フォーマット個数取得.
				int count = 0;
				int size = 0;
				config.GetNumberOfCapabilities(ref count, ref size);

				// データ用領域確保.
				dataptr = Marshal.AllocHGlobal(size);

				// メディアタイプ取得.
				AM_MEDIA_TYPE mt = null;
				config.GetStreamCaps(index, ref mt, dataptr);
				return mt;
			}
			finally
			{
				if (dataptr == IntPtr.Zero)
					Marshal.FreeHGlobal(dataptr); 
			}
		}

		#endregion

		#region フォーマット形式の設定:

		/// <summary>
		/// フォーマット形式の設定
		/// </summary>
		/// <param name="pin">ピン</param>
		/// <param name="format">設定するフォーマット形式</param>
		public static void SetFormat(IPin pin, AM_MEDIA_TYPE format)
		{
			var config = pin as IAMStreamConfig;
			if (config == null)
				throw new DSLab.CxDSException(HRESULT.E_NOINTERFACE);
			config.SetFormat(format);
		}

		/// <summary>
		/// フォーマットサイズの設定
		/// </summary>
		/// <param name="pin">ピン</param>
		/// <param name="width">幅 [0,1~]</param>
		/// <param name="height">高さ [0,1~]</param>
		/// <remarks>
		///		width,height に 0,0 が指定されている場合は既定値を使用する.
		///		それ以外は、VIDEOINFOHEADER を書き換えて SetFormat を行う.
		///		http://msdn.microsoft.com/ja-jp/library/cc353344.aspx
		/// </remarks>
		public static void SetFormatSize(IPin pin, int width, int height)
		{
			if (width <= 0 || height <= 0) return;

			#region 手段1) IAMStreamConfig.GetNumberOfCapabilities で列挙して、該当する AM_MEDIA_TYPE を SetFormat する方法.
			if (pin is IAMStreamConfig)
			{
				var config = (IAMStreamConfig)pin;
				int count = 0;
				int size = 0;
				config.GetNumberOfCapabilities(ref count, ref size);

				if (size == Marshal.SizeOf(new VIDEO_STREAM_CONFIG_CAPS()))
				{
					for (int i = 0; i < count; i++)
					{
						var mt = new AM_MEDIA_TYPE();
						var vih = new VIDEOINFOHEADER();
						IntPtr addr = IntPtr.Zero;

						try
						{
							addr = Marshal.AllocCoTaskMem(size);
							int status = config.GetStreamCaps(i, ref mt, addr);
							if (status == (int)HRESULT.S_OK &&
								GUID.Compare(mt.majortype.ToString(), GUID.MEDIATYPE_Video) &&
								GUID.Compare(mt.formattype.ToString(), GUID.FORMAT_VideoInfo) &&
								mt.cbFormat >= Marshal.SizeOf(vih) &&
								mt.pbFormat != IntPtr.Zero
								)
							{
								vih = (VIDEOINFOHEADER)Marshal.PtrToStructure(mt.pbFormat, typeof(VIDEOINFOHEADER));

								// --- ビデオ入力サイズの確認.
								if (vih.bmiHeader.biWidth == width &&
									vih.bmiHeader.biHeight == height)
								{
									config.SetFormat(mt);
									return;
								}
							}
						}
						finally
						{
							if (addr != IntPtr.Zero)
								Marshal.FreeCoTaskMem(addr);
							Axi.FreeMediaType(ref mt);
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
				AM_MEDIA_TYPE mt = new AM_MEDIA_TYPE();
				VIDEOINFOHEADER video_info = new VIDEOINFOHEADER();

				mt = Axi.GetFormat(pin);
				video_info = (VIDEOINFOHEADER)Marshal.PtrToStructure(mt.pbFormat, typeof(VIDEOINFOHEADER));

				// --- ビデオ入力サイズ.
				video_info.bmiHeader.biWidth = width;
				video_info.bmiHeader.biHeight = height;

				// 2013.09.18(Wed): LifeCam (x86) でエラーが発生するので試したが効果は無かった.
				//video_info.bmiHeader.biBitCount = (short)BppIn;

				// 2013.09.18(Wed): lSampleSize を変更すると LifeCam (x86) でエラーが発生する.
				// --- サンプルサイズ.
				//int horz = System.Math.Abs(param.FilterFormatInfo.VideoSize.Width);
				//int vert = System.Math.Abs(param.FilterFormatInfo.VideoSize.Height);
				//int bpp = BppIn;
				//mt.lSampleSize = FVIL.Data.CFviImage.CalcHorzByte(horz, bpp) * (uint)vert;

				Marshal.StructureToPtr(video_info, mt.pbFormat, true);
				Axi.SetFormat(pin, mt);
			}
			#endregion
		}

		#endregion

		#region メディアタイプ関係:

		/// <summary>
		/// メディアタイプの領域の解放
		/// </summary>
		/// <param name="pointer">解放するメディアタイプのポインタ</param>
		public static void FreeMediaType(IntPtr pointer)
		{
			var mt = (AM_MEDIA_TYPE)Marshal.PtrToStructure(pointer, typeof(AM_MEDIA_TYPE));
			FreeMediaType(ref mt);
			Marshal.FreeCoTaskMem(pointer);
		}

		/// <summary>
		/// メディアタイプの領域の解放
		/// </summary>
		/// <param name="mt">解放するメディアタイプ</param>
		public static void FreeMediaType(ref AM_MEDIA_TYPE mt)
		{
			if (mt.lSampleSize != 0)
				Marshal.FreeCoTaskMem(mt.pbFormat);
			if (mt.pUnk != IntPtr.Zero)
				Marshal.FreeCoTaskMem(mt.pUnk);
			mt = null;
		}

		#endregion

		#region プロパティページ表示:

		/// <summary>
		/// フィルタ又はピンのプロパティページを表示します。
		/// </summary>
		/// <param name="hwnd">プロパティページダイアログのオーナーウィンドウハンドル</param>
		/// <param name="caption">ダイアログのタイトル</param>
		/// <param name="filter">フィルタ又はピン</param>
		/// <returns>
		///		正常に開かれた場合は true を返します。
		///		利用できない場合は false を返します。
		///		異常があれば例外を発行します。
		///	</returns>
		public static bool OpenPropertyDialog(IntPtr hwnd, string caption, object filter)
		{
			var spp = filter as ISpecifyPropertyPages;
			if (spp == null)
				return false;

			CAUUID ca = default(CAUUID);
			try
			{
				spp.GetPages(ref ca);
				DSLab.Win32API.OleCreatePropertyFrame(hwnd, 0, 0, caption, 1, ref filter, ca.cElems, ca.pElems, 0, 0, IntPtr.Zero);
			}
			finally
			{
				if (ca.pElems != IntPtr.Zero)
					Marshal.FreeCoTaskMem(ca.pElems); 
			}
			return true;
		}

		#endregion

		#region GraphEdit ファイル保存:

		/// <summary>
		/// GraphEdit ファイル保存
		/// </summary>
		/// <param name="graph"></param>
		/// <param name="filename"></param>
		/// <remarks>
		/// 	https://msdn.microsoft.com/ja-jp/library/Cc370617.aspx <br/>
		/// 	<br/>
		/// 	USB カメラ(VideoIputDevice)を接続したフィルタグラフを保存すると
		/// 	IPersistStream.Save で例外が発生します。
		/// 	原因は不明です。
		/// </remarks>
		public static void SaveGraphFile(IGraphBuilder graph, string filename)
		{
			HRESULT hr;

			IStorage storage = null;
			IStream stream = null;

			try
			{
				hr = (HRESULT)Win32API.StgCreateDocfile(
					filename,
					STGM.STGM_CREATE |
					STGM.STGM_TRANSACTED |
					STGM.STGM_READWRITE |
					STGM.STGM_SHARE_EXCLUSIVE,
					0, ref storage);
				if (hr != HRESULT.S_OK)
					throw new CxDSException(hr);

				hr = (HRESULT)storage.CreateStream(
					"ActiveMovieGraph",
					STGM.STGM_WRITE |
					STGM.STGM_CREATE |
					STGM.STGM_SHARE_EXCLUSIVE,
					0, 0, ref stream);
				if (hr != HRESULT.S_OK)
					throw new CxDSException(hr);

				var persist = (IPersistStream)graph;
				hr = (HRESULT)persist.Save(stream, true);	// (!) ここで例外が発生する.
				if (hr != HRESULT.S_OK)
					throw new CxDSException(hr);

				storage.Commit(STGC.STGC_DEFAULT);
			}
			finally
			{
				#region 解放:
				if (stream != null)
					Marshal.ReleaseComObject(stream);
				stream = null;

				if (storage != null)
					Marshal.ReleaseComObject(storage);
				storage = null;
				#endregion
			}
		}

		#endregion

		#region Windows Media Format 関連:

		/// <summary>
		/// WMV 形式ファイル保存用: ビデオのフレームサイズを設定します。
		/// </summary>
		/// <param name="config">ASF Writer から取得したインターフェース</param>
		/// <param name="size">設定値</param>
		/// <remarks>
		///		第一引数の <c>config</c> は下記のように取得してください。<br/>
		///		<example lang="c#">
		///			var filetype = new Guid(GUID.MEDIASUBTYPE_Asf);
		///			builder.SetOutputFileName(new IntPtr(&filetype), filename, ref videoRenderer, ref fileSink);
		///			var config = (IConfigAsfWriter)videoRenderer;
		///		</example>
		///		<br/>
		///		詳細は下記に記載されています。(但し、言語は C++ です。) <br/>
		///		<br/>
		///		<list type="bullet">
		///			<item>
		///				Capturing Video to a Windows Media File <br/>
		///				https://msdn.microsoft.com/en-us/library/windows/desktop/dd318630(v=vs.85).aspx
		///			</item>
		///			<item>
		///				Creating ASF Files in DirectShow <br/>
		///				https://msdn.microsoft.com/en-us/library/windows/desktop/dd375008(v=vs.85).aspx
		///				<list type="bullet">
		///				<item>
		///					Configuring the ASF Writer <br/>
		///					https://msdn.microsoft.com/en-us/library/windows/desktop/dd387911(v=vs.85).aspx
		///				</item>
		///				</list>
		///			</item>
		///			<item>
		///				Using Windows Media in DirectShow <br/>
		///				https://msdn.microsoft.com/en-us/library/windows/desktop/dd407300(v=vs.85).aspx
		///				<list type="bullet">
		///				<item>
		///					WM ASF Writer Filter <br/>
		///					https://msdn.microsoft.com/en-us/library/windows/desktop/dd390985(v=vs.85).aspx
		///				</item>
		///				</list>
		///			</item>
		///		</list>
		///		<br/>
		///		しかし、上記の説明だけでは解決できないので、下記も参考にしています。<br/>
		///		Social <br/>
		///		https://social.msdn.microsoft.com/Forums/sqlserver/en-US/962d4370-5fde-4d23-83bc-b8f6191b375e/asf-writer-creates-a-file-that-appears-not-to-match-the-encoding-profile-settings?forum=windowsdirectshowdevelopment <br/>
		///		https://social.msdn.microsoft.com/forums/windowsdesktop/en-us/fb0a6151-d916-422a-966b-ee1fb164df5f/asf-profile-configurations-fails <br/>
		/// </remarks>
		public static void SetVideoFrameSize(IConfigAsfWriter config, Size size)
		{
			try
			{
				HRESULT hr;

				// 現在のプロファイルを取得します.
				IWMProfile profile;
				hr = (HRESULT)config.GetCurrentProfile(out profile);
				if (hr < HRESULT.S_OK)
					throw new CxDSException(hr);

				// ストリームの個数を取得します.
				uint stream_num;
				hr = (HRESULT)profile.GetStreamCount(out stream_num);
				if (hr < HRESULT.S_OK)
					throw new CxDSException(hr);

				// 映像入力のストリームを探索してフレームサイズを設定します.
				for (uint index = 0; index < stream_num; index++)
				{
					// ストリームを取得します.
					IWMStreamConfig stream_config;
					hr = (HRESULT)profile.GetStream(index, out stream_config);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					// ストリームのメディアタイプを確認します.
					Guid stream_type;
					hr = (HRESULT)stream_config.GetStreamType(out stream_type);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					// 映像以外は無視します.
					if (stream_type != new Guid(GUID.MEDIATYPE_Video))
						continue;

					// メディアタイプ情報を格納するために必要なサイズ (bytes) を取得します.
					var props = (IWMMediaProps)stream_config;
					int cbType = 0;
					hr = (HRESULT)props.GetMediaType(IntPtr.Zero, ref cbType);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					// メディアタイプ情報を格納する領域を確保します.
					var pwmt = Marshal.AllocHGlobal(cbType);
					if (pwmt != IntPtr.Zero)
					{
						try
						{
							// メディアタイプ情報を取得します.
							hr = (HRESULT)props.GetMediaType(pwmt, ref cbType);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);

							// フレームサイズを指定された値に書き替えます.
							unsafe
							{
								var wmt = (WM_MEDIA_TYPE*)pwmt;
								var vih = (VIDEOINFOHEADER*)wmt->pbFormat;
								vih->bmiHeader.biWidth = size.Width;		// 幅.
								vih->bmiHeader.biHeight = size.Height;		// 高さ.
							}

							// メディアタイプを設定します.
							hr = (HRESULT)props.SetMediaType(pwmt);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);
						}
						finally
						{
							Marshal.FreeHGlobal(pwmt);
						}
					}

					// ストリームを再構成します.
					hr = (HRESULT)profile.ReconfigStream(stream_config);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);
				}

				// プロファイルを再構成します.
				hr = (HRESULT)config.ConfigureFilterUsingProfile(profile);
				if (hr < HRESULT.S_OK)
					throw new CxDSException(hr);
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		#endregion
	}
}
