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

using System.Runtime.InteropServices;

namespace DSLab
{
	// 
	// https://msdn.microsoft.com/ja-jp/library/cc354162.aspx
	// 

	#region グラフ関連.

	/// <summary>
	/// フィルタ グラフを作成するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a8689f-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFilterGraph
	{
		int AddFilter([In] IBaseFilter pFilter, [In, MarshalAs(UnmanagedType.LPWStr)] string pName);
		int RemoveFilter([In] IBaseFilter pFilter);
		int EnumFilters([In, Out] ref IEnumFilters ppEnum);
		int FindFilterByName(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pName,
			[In, Out] ref IBaseFilter ppFilter);
		int ConnectDirect(
			[In] IPin ppinOut,
			[In] IPin ppinIn,
			[In, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int Reconnect([In] IPin ppin);
		int Disconnect([In] IPin ppin);
		int SetDefaultSyncSource();
	}

	/// <summary>
	/// アプリケーションからフィルタ グラフを作成するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a868a9-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IGraphBuilder // : IFilterGraph
	{
		// IFilterGraph
		int AddFilter([In] IBaseFilter pFilter, [In, MarshalAs(UnmanagedType.LPWStr)] string pName);
		int RemoveFilter([In] IBaseFilter pFilter);
		int EnumFilters([In, Out] ref IEnumFilters ppEnum);
		int FindFilterByName(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pName,
			[In, Out] ref IBaseFilter ppFilter);
		int ConnectDirect(
			[In] IPin ppinOut,
			[In] IPin ppinIn,
			[In, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int Reconnect([In] IPin ppin);
		int Disconnect([In] IPin ppin);
		int SetDefaultSyncSource();

		// IGraphBuilder
		int Connect([In] IPin ppinOut, [In] IPin ppinIn);
		int Render([In] IPin ppinOut);
		int RenderFile(
			[In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFile,
			[In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrPlayList);
		int AddSourceFilter(
			[In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFileName,
			[In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFilterName,
			[In, Out] ref IBaseFilter ppFilter);
		int SetLogFile(IntPtr hFile);
		int Abort();
		int ShouldOperationContinue();
	}

	/// <summary>
	/// フィルタ グラフを通るデータ フローを制御するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a868b1-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IMediaControl
	{
		int Run();
		int Pause();
		int Stop();
		int GetState(int msTimeout, out int pfs);
		int RenderFile(string strFilename);
		int AddSourceFilter([In] string strFilename, [In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object ppUnk);
		int get_FilterCollection([In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object ppUnk);
		int get_RegFilterCollection([In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object ppUnk);
		int StopWhenReady();
	}

	/// <summary>
	/// ストリーム内の位置をシークするメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a868b2-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IMediaPosition
	{
		int get_Duration(ref double pLength);
		int put_CurrentPosition(double llTime);
		int get_CurrentPosition(ref double pllTime);
		int get_StopTime(ref double pllTime);
		int put_StopTime(double llTime);
		int get_PrerollTime(ref double pllTime);
		int put_PrerollTime(double llTime);
		int put_Rate(double dRate);
		int get_Rate(ref double pdRate);
		int CanSeekForward(ref int pCanSeekForward);
		int CanSeekBackward(ref int pCanSeekBackward);
	}

	/// <summary>
	/// ビデオ ウィンドウのプロパティを設定するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a868b4-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IVideoWindow
	{
		int put_Caption(string caption);
		int get_Caption([In, Out] ref string caption);
		int put_WindowStyle(int windowStyle);
		int get_WindowStyle(ref int windowStyle);
		int put_WindowStyleEx(int windowStyleEx);
		int get_WindowStyleEx(ref int windowStyleEx);
		int put_AutoShow(int autoShow);
		int get_AutoShow(ref int autoShow);
		int put_WindowState(int windowState);
		int get_WindowState(ref int windowState);
		int put_BackgroundPalette(int backgroundPalette);
		int get_BackgroundPalette(ref int backgroundPalette);
		int put_Visible(int visible);
		int get_Visible(ref int visible);
		int put_Left(int left);
		int get_Left(ref int left);
		int put_Width(int width);
		int get_Width(ref int width);
		int put_Top(int top);
		int get_Top(ref int top);
		int put_Height(int height);
		int get_Height(ref int height);
		int put_Owner(IntPtr owner);
		int get_Owner(ref IntPtr owner);
		int put_MessageDrain(IntPtr drain);
		int get_MessageDrain(ref IntPtr drain);
		int get_BorderColor(ref int color);
		int put_BorderColor(int color);
		int get_FullScreenMode(ref int fullScreenMode);
		int put_FullScreenMode(int fullScreenMode);
		int SetWindowForeground(int focus);
		int NotifyOwnerMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
		int SetWindowPosition(int left, int top, int width, int height);
		int GetWindowPosition(ref int left, ref int top, ref int width, ref int height);
		int GetMinIdealImageSize(ref int width, ref int height);
		int GetMaxIdealImageSize(ref int width, ref int height);
		int GetRestorePosition(ref int left, ref int top, ref int width, ref int height);
		int HideCursor(int HideCursorValue);
		int IsCursorHidden(ref int hideCursor);
	}

	/// <summary>
	/// ビデオ ストリームのプロパティを設定するメソッドを提供するインタフェース.
	/// </summary>
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("56a868b5-0ad4-11ce-b03a-0020af0ba770")]
	public interface IBasicVideo
	{
		void get_AvgTimePerFrame(ref double pAvgTimePerFrame);
		void get_BitRate(ref int pBitRate);
		void get_BitErrorRate(ref int pBitErrorRate);
		void get_VideoWidth(ref int pVideoWidth);
		void get_VideoHeight(ref int pVideoHeight);
		void put_SourceLeft(int SourceLeft);
		void get_SourceLeft(ref int pSourceLeft);
		void put_SourceWidth(int SourceWidth);
		void get_SourceWidth(ref int pSourceWidth);
		void put_SourceTop(int SourceTop);
		void get_SourceTop(ref int pSourceTop);
		void put_SourceHeight(int SourceHeight);
		void get_SourceHeight(ref int pSourceHeight);
		void put_DestinationLeft(int DestinationLeft);
		void get_DestinationLeft(ref int pDestinationLeft);
		void put_DestinationWidth(int DestinationWidth);
		void get_DestinationWidth(ref int pDestinationWidth);
		void put_DestinationTop(int DestinationTop);
		void get_DestinationTop(ref int pDestinationTop);
		void put_DestinationHeight(int DestinationHeight);
		void get_DestinationHeight(ref int pDestinationHeight);
		void SetSourcePosition(int Left, int Top, int Width, int Height);
		void GetSourcePosition(ref int pLeft, ref int pTop, ref int pWidth, ref int pHeight);
		void SetDefaultSourcePosition();
		void SetDestinationPosition(int Left, int Top, int Width, int Height);
		void GetDestinationPosition(ref int pLeft, ref int pTop, ref int pWidth, ref int pHeight);
		void SetDefaultDestinationPosition();
		void GetVideoSize(ref int pWidth, ref int pHeight);
		void GetVideoPaletteEntries(int StartIndex, int Entries, ref int pRetrieved, ref int pPalette);
		void GetCurrentImage(ref int pBufferSize, ref int pDIBImage);
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsUsingDefaultSource();
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsUsingDefaultDestination();
	}

	/// <summary>
	/// ビデオ ストリームのプロパティを設定するメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	/// IBasicVideo2 インターフェイスは、IBasicVideo インターフェイスから派生したインターフェイスで、
	/// ビデオ ストリームのアスペクト比を取得する別の方法をアプリケーションに提供する。
	/// このインターフェイスはビデオ レンダラ フィルタに実装されるが、フィルタ グラフ マネージャでアプリケーションに公開される。
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("329bb360-f6ea-11d1-9038-00a0c9697298"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IBasicVideo2
	{
		int AvgTimePerFrame(ref double pAvgTimePerFrame);
		int BitRate(ref int pBitRate);
		int BitErrorRate(ref int pBitRate);
		int VideoWidth(ref int pVideoWidth);
		int VideoHeight(ref int pVideoHeight);
		int put_SourceLeft(int SourceLeft);
		int get_SourceLeft(ref int pSourceLeft);
		int put_SourceWidth(int SourceWidth);
		int get_SourceWidth(ref int pSourceWidth);
		int put_SourceTop(int SourceTop);
		int get_SourceTop(ref int pSourceTop);
		int put_SourceHeight(int SourceHeight);
		int get_SourceHeight(ref int pSourceHeight);
		int put_DestinationLeft(int DestinationLeft);
		int get_DestinationLeft(ref int pDestinationLeft);
		int put_DestinationWidth(int DestinationWidth);
		int get_DestinationWidth(ref int pDestinationWidth);
		int put_DestinationTop(int DestinationTop);
		int get_DestinationTop(ref int pDestinationTop);
		int put_DestinationHeight(int DestinationHeight);
		int get_DestinationHeight(ref int pDestinationHeight);
		int SetSourcePosition(int left, int top, int width, int height);
		int GetSourcePosition(ref int left, ref int top, ref int width, ref int height);
		int SetDefaultSourcePosition();
		int SetDestinationPosition(int left, int top, int width, int height);
		int GetDestinationPosition(ref int left, ref int top, ref int width, ref int height);
		int SetDefaultDestinationPosition();
		int GetVideoSize(ref int pWidth, ref int pHeight);
		int GetVideoPaletteEntries(int StartIndex, int Entries, ref int pRetrieved, IntPtr pPalette);
		int GetCurrentImage(ref int pBufferSize, IntPtr pDIBImage);
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsUsingDefaultSource();
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsUsingDefaultDestination();

		/// <summary>
		/// ムービーの優先アスペクト比を取得する。(追加されたメソッド)
		/// </summary>
		/// <param name="plAspectX"></param>
		/// <param name="plAspectY"></param>
		/// <returns>
		/// </returns>
		int GetPreferredAspectRatio(ref int plAspectX, ref int plAspectY);
	}

	/// <summary>
	/// オーディオ ストリームのボリュームとバランスを制御するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a868b3-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IBasicAudio
	{
		int put_Volume(int lVolume);
		int get_Volume(ref int plVolume);
		int put_Balance(int lBalance);
		int get_Balance(ref int plBalance);
	}

	/// <summary>
	/// イベント通知を取得するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a868b6-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IMediaEvent
	{
		int GetEventHandle(ref IntPtr hEvent);
		int GetEvent(ref EventCode lEventCode, ref int lParam1, ref int lParam2, int msTimeout);
		int WaitForCompletion(int msTimeout, out int pEvCode);
		int CancelDefaultHandling(int lEvCode);
		int RestoreDefaultHandling(int lEvCode);
		int FreeEventParams(EventCode lEvCode, int lParam1, int lParam2);
	}

	/// <summary>
	///  イベント通知を取得するメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	/// IMediaEvent インターフェイスを継承します。
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("56a868c0-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IMediaEventEx // : IMediaEvent
	{
		// IMediaEvent
		int GetEventHandle(ref IntPtr hEvent);
		int GetEvent(ref EventCode lEventCode, ref int lParam1, ref int lParam2, int msTimeout);
		int WaitForCompletion(int msTimeout, out int pEvCode);
		int CancelDefaultHandling(int lEvCode);
		int RestoreDefaultHandling(int lEvCode);
		int FreeEventParams(EventCode lEvCode, int lParam1, int lParam2);

		// IMediaEventEx
		int SetNotifyWindow(IntPtr hwnd, int lMsg, IntPtr lInstanceData);
		int SetNotifyFlags(int lNoNotifyFlags);
		int GetNotifyFlags(ref int lplNoNotifyFlags);
	}

	/// <summary>
	/// ストリーム内の位置にシークするメソッドと、再生レートを設定するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("36b73880-c2c8-11cf-8b46-00805f6cef60"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMediaSeeking
	{
		int GetCapabilities(ref AM_SEEKING_SeekingCapabilities pCapabilities);
		int CheckCapabilities([In, Out] ref AM_SEEKING_SeekingCapabilities pCapabilities);
		int IsFormatSupported([In] ref Guid pFormat);
		int QueryPreferredFormat([In, Out] ref Guid pFormat);
		int GetTimeFormat([In, Out] ref Guid pFormat);
		int IsUsingTimeFormat([In] ref Guid pFormat);
		int SetTimeFormat([In] ref Guid pFormat);
		int GetDuration(ref long pDuration);
		int GetStopPosition(ref long pStop);
		int GetCurrentPosition(ref long pCurrent);
		int ConvertTimeFormat(ref long pTarget, [In] ref Guid pTargetFormat, long Source, [In] ref Guid pSourceFormat);
		int SetPositions([In] ref long pCurrent, AM_SEEKING_SEEKING_FLAGS dwCurrentFlags, [In] ref long pStop, AM_SEEKING_SEEKING_FLAGS dwStopFlags);
		int GetPositions(ref long pCurrent, ref long pStop);
		int GetAvailable(ref long pEarliest, ref long pLatest);
		int SetRate(double dRate);
		int GetRate(ref double pdRate);
		int GetPreroll(ref long pllPreroll);
	}

	/// <summary>
	/// キャプチャ グラフやその他のカスタム フィルタ グラフを構築するメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	///		このインターフェースは GUID へのポインタの部分を IntPtr にしています。
	///		GUID に NULL (IntPtr.Zero) を指定する必要があれば、このインターフェースを使用してください。
	///		そうでなければ ICaptureGraphBuilder2Ex を使用して構いません。
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("93E5A4E0-2D50-11d2-ABFA-00A0C9C6E38D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICaptureGraphBuilder2
	{
		int SetFiltergraph([In] IGraphBuilder pfg);
		int GetFiltergraph([In, Out] ref IGraphBuilder ppfg);
		int SetOutputFileName([In] IntPtr pType, [In, MarshalAs(UnmanagedType.LPWStr)] string lpstrFile, [In, Out] ref IBaseFilter ppbf, [In, Out] ref IFileSinkFilter ppSink);
		int FindInterface([In] IntPtr pCategory, [In] IntPtr pType, [In] IBaseFilter pbf, [In] IntPtr riid, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object ppint);
		int RenderStream([In] IntPtr pCategory, [In] IntPtr pType, [In, MarshalAs(UnmanagedType.IUnknown)] object pSource, [In] IBaseFilter pfCompressor, [In] IBaseFilter pfRenderer);
		int ControlStream([In] IntPtr pCategory, [In] IntPtr pType, [In] IBaseFilter pFilter, [In] IntPtr pstart, [In] IntPtr pstop, [In] short wStartCookie, [In] short wStopCookie);
		int AllocCapFile([In, MarshalAs(UnmanagedType.LPWStr)] string lpstrFile, [In] long dwlSize);
		int CopyCaptureFile([In, MarshalAs(UnmanagedType.LPWStr)] string lpwstrOld, [In, MarshalAs(UnmanagedType.LPWStr)] string lpwstrNew, [In] int fAllowEscAbort, [In] IAMCopyCaptureFileProgress pFilter);
		int FindPin([In] object pSource, [In] int pindir, [In] IntPtr pCategory, [In] IntPtr pType, [In, MarshalAs(UnmanagedType.Bool)] bool fUnconnected, [In] int num, [Out] out IntPtr ppPin);
	}

	/// <summary>
	/// キャプチャ グラフやその他のカスタム フィルタ グラフを構築するメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	///		このインターフェースは GUID へのポインタの部分を ref Guid にしています。
	///		GUID に NULL (IntPtr.Zero) を指定する必要があれば、ICaptureGraphBuilder2 を使用してください。
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("93E5A4E0-2D50-11d2-ABFA-00A0C9C6E38D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICaptureGraphBuilder2Ex
	{
		int SetFiltergraph([In] IGraphBuilder pfg);
		int GetFiltergraph([In, Out] ref IGraphBuilder ppfg);
		int SetOutputFileName([In] ref Guid pType, [In, MarshalAs(UnmanagedType.LPWStr)] string lpstrFile, [In, Out] ref IBaseFilter ppbf, [In, Out] ref IFileSinkFilter ppSink);
		int FindInterface([In] ref Guid pCategory, [In] ref Guid pType, [In] IBaseFilter pbf, [In] ref Guid riid, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object ppint);
		int RenderStream([In] ref Guid pCategory, [In] ref Guid pType, [In, MarshalAs(UnmanagedType.IUnknown)] object pSource, [In] IBaseFilter pfCompressor, [In] IBaseFilter pfRenderer);
		int ControlStream([In] ref Guid pCategory, [In] ref Guid pType, [In] IBaseFilter pFilter, [In] IntPtr pstart, [In] IntPtr pstop, [In] short wStartCookie, [In] short wStopCookie);
		int AllocCapFile([In, MarshalAs(UnmanagedType.LPWStr)] string lpstrFile, [In] long dwlSize);
		int CopyCaptureFile([In, MarshalAs(UnmanagedType.LPWStr)] string lpwstrOld, [In, MarshalAs(UnmanagedType.LPWStr)] string lpwstrNew, [In] int fAllowEscAbort, [In] IAMCopyCaptureFileProgress pFilter);
		int FindPin([In] object pSource, [In] int pindir, [In] ref Guid pCategory, [In] ref Guid pType, [In, MarshalAs(UnmanagedType.Bool)] bool fUnconnected, [In] int num, [Out] out IntPtr ppPin);
	}

	/// <summary>
	/// メディアサンプルの出力先ファイル名を設定または取得するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("a2104830-7c70-11cf-8bce-00aa00a3f1a6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFileSinkFilter
	{
		int SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [In, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int GetCurFile([In, Out, MarshalAs(UnmanagedType.LPWStr)] ref string pszFileName, [Out, MarshalAs(UnmanagedType.LPStruct)] out AM_MEDIA_TYPE pmt);
	}

	/// <summary>
	/// メディアサンプルの出力先ファイル名を設定または取得するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("00855B90-CE1B-11d0-BD4F-00A0C911CE86"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFileSinkFilter2
	{
		// IFileSinkFilter
		int SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [In, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int GetCurFile([In, Out, MarshalAs(UnmanagedType.LPWStr)] ref string pszFileName, [Out, MarshalAs(UnmanagedType.LPStruct)] out AM_MEDIA_TYPE pmt);

		// IFileSinkFilter2
		int SetMode([In] uint dwFlags);
		int GetMode([Out] out uint dwFlags);
	}

	/// <summary>
	/// レンダリングするメディアファイルのファイル名およびメディアタイプを設定するメソッドを提供するインターフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a868a6-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFileSourceFilter
	{
		int Load([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [In] IntPtr pmt);
		int GetCurFile([In, Out, MarshalAs(UnmanagedType.LPWStr)] string ppszFileName, [Out] IntPtr pmt);
	}

	/// <summary>
	/// ICaptureGraphBuilder2::CopyCaptureFile メソッドが使うコールバック インターフェイス.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("670d1d20-a068-11d0-b3f0-00aa003761c5"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAMCopyCaptureFileProgress
	{
		int Progress(int iProgress);
	}

	#endregion

	#region グラフ関連: カメラ制御インターフェース.

	/// <summary>
	/// カメラを制御するメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	/// see: http://msdn.microsoft.com/ja-jp/library/cc354791.aspx
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("C6E13370-30AC-11d0-A18C-00A0C9118956"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAMCameraControl
	{
		/// <summary>
		/// 範囲と既定値の取得
		/// </summary>
		/// <param name="Property">問い合わせるプロパティの種別</param>
		/// <param name="pMin">プロパティの最小値</param>
		/// <param name="pMax">プロパティの最大値</param>
		/// <param name="pSteppingDelta">プロパティのステップサイズ</param>
		/// <param name="pDefault">プロパティの既定値</param>
		/// <param name="pCapsFlag">プロパティの自動制御/手動制御を示す値</param>
		/// <returns>
		/// メソッドが成功すると、S_OK を返します。それ以外の場合は HRESULT エラーコードを返します。
		/// </returns>
		int GetRange([In] CameraControlProperty Property, [In, Out] ref int pMin, [In, Out] ref int pMax, [In, Out] ref int pSteppingDelta, [In, Out] ref int pDefault, [In, Out] ref int pCapsFlag);

		/// <summary>
		/// プロパティ値の設定
		/// </summary>
		/// <param name="Property">問い合わせるプロパティの種別</param>
		/// <param name="lValue">プロパティの値 (Flags 引数が Auto の場合は無視します。)</param>
		/// <param name="Flags">プロパティの自動制御/手動制御を示す値</param>
		/// <returns>
		/// メソッドが成功すると、S_OK を返します。それ以外の場合は HRESULT エラーコードを返します。
		/// </returns>
		int Set([In] CameraControlProperty Property, [In] int lValue, [In] int Flags);

		/// <summary>
		/// プロパティ値の取得
		/// </summary>
		/// <param name="Property">問い合わせるプロパティの種別</param>
		/// <param name="lValue">プロパティの値</param>
		/// <param name="Flags">プロパティの自動制御/手動制御を示す値</param>
		/// <returns>
		/// メソッドが成功すると、S_OK を返します。それ以外の場合は HRESULT エラーコードを返します。
		/// </returns>
		int Get([In] CameraControlProperty Property, [In, Out] ref int lValue, [In, Out] ref int Flags);
	}

	#endregion

	#region グラフ関連: ビデオ品質調整インターフェース.

	/// <summary>
	/// ビデオ信号品質を調整するメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	/// see: http://msdn.microsoft.com/ja-jp/library/cc355361.aspx
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("C6E13360-30AC-11d0-A18C-00A0C9118956"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAMVideoProcAmp
	{
		/// <summary>
		/// 範囲と既定値の取得
		/// </summary>
		/// <param name="Property">問い合わせるプロパティの種別</param>
		/// <param name="pMin">プロパティの最小値</param>
		/// <param name="pMax">プロパティの最大値</param>
		/// <param name="pSteppingDelta">プロパティのステップサイズ</param>
		/// <param name="pDefault">プロパティの既定値</param>
		/// <param name="pCapsFlag">プロパティの自動制御/手動制御を示す値</param>
		/// <returns>
		/// <list type="table">
		/// <listheader>
		///		<term>戻り値</term>
		///		<desc>説明</desc>
		/// </listheader>
		/// <item>
		///		<term>S_OK</term>
		///		<desc>正常</desc>
		/// </item>
		/// <item>
		///		<term>E_PROP_ID_UNSUPPORTED</term>
		///		<desc>このデバイスはこのメソッドをサポートしていません。</desc>
		/// </item>
		/// <item>
		///		<term>E_POINTER</term>
		///		<desc>引数に NULL ポインタが指定されました。</desc>
		/// </item>
		/// <item>
		///		<term>E_INVALIDARG</term>
		///		<desc>引数に指定された値が無効です。</desc>
		/// </item>
		/// </list>
		/// </returns>
		int GetRange([In] VideoProcAmpProperty Property, [In, Out] ref int pMin, [In, Out] ref int pMax, [In, Out] ref int pSteppingDelta, [In, Out] ref int pDefault, [In, Out] ref int pCapsFlag);

		/// <summary>
		/// プロパティ値の設定
		/// </summary>
		/// <param name="Property">問い合わせるプロパティの種別</param>
		/// <param name="lValue">プロパティの値 (Flags 引数が Auto の場合は無視します。)</param>
		/// <param name="Flags">プロパティの自動制御/手動制御を示す値</param>
		/// <returns>
		/// メソッドが成功すると、S_OK を返します。それ以外の場合は HRESULT エラーコードを返します。
		/// </returns>
		int Set([In] VideoProcAmpProperty Property, [In] int lValue, [In] int Flags);

		/// <summary>
		/// プロパティ値の取得
		/// </summary>
		/// <param name="Property">問い合わせるプロパティの種別</param>
		/// <param name="lValue">プロパティの値</param>
		/// <param name="Flags">プロパティの自動制御/手動制御を示す値</param>
		/// <returns>
		/// メソッドが成功すると、S_OK を返します。それ以外の場合は HRESULT エラーコードを返します。
		/// </returns>
		int Get([In] VideoProcAmpProperty Property, [In, Out] ref int lValue, [In, Out] ref int Flags);
	}

	#endregion

	#region グラフ関連: フリッピング／外部トリガを提供するインターフェース.

	/// <summary>
	/// フリッピング／外部トリガを提供するインターフェース
	/// </summary>
	/// <remarks>
	///	see: http://msdn.microsoft.com/ja-jp/library/cc355345.aspx
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("6A2E0670-28E4-11D0-A18C-00A0C9118956"), System.Security.SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAMVideoControl
	{
		/// <summary>
		/// 使用するハードウェアの能力を取得する。
		/// </summary>
		/// <param name="pPin">機能のクエリ対象となるピンへのポインタ。</param>
		/// <param name="pCapsFlags">VideoControlFlags 列挙型のフラグの組み合わせを表す値へのポインタ。</param>
		/// <returns>
		///		インターフェイスの実装に応じた HRESULT 値を返す。
		///		取得できる能力には、水平方向へのピクチャのフリッピング、垂直方向へのピクチャのフリッピング、外部トリガの有効化、外部トリガのシミュレーションのうち、1 つまたは複数が含まれる。
		/// </returns>
		[PreserveSig]
		int GetCaps(
			[In] IPin pPin,
			[Out] out int pCapsFlags
			);

		/// <summary>
		/// 操作のビデオ制御モードを設定する。
		/// </summary>
		/// <param name="pPin">ビデオ制御モードの設定対象となるピンへのポインタ。</param>
		/// <param name="Mode">ビデオ制御モードを設定する VideoControlFlags 列挙型のフラグの組み合わせを指定する値。</param>
		/// <returns>
		///		インターフェイスの実装に応じた HRESULT 値を返す。
		///		取得できる操作モードには、水平方向へのピクチャのフリッピング、垂直方向へのピクチャのフリッピング、外部トリガの有効化、外部トリガのシミュレーションのうち、1 つまたは複数が含まれる。
		/// </returns>
		[PreserveSig]
		int SetMode(
			[In] IPin pPin,
			[In] int Mode
			);

		/// <summary>
		/// 操作のビデオ制御モードを取得する。
		/// </summary>
		/// <param name="pPin">ビデオ制御モードの取得対象となるピンへのポインタ。</param>
		/// <param name="Mode">ビデオ制御モードを指定する VideoControlFlags 列挙型のフラグの組み合わせを表す値へのポインタ。</param>
		/// <returns>
		///		インターフェイスの実装に応じた HRESULT 値を返す。
		///		取得できる操作モードには、水平方向へのピクチャのフリッピング、垂直方向へのピクチャのフリッピング、外部トリガの有効化、外部トリガのシミュレーションのうち、1 つまたは複数が含まれる。
		/// </returns>
		[PreserveSig]
		int GetMode(
			[In] IPin pPin,
			[Out] out int Mode
			);

		/// <summary>
		/// デバイスがストリーミングしている実際のフレームレートを取得する。
		/// </summary>
		/// <param name="pPin">フレーム レートの取得対象となるピンへのポインタ。</param>
		/// <param name="ActualFrameRate">フレーム時間幅で表されるフレーム レートへのポインタ (100 ナノ秒単位)。</param>
		/// <returns>
		///		インターフェイスの実装に応じた HRESULT 値を返す。
		/// </returns>
		[PreserveSig]
		int GetCurrentActualFrameRate(
			[In] IPin pPin,
			[Out] out long ActualFrameRate
			);

		/// <summary>
		/// 現在利用可能な最大フレームレートを取得する。
		/// </summary>
		/// <param name="pPin">最大フレーム レートの取得対象となるピンへのポインタ。</param>
		/// <param name="iIndex">最大フレーム レートのクエリ対象となるフォーマットのインデックス。このインデックスは、フォーマットが IAMStreamConfig::GetStreamCaps で列挙される順序に対応している。値の範囲は、0 から MStreamConfig::GetNumberOfCapabilities メソッドが返す、サポートされている VIDEO_STREAM_CONFIG_CAPS 構造体の数から 1 を減算した値までである。</param>
		/// <param name="Dimensions">フレーム イメージのピクセル単位のサイズ (幅と高さ)。</param>
		/// <param name="MaxAvailableFrameRate">利用可能な最大フレーム レートへのポインタ。フレーム レートは、100 ナノ秒単位のフレーム時間幅として表される。</param>
		/// <returns>
		///		インターフェイスの実装に応じた HRESULT 値を返す。
		/// </returns>
		[PreserveSig]
		int GetMaxAvailableFrameRate(
			[In] IPin pPin,
			[In] int iIndex,
			[In] Size Dimensions,
			[Out] out long MaxAvailableFrameRate
			);

		/// <summary>
		/// 利用可能なフレームレートのリストを取得する。
		/// </summary>
		/// <param name="pPin">フレーム レートのリストのクエリ対象となるピンへのポインタ。</param>
		/// <param name="iIndex">フレーム レートのクエリ対象となるフォーマットのインデックス。このインデックスは、フォーマットが IAMStreamConfig::GetStreamCaps で列挙される順序に対応している。値の範囲は、0 から MStreamConfig::GetNumberOfCapabilities メソッドが返す、サポートされている VIDEO_STREAM_CONFIG_CAPS 構造体の数から 1 を減算した値までである。</param>
		/// <param name="Dimensions">フレーム イメージのピクセル単位のサイズ (幅と高さ)。</param>
		/// <param name="ListSize">フレーム レートのリスト内の要素数へのポインタ。</param>
		/// <param name="FrameRates">100 ナノ秒単位のフレーム レートの配列へのポインタのアドレス。ListSize だけを必要とする場合、このデータは NULL でもよい。</param>
		/// <returns>
		///		インターフェイスの実装に応じた HRESULT 値を返す。
		///		呼び出し元は、CoTaskMemFree を呼び出してメモリを解放する必要がある。
		/// </returns>
		[PreserveSig]
		int GetFrameRateList(
			[In] IPin pPin,
			[In] int iIndex,
			[In] Size Dimensions,
			[Out] out int ListSize,
			[Out] out IntPtr FrameRates
			);
	}
	#endregion

	#region フィルタ関連:

	/// <summary>
	/// フィルタを制御するためのメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a86895-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBaseFilter
	{
		// Inherits IPersist
		int GetClassID([Out] out Guid pClassID);

		// Inherits IMediaControl
		int Stop();
		int Pause();
		int Run(long tStart);
		int GetState(int dwMilliSecsTimeout, [In, Out] ref int filtState);
		int SetSyncSource([In] IReferenceClock pClock);
		int GetSyncSource([In, Out] ref IReferenceClock pClock);

		// -----
		int EnumPins([In, Out] ref IEnumPins ppEnum);
		int FindPin([In, MarshalAs(UnmanagedType.LPWStr)] string Id, [In, Out] ref IPin ppPin);
		int QueryFilterInfo([Out] FILTER_INFO pInfo);
		int JoinFilterGraph([In] IFilterGraph pGraph, [In, MarshalAs(UnmanagedType.LPWStr)] string pName);
		int QueryVendorInfo([In,Out, MarshalAs(UnmanagedType.LPWStr)] ref string pVendorInfo);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// https://msdn.microsoft.com/ja-jp/library/microsoft.visualstudio.ole.interop.ipersist.aspx
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("0000010c-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersist
	{
		int GetClassID([Out] out Guid pClassID);
	}

	/// <summary>
	/// フィルタ状態の切り替えやフィルタの現在の状態を取得するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a86899-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMediaFilter
	{
		// Inherits IPersist
		int GetClassID([Out] out Guid pClassID);	// override

		// IMediaFilter
		int Stop();
		int Pause();
		int Run(long tStart);
		int GetState(int dwMilliSecsTimeout, ref int filtState);
		int SetSyncSource([In] IReferenceClock pClock);
		int GetSyncSource([In, Out] ref IReferenceClock pClock);
	}

	/// <summary>
	/// フィルタ グラフ内のフィルタを列挙するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a86893-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumFilters
	{
		int Next([In] int cFilters, [In, Out] ref IBaseFilter ppFilter, [In, Out] ref int pcFetched);
		int Skip([In] int cFilters);
		void Reset();
		void Clone([In, Out] ref IEnumFilters ppEnum);
	}

	/// <summary>
	/// オーディオおよびビデオの出力フォーマットを設定するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("C6E13340-30AC-11d0-A18C-00A0C9118956"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAMStreamConfig
	{
		int SetFormat([In, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int GetFormat([In, Out, MarshalAs(UnmanagedType.LPStruct)] ref AM_MEDIA_TYPE ppmt);
		int GetNumberOfCapabilities(ref int piCount, ref int piSize);
		int GetStreamCaps(int iIndex, [In, Out, MarshalAs(UnmanagedType.LPStruct)] ref AM_MEDIA_TYPE ppmt, IntPtr pSCC);
	}

	/// <summary>
	/// 再生する論理ストリームの制御、および、それに関する情報を取得するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("c1960960-17f5-11d1-abe1-00a0c905f375"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAMStreamSelect
	{
		int Count(ref int Streams);
		int Info(int Index, [In, Out, MarshalAs(UnmanagedType.LPStruct)] ref AM_MEDIA_TYPE ppmt, ref int Flags, IntPtr lcid, ref int Group, ref IntPtr Name, ref IntPtr Obj, ref IntPtr ppUnk);
		int Enable(int Index, int Flag);
	}

	/// <summary>
	/// メディア サンプルのプロパティを設定および取得するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a8689a-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMediaSample
	{
		int GetPointer(ref IntPtr ppBuffer);
		int GetSize();
		int GetTime(ref long pTimeStart, ref long pTimeEnd);
		int SetTime([In, MarshalAs(UnmanagedType.LPStruct)] UInt64 pTimeStart, [In, MarshalAs(UnmanagedType.LPStruct)] UInt64 pTimeEnd);
		int IsSyncPoint();
		int SetSyncPoint([In, MarshalAs(UnmanagedType.Bool)] bool bIsSyncPoint);
		int IsPreroll();
		int SetPreroll([In, MarshalAs(UnmanagedType.Bool)] bool bIsPreroll);
		int GetActualDataLength();
		int SetActualDataLength(int len);
		int GetMediaType([In, Out, MarshalAs(UnmanagedType.LPStruct)] ref AM_MEDIA_TYPE ppMediaType);
		int SetMediaType([In, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pMediaType);
		int IsDiscontinuity();
		int SetDiscontinuity([In, MarshalAs(UnmanagedType.Bool)] bool bDiscontinuity);
		int GetMediaTime(ref long pTimeStart, ref long pTimeEnd);
		int SetMediaTime([In, MarshalAs(UnmanagedType.LPStruct)] UInt64 pTimeStart, [In, MarshalAs(UnmanagedType.LPStruct)] UInt64 pTimeEnd);
	}

	/// <summary>
	/// ピンの優先されるメディア タイプを列挙するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("89c31040-846b-11ce-97d3-00aa0055595a"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumMediaTypes
	{
		int Next([In] int cMediaTypes, [In, Out, MarshalAs(UnmanagedType.LPStruct)] ref AM_MEDIA_TYPE ppMediaTypes, [In, Out] ref int pcFetched);
		int Skip([In] int cMediaTypes);
		int Reset();
		int Clone([In, Out] ref IEnumMediaTypes ppEnum);
	}

	/// <summary>
	/// ピンを接続するメソッドやピンの情報を取得するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a86891-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPin
	{
		int Connect([In] IPin pReceivePin, [In, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int ReceiveConnection([In] IPin pReceivePin, [In, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int Disconnect();
		int ConnectedTo([In, Out] ref IPin ppPin);
		int ConnectionMediaType([Out, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int QueryPinInfo([Out] PIN_INFO pInfo);
		int QueryDirection(ref PIN_DIRECTION pPinDir);
		int QueryId([In, Out, MarshalAs(UnmanagedType.LPWStr)] ref string Id);
		int QueryAccept([In, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int EnumMediaTypes([In, Out] ref IEnumMediaTypes ppEnum);
		int QueryInternalConnections(IntPtr apPin, [In, Out] ref int nPin);
		int EndOfStream();
		int BeginFlush();
		int EndFlush();
		int NewSegment(long tStart, long tStop, double dRate);
	}

	/// <summary>
	/// フィルタ上のピンを列挙するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a86892-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumPins
	{
		int Next([In] int cPins, [In, Out] ref IPin ppPins, [In, Out] ref int pcFetched);
		int Skip([In] int cPins);
		void Reset();
		void Clone([In, Out] ref IEnumPins ppEnum);
	}

	/// <summary>
	/// 基準タイムを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("56a86897-0ad4-11ce-b03a-0020af0ba770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IReferenceClock
	{
		int GetTime(ref long pTime);
		int AdviseTime(long baseTime, long streamTime, IntPtr hEvent, ref int pdwAdviseCookie);
		int AdvisePeriodic(long startTime, long periodTime, IntPtr hSemaphore, ref int pdwAdviseCookie);
		int Unadvise(int dwAdviseCookie);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// dmodshow.h [C:\Program Files (x86)\Microsoft SDKs\v7.1A\Include]
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("52d6f586-9f0f-4824-8fc8-e32ca04930c2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDMOWrapperFilter
	{
		int Init([In] ref Guid clsidDMO, [In] ref Guid catDMO);
	}

	#endregion

	#region Windows Media Format 関連:

	/// <summary>
	/// ASF (Advanced Streaming Format) プロファイルの取得や設定を行うメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	/// https://msdn.microsoft.com/ja-jp/library/Cc355803.aspx
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("45086030-F7E4-486a-B504-826BB5792A3B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IConfigAsfWriter
	{
		int ConfigureFilterUsingProfileId([In] int dwProfileId);
		int GetCurrentProfileId([Out] out int pdwProfileId);
		int ConfigureFilterUsingProfileGuid([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidProfile);
		int GetCurrentProfileGuid([Out] out Guid pProfileGuid);
		int ConfigureFilterUsingProfile([In] IWMProfile pProfile);
		int GetCurrentProfile([Out] out IWMProfile ppProfile);
		int SetIndexMode([In, MarshalAs(UnmanagedType.Bool)] bool bIndexFile);
		int GetIndexMode([Out, MarshalAs(UnmanagedType.Bool)] out bool pbIndexFile);
	}

	/// <summary>
	/// ASF (Advanced Streaming Format) プロファイルの取得や設定を行うメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	/// https://msdn.microsoft.com/ja-jp/library/Cc355803.aspx
	/// </remarks>
	/// <seealso cref="T:DSLab._AM_ASFWRITERCONFIG_PARAM"/>
	[ComVisible(true), ComImport(), Guid("7989CCAA-53F0-44f0-884A-F3B03F6AE066"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IConfigAsfWriter2
	{
		// Inherits IConfigAsfWriter
		int ConfigureFilterUsingProfileId([In] int dwProfileId);
		int GetCurrentProfileId([Out] out int pdwProfileId);
		int ConfigureFilterUsingProfileGuid([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidProfile);
		int GetCurrentProfileGuid([Out] out Guid pProfileGuid);
		int ConfigureFilterUsingProfile([In] IWMProfile pProfile);
		int GetCurrentProfile([Out] out IWMProfile ppProfile);
		int SetIndexMode([In, MarshalAs(UnmanagedType.Bool)] bool bIndexFile);
		int GetIndexMode([Out, MarshalAs(UnmanagedType.Bool)] out bool pbIndexFile);

		// IConfigAsfWriter2
		int StreamNumFromPin([In] IPin pPin, [Out] out short pwStreamNum);
		int SetParam([In] uint dwParam, [In] int dwParam1, [In] int dwParam2);
		int GetParam([In] uint dwParam, [Out] out uint pdwParam1, [Out] out uint pdwParam2);
		int ResetMultiPassState();
	}

	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true), ComImport(), Guid("96406BDB-2B2B-11d3-B36B-00C04F6108FF"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWMProfile
	{
		int GetVersion([Out] out WMT_VERSION pdwVersion);
		int GetName([In,Out] IntPtr pwszName, [In,Out] ref uint pcchName);
		int SetName([In,MarshalAs(UnmanagedType.LPWStr)] string pwszName);
		int GetDescription([In, Out] IntPtr pwszDescription, [In, Out] ref uint pcchDescription);
		int SetDescription([In, MarshalAs(UnmanagedType.LPWStr)] string pwszDescription);
		int GetStreamCount([Out] out uint pcStreams);
		int GetStream([In] uint dwStreamIndex, [Out] out IWMStreamConfig ppConfig);
		int GetStreamByNumber([In] ushort wStreamNum, [Out] out IWMStreamConfig ppConfig);
		int RemoveStream([In] IWMStreamConfig pConfig);
		int RemoveStreamByNumber([In] ushort wStreamNum);
		int AddStream([In] IWMStreamConfig pConfig);
		int ReconfigStream([In] IWMStreamConfig pConfig);
		int CreateNewStream([In] ref Guid guidStreamType, [Out] out IWMStreamConfig ppConfig);
		int GetMutualExclusionCount([Out] out uint pcME);
		int GetMutualExclusion([In] uint dwMEIndex, [Out] out IWMMutualExclusion ppME);
		int RemoveMutualExclusion([In] IWMMutualExclusion pME);
		int AddMutualExclusion([In] IWMMutualExclusion pME);
		int CreateNewMutualExclusion([Out] out IWMMutualExclusion ppME);
	}

	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true), ComImport(), Guid("d16679f2-6ca0-472d-8d31-2f5d55aee155"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWMProfileManager
	{
		int CreateEmptyProfile([In] WMT_VERSION dwVersion, [Out] out IWMProfile ppProfile);
		int LoadProfileByID([In] ref Guid guidProfile, [Out] out IWMProfile ppProfile);
		int LoadProfileByData([In, MarshalAs(UnmanagedType.LPWStr)] string pwszProfile, [Out] out IWMProfile ppProfile);
		int SaveProfile([In] IWMProfile pIWMProfile, [In, MarshalAs(UnmanagedType.LPWStr)] string pwszProfile, [In,Out] ref uint pdwLength);
		int GetSystemProfileCount([Out] out uint pcProfiles);
		int LoadSystemProfile([In] uint dwProfileIndex, [Out] out IWMProfile ppProfile);
	}

	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true), ComImport(), Guid("7A924E51-73C1-494d-8019-23D37ED9B89A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWMProfileManager2
	{
		// Inherits IWMProfileManager
		int CreateEmptyProfile([In] WMT_VERSION dwVersion, [Out] out IWMProfile ppProfile);
		int LoadProfileByID([In] ref Guid guidProfile, [Out] out IWMProfile ppProfile);
		int LoadProfileByData([In, MarshalAs(UnmanagedType.LPWStr)] string pwszProfile, [Out] out IWMProfile ppProfile);
		int SaveProfile([In] IWMProfile pIWMProfile, [In, MarshalAs(UnmanagedType.LPWStr)] string pwszProfile, [In,Out] ref uint pdwLength);
		int GetSystemProfileCount([Out] out uint pcProfiles);
		int LoadSystemProfile([In] uint dwProfileIndex, [Out] out IWMProfile ppProfile);

		// IWMProfileManager2
		int GetSystemProfileVersion([Out] out WMT_VERSION pdwVersion);
		int SetSystemProfileVersion([In] WMT_VERSION dwVersion);
	}

	[ComVisible(true), ComImport(), Guid("BA4DCC78-7EE0-4ab8-B27A-DBCE8BC51454"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWMProfileManagerLanguage
	{
		int GetUserLanguageID([Out] out ushort wLangID);
		int SetUserLanguageID([In] ushort wLangID);
	}

	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true), ComImport(), Guid("96406BDC-2B2B-11d3-B36B-00C04F6108FF"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWMStreamConfig
	{
		int GetStreamType([Out] out Guid pguidStreamType);
		int GetStreamNumber([Out] out ushort pwStreamNum);
		int SetStreamNumber([In] ushort wStreamNum);
		int GetStreamName([Out, MarshalAs(UnmanagedType.LPWStr)] out string pwszStreamName, [In, Out] ref ushort pcchStreamName);
		int SetStreamName([In, MarshalAs(UnmanagedType.LPWStr)] string pwszStreamName);
		int GetConnectionName([Out, MarshalAs(UnmanagedType.LPWStr)] out string pwszInputName, [In, Out] ref ushort pcchInputName);
		int SetConnectionName([In, MarshalAs(UnmanagedType.LPWStr)] string pwszInputName);
		int GetBitrate([Out] out uint pdwBitrate);
		int SetBitrate([In] uint pdwBitrate);
		int GetBufferWindow([Out] out uint pmsBufferWindow);
		int SetBufferWindow([In] uint msBufferWindow);
	}

	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true), ComImport(), Guid("96406BDE-2B2B-11d3-B36B-00C04F6108FF"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWMMutualExclusion
	{
		int GetType([Out] out Guid pguidType);
		int SetType([In] ref Guid guidType);
	}

	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true), ComImport(), Guid("96406BCE-2B2B-11d3-B36B-00C04F6108FF"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IWMMediaProps
	{
		int GetType([Out] out Guid pguidType);
		int GetMediaType([In, Out] IntPtr pType, [In, Out] ref int pcbType);	// WM_MEDIA_TYPE
		int SetMediaType([In] IntPtr pType);	// WM_MEDIA_TYPE
	}

	#endregion

	#region GraphEdit ファイル関連:

	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true), ComImport(), Guid("0000000d-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumSTATSTG
	{
		int Next([In] System.UInt32 celt, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] System.Runtime.InteropServices.ComTypes.STATSTG[] rgelt, [In, Out] ref System.UInt32 pceltFetched);
		int Skip([In] System.UInt32 celt);
		int Reset();
		int Clone([In, Out] ref IEnumSTATSTG ppenum);
	}

	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true), ComImport(), Guid("0000000b-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IStorage
	{
		int CreateStream(
			[In, MarshalAs(UnmanagedType.LPWStr)] string wcsName,
			[In, MarshalAs(UnmanagedType.U4)] STGM grfMode,
			[In, MarshalAs(UnmanagedType.U4)] System.UInt32 reserved1,
			[In, MarshalAs(UnmanagedType.U4)] System.UInt32 reserved2,
			[In, Out, MarshalAs(UnmanagedType.Interface)] ref System.Runtime.InteropServices.ComTypes.IStream ppstg);
		int OpenStream(
			[In, MarshalAs(UnmanagedType.LPWStr)] string wcsName,
			[In] IntPtr reserved1,
			[In, MarshalAs(UnmanagedType.U4)] STGM grfMode,
			[In] System.UInt32 reserved2,
			[In, Out, MarshalAs(UnmanagedType.Interface)] ref System.Runtime.InteropServices.ComTypes.IStream ppstg);
		int CreateStorage(
			[MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
			[In, MarshalAs(UnmanagedType.U4)] STGM grfMode,
			[In, MarshalAs(UnmanagedType.U4)] System.UInt32 reserved1,
			[In, MarshalAs(UnmanagedType.U4)] System.UInt32 reserved2,
			[In, Out, MarshalAs(UnmanagedType.Interface)] ref IStorage ppstg);
		int OpenStorage(
			[In, MarshalAs(UnmanagedType.LPWStr)] string wcsName,
			[In] IntPtr stgPriority,
			[In, MarshalAs(UnmanagedType.U4)] STGM grfMode,
			[In] IntPtr snbExclude,
			[In] System.UInt32 reserved,
			[In, Out, MarshalAs(UnmanagedType.Interface)] ref IStorage ppstg);
		int CopyTo(
			[In, MarshalAs(UnmanagedType.U4)] int ciidExclude,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] Guid[] rgiidExclude,
			[In] IntPtr snbExclude,
			[In, MarshalAs(UnmanagedType.Interface)] IStorage pstgDest);
		int MoveElementTo(
			[In, MarshalAs(UnmanagedType.LPWStr)] string wcsName,
			[In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest,
			[In, MarshalAs(UnmanagedType.LPWStr)] string wcsNewName,
			[In, MarshalAs(UnmanagedType.U4)] STGMOVE grfFlags);
		int Commit([In, MarshalAs(UnmanagedType.U4)] STGC grfCommitFlags);
		int Revert();
		int EnumElements(
			[In] System.UInt32 reserved1,
			[In] IntPtr reserved2,
			[In] System.UInt32 reserved3,
			[In, Out, MarshalAs(UnmanagedType.Interface)] ref IEnumSTATSTG ppenum);
		int DestroyElement([In, MarshalAs(UnmanagedType.LPWStr)] string wcsName);
		int RenameElement(
			[In, MarshalAs(UnmanagedType.LPWStr)] string wcsOldName,
			[In, MarshalAs(UnmanagedType.LPWStr)] string wcsNewName);
		int SetElementTimes(
			[In, MarshalAs(UnmanagedType.LPWStr)] string wcsName,
			[In] ref System.Runtime.InteropServices.ComTypes.FILETIME ctime,
			[In] ref System.Runtime.InteropServices.ComTypes.FILETIME atime,
			[In] ref System.Runtime.InteropServices.ComTypes.FILETIME mtime);
		int SetClass([In] ref Guid clsid);
		int SetStateBits(
			[In, MarshalAs(UnmanagedType.U4)] System.UInt32 grfStateBits,
			[In, MarshalAs(UnmanagedType.U4)] System.UInt32 grfMask);
		int Stat(
			[In] ref System.Runtime.InteropServices.ComTypes.STATSTG statstg,
			[In, MarshalAs(UnmanagedType.U4)] STATFLAG grfStatFlag);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	///		https://msdn.microsoft.com/ja-jp/library/microsoft.visualstudio.ole.interop.ipersiststream.aspx
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("00000109-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersistStream
	{
		// Inherits IPersist
		int GetClassID([Out] out Guid pClassID);	// override

		// IPersistStream
		int IsDirty();
		int Load(
			[In, MarshalAs(UnmanagedType.Interface)] System.Runtime.InteropServices.ComTypes.IStream pStm);
		int Save(
			[In, MarshalAs(UnmanagedType.Interface)] System.Runtime.InteropServices.ComTypes.IStream pStm,
			[In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);
		int GetSizeMax([Out] out ulong pcbSize);
	}

	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true), ComImport(), Guid("B196B28B-BAB4-101A-B69C-00AA00341D07"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISpecifyPropertyPages
	{
		int GetPages(ref CAUUID pPages);
	}

	/// <summary>
	/// デバイス用の列挙子を作成するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("29840822-5B84-11D0-BD3B-00A0C911CE86"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICreateDevEnum
	{
		int CreateClassEnumerator([In] ref Guid pType, [In, Out] ref System.Runtime.InteropServices.ComTypes.IEnumMoniker ppEnumMoniker, [In] int dwFlags);
	}

	/// <summary>
	/// 
	/// </summary>
	[ComVisible(true), ComImport(), Guid("55272A00-42CB-11CE-8135-00AA004BB851"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPropertyBag
	{
		int Read([MarshalAs(UnmanagedType.LPWStr)] string PropName, ref object Var, int ErrorLog);
		int Write(string PropName, ref object Var);
	}

	#endregion

	#region SampleGrabber 関連:

	/// <summary>
	/// フィルタ グラフ内を通るメディア サンプルを取得するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("6B652FFF-11FE-4fce-92AD-0266B5D7C78F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISampleGrabber
	{
		int SetOneShot([In, MarshalAs(UnmanagedType.Bool)] bool OneShot);
		int SetMediaType([In, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int GetConnectedMediaType([Out, MarshalAs(UnmanagedType.LPStruct)] AM_MEDIA_TYPE pmt);
		int SetBufferSamples([In, MarshalAs(UnmanagedType.Bool)] bool BufferThem);
		int GetCurrentBuffer(ref int pBufferSize, IntPtr pBuffer);
		int GetCurrentSample(IntPtr ppSample);
		int SetCallback(ISampleGrabberCB pCallback, int WhichMethodToCallback);
	}

	/// <summary>
	/// ISampleGrabber::SetCallback メソッドに対応するコールバック メソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("0579154A-2B53-4994-B0D0-E773148EFF85"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISampleGrabberCB
	{
		[PreserveSig()]
		int SampleCB(double SampleTime, IMediaSample pSample);
		[PreserveSig()]
		int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen);
	}

	#endregion

	#region VMR9 関連:

	/// <summary>
	/// 
	/// </summary>
	[Flags(), ComVisible(false)]
	public enum VMR9AlphaBitmapFlags : int
	{
		VMR9AlphaBitmap_Disable = 0x1,
		VMR9AlphaBitmap_hDC = 0x2,
		VMR9AlphaBitmap_EntireDDS = 0x4,
		VMR9AlphaBitmap_SrcColorKey = 0x8,
		VMR9AlphaBitmap_SrcRect = 0x10,
		VMR9AlphaBitmap_FilterMode = 0x20
	}

	/// <summary>
	/// 合成空間内のビデオ矩形の位置を指定する.
	/// </summary>
	/// <remarks>
	///		seealso: VMR9AlphaBitmap
	/// </remarks>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public class VMR9NormalizedRect
	{
		public float Left;
		public float Top;
		public float Right;
		public float Bottom;
	}

	/// <summary>
	/// ブレンドするビットマップイメージとそのブレンディングパラメータ.
	/// </summary>
	/// <remarks>
	/// VMR-9 の IVMRMixerBitmap9 メソッドで使われる.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public class VMR9AlphaBitmap
	{
		public VMR9AlphaBitmapFlags dwFlags;
		public IntPtr hDC;
		public IntPtr pDDS;
		public RECT rSrc;
		public VMR9NormalizedRect rDest;
		public float fAlpha;
		public int clrSrcKey;
		public uint dwFilterMode;
	}

	/// <summary>
	/// Video Mixing Renderer フィルタ 9 を使用して、
	/// 静止画像とビデオ ストリームの合成に関するメソッドを提供するインタフェース。
	/// </summary>
	[ComVisible(true), ComImport(), Guid("ced175e5-1935-4820-81bd-ff6ad00c9108"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IVMRMixerBitmap9
	{
		int SetAlphaBitmap([In, MarshalAs(UnmanagedType.LPStruct)] VMR9AlphaBitmap pBmpParms);
		int UpdateAlphaBitmapParameters([In, MarshalAs(UnmanagedType.LPStruct)] VMR9AlphaBitmap pBmpParms);
		int GetAlphaBitmapParameters([Out, MarshalAs(UnmanagedType.LPStruct)] VMR9AlphaBitmap pBmpParms);
	}

	/// <summary>
	/// ビデオ ストリームで実行されるイメージ調整を指定する.
	/// </summary>
	/// <remarks>
	/// Video Mixing Renderer フィルタ 9 (VMR-9) で使われる.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public class VMR9ProcAmpControl
	{
		public UInt32 dwSize;
		public UInt32 dwFlags;
		public float Brightness;
		public float Contrast;
		public float Hue;
		public float Saturation;
	}

	/// <summary>
	/// イメージ調整プロパティに対応する有効な範囲を指定する.
	/// </summary>
	/// <remarks>
	/// Video Mixing Renderer 9 フィルタ (VMR-9) で使われる.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public class VMR9ProcAmpControlRange
	{
		public UInt32 dwSize;
		public VMR9ProcAmpControl dwProperty;
		public float MinValue;
		public float MaxValue;
		public float DefaultValue;
		public float StepSize;
	}

	/// <summary>
	/// Video Mixing Renderer フィルタ 9 (VMR-9) で受け取ったビデオ ストリームを操作するメソッドを提供するインタフェース.
	/// </summary>
	[ComVisible(true), ComImport(), Guid("1a777eaa-47c8-4930-b2c9-8fee1c1b0f3b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IVMRMixerControl9
	{
		int SetAlpha([In] UInt32 dwStreamID, [In] float Alpha);
		int GetAlpha([In] UInt32 dwStreamID, [In, Out] ref float Alpha);
		int SetZOrder([In] UInt32 dwStreamID, [In] UInt32 dwZ);
		int GetZOrder([In] UInt32 dwStreamID, [In, Out] ref UInt32 dwZ);
		int SetOutputRect([In] UInt32 dwStreamID, [In, MarshalAs(UnmanagedType.LPStruct)] VMR9NormalizedRect pRect);
		int GetOutputRect([In] UInt32 dwStreamID, [Out, MarshalAs(UnmanagedType.LPStruct)] VMR9NormalizedRect pRect);
		int SetBackgroundClr([In] UInt32 dwMixerPrefs);
		int GetBackgroundClr([In, Out] ref UInt32 dwMixerPrefs);
		int SetProcAmpControl([In] UInt32 dwStreamID, [In, MarshalAs(UnmanagedType.LPStruct)] VMR9ProcAmpControl lpClrControl);
		int GetProcAmpControl([In] UInt32 dwStreamID, [In, Out, MarshalAs(UnmanagedType.LPStruct)] VMR9ProcAmpControl lpClrControl);
		int GetProcAmpControlRange([In] UInt32 dwStreamID, [In, Out, MarshalAs(UnmanagedType.LPStruct)] VMR9ProcAmpControlRange lpClrControl);
	}


	#endregion

	#region ストリームバッファエンジン関連:

	/// <summary>
	/// グラフプロファイルをロックするメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	/// このインターフェイスは、ストリーム バッファ シンク フィルタが公開する.
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("afd1f242-7efd-45ee-ba4e-407a25c9a77a"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IStreamBufferSink
	{
		int LockProfile([In, MarshalAs(UnmanagedType.LPWStr)] string pszStreamBufferFilename);
		int CreateRecorder([In, MarshalAs(UnmanagedType.LPWStr)] string pszStreamBufferFilename, [In] int dwRecordType, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object pRecordingIUnknown);
		int IsProfileLocked();
	}

	/// <summary>
	/// コンテンツを再生するメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	/// このインターフェイスは、ストリーム バッファ ソース フィルタが公開する.
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("1c5bd776-6ced-4f44-8164-5eab0e98db12"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IStreamBufferSource
	{
		int SetStreamSink([In] IStreamBufferSink pIStreamBufferSink);
	}

	/// <summary>
	/// ストリーム バッファ ソース グラフ内のシークを制御するメソッドを提供するインタフェース.
	/// </summary>
	/// <remarks>
	/// このインタフェースは、ストリーム バッファ ソース フィルタが公開する.
	/// </remarks>
	[ComVisible(true), ComImport(), Guid("f61f5c26-863d-4afa-b0ba-2f81dc978596"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IStreamBufferMediaSeeking
	{
		int GetCapabilities(ref AM_SEEKING_SeekingCapabilities pCapabilities);
		int CheckCapabilities([In, Out] ref AM_SEEKING_SeekingCapabilities pCapabilities);
		int IsFormatSupported([In] ref Guid pFormat);
		int QueryPreferredFormat([In, Out] ref Guid pFormat);
		int GetTimeFormat([In, Out] ref Guid pFormat);
		int IsUsingTimeFormat([In] ref Guid pFormat);
		int SetTimeFormat([In] ref Guid pFormat);
		int GetDuration(ref long pDuration);
		int GetStopPosition(ref long pStop);
		int GetCurrentPosition(ref long pCurrent);
		int ConvertTimeFormat(ref long pTarget, [In] ref Guid pTargetFormat, long Source, [In] ref Guid pSourceFormat);
		int SetPositions([In] ref long pCurrent, AM_SEEKING_SEEKING_FLAGS dwCurrentFlags, [In] long pStop, AM_SEEKING_SEEKING_FLAGS dwStopFlags);
		int GetPositions(ref long pCurrent, ref long pStop);
		int GetAvailable(ref long pEarliest, ref long pLatest);
		int SetRate(double dRate);
		int GetRate(ref double pdRate);
		int GetPreroll(ref long pllPreroll);
	}

	#endregion
}
