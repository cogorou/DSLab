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

namespace DSLab
{
	// 
	// https://msdn.microsoft.com/ja-jp/library/cc353907.aspx
	// 

	#region AM_SEEKING_SeekingCapabilities

	/// <summary>
	/// メディアストリームのシーク能力を示す定数
	/// </summary>
	[Flags(), ComVisible(false)]
	public enum AM_SEEKING_SeekingCapabilities
	{
		/// <summary>
		/// ストリームは絶対位置にシークできる。
		/// </summary>
		CanSeekAbsolute = 0x1,
		/// <summary>
		/// ストリームは順方向にシークできる。
		/// </summary>
		CanSeekForwards = 0x2,
		/// <summary>
		/// ストリームは逆方向にシークできる。
		/// </summary>
		CanSeekBackwards = 0x4,
		/// <summary>
		/// ストリームは現在位置を報告できる。
		/// </summary>
		CanGetCurrentPos = 0x8,
		/// <summary>
		/// ストリームは停止位置を報告できる。
		/// </summary>
		CanGetStopPos = 0x10,
		/// <summary>
		/// ストリームは時間幅を報告できる。
		/// </summary>
		CanGetDuration = 0x20,
		/// <summary>
		/// ストリームは逆方向に再生できる。
		/// </summary>
		CanPlayBackwards = 0x40,
		/// <summary>
		/// ストリームはシームレスループを行える。
		/// </summary>
		CanDoSegments = 0x80,
		/// <summary>
		/// 予約済み。
		/// </summary>
		Source = 0x100
	}

	#endregion

	#region AM_SEEKING_SEEKING_FLAGS

	/// <summary>
	/// メディアストリームのシーク能力を示す定数
	/// </summary>
	[Flags(), ComVisible(false)]
	public enum AM_SEEKING_SEEKING_FLAGS
	{
		/// <summary>
		/// 位置変更はない。タイム引数は、NULLとすることもできる。
		/// </summary>
		NoPositioning = 0x0,
		/// <summary>
		/// 指定した位置は絶対値です。
		/// </summary>
		AbsolutePositioning = 0x1,
		/// <summary>
		/// 指定した位置は以前の値からの相対値です。
		/// </summary>
		RelativePositioning = 0x2,
		/// <summary>
		/// 停止位置は現在位置からの相対値です。
		/// </summary>
		IncrementalPositioning = 0x3,
		/// <summary>
		/// 
		/// </summary>
		PositioningBitsMask = 0x3,
		/// <summary>
		/// 最も近いキーフレームをシークする。これは高速だが、精度は落ちる。
		/// </summary>
		SeekToKeyFrame = 0x4,
		/// <summary>
		/// 等しい基準タイムを返す。
		/// </summary>
		ReturnTime = 0x8,
		/// <summary>
		/// セグメントシークを使う。
		/// </summary>
		Segment = 0x10,
		/// <summary>
		/// フラッシュしない。
		/// </summary>
		NoFlush = 0x20
	}

	#endregion

	#region AMSTREAMSELECTINFOFLAGS

	/// <summary>
	/// IAMStreamSelectインターフェースのInfoメソッドで使用する定数
	/// </summary>
	[ComVisible(false)]
	public enum AMSTREAMSELECTINFOFLAGS
	{
		/// <summary>
		/// このストリームは無効です。
		/// </summary> 
		DISABLE = 0,
		/// <summary>
		/// ストリームは有効で、このグループ内の他のストリームも有効です。
		/// </summary> 
		ENABLED = 1,
		/// <summary>
		/// グループ内でこのストリームのみが有効です。
		/// </summary> 
		EXCLUSIVE = 2
	}

	#endregion

	#region AMSTREAMSELECTENABLEFLAGS

	/// <summary>
	/// IAMStreamSelectインターフェースのEnableメソッドで使用する定数
	/// </summary>
	[ComVisible(false)]
	public enum AMSTREAMSELECTENABLEFLAGS
	{
		/// <summary>
		/// このストリームを含むグループ内のすべてのストリームを無効にする。
		/// </summary> 
		DISABLE = 0,
		/// <summary>
		/// 指定のグループ内のこのストリームだけを有効にし、他のすべてのストリームを無効にする。
		/// </summary> 
		ENABLE = 1,
		/// <summary>
		/// このストリームを含むグループ内のすべてのストリームを有効にする。
		/// </summary> 
		ENABLEALL = 2
	}

	#endregion

	#region PIN_DIRECTION

	/// <summary>
	/// ピンの方向を示す定数
	/// </summary>
	[ComVisible(false)]
	public enum PIN_DIRECTION
	{
		/// <summary>
		/// 入力ピン
		/// </summary> 
		PINDIR_INPUT = 0,

		/// <summary>
		/// 出力ピン
		/// </summary> 
		PINDIR_OUTPUT = 1,
	}

	#endregion

	#region FILTER_STATE

	/// <summary>
	/// フィルタグラフの状態を示す定数
	/// </summary>
	/// <remarks>
	/// http://msdn.microsoft.com/ja-jp/library/cc354514.aspx
	/// </remarks>
	[ComVisible(false)]
	public enum FILTER_STATE : int
	{
		/// <summary>
		/// 停止中。フィルタはデータ処理中でない。
		/// </summary> 
		Stopped = 0,
		/// <summary>
		/// ポーズ。フィルタはデータのレンダリング中ではないが処理中です。
		/// </summary> 
		Paused = 1,
		/// <summary>
		/// 実行中。フィルタはデータの処理およびレンダリング中です。
		/// </summary> 
		Running = 2,
	}

	#endregion

	#region CameraControlProperty

	/// <summary>
	/// カメラ制御プロパティを示す定数。(see:IAMCameraControl)
	/// </summary>
	/// <remarks>
	/// see: http://msdn.microsoft.com/ja-jp/library/cc352340.aspx
	/// </remarks>
	[ComVisible(false)]
	public enum CameraControlProperty
	{
		/// <summary>
		/// パンの設定 (度単位)。範囲:-180~+180、既定値:0。
		/// </summary>
		Pan = 0,
		/// <summary>
		/// 傾きの設定 (度単位)。範囲:-180~+180、既定値:0。
		/// </summary>
		Tilt = 1,
		/// <summary>
		/// ロールの設定 (度単位)。範囲:-180~+180、既定値:0。
		/// </summary>
		Roll = 2,
		/// <summary>
		/// ズームの設定 (ミリ単位)。範囲:10~600、既定値はデバイスによって異なります。
		/// </summary>
		Zoom = 3,
		/// <summary>
		/// 露出の設定 (ログベース 2 秒)。(例: 0=1sec, 1=2sec, 2=4sec, -1=1/2sec, -2=1/4sec, -3=1/8sec)
		/// </summary>
		Exposure = 4,
		/// <summary>
		/// 絞り設定 (fstop * 10 単位)。
		/// </summary>
		Iris = 5,
		/// <summary>
		/// 焦点の設定 (ミリ単位)。範囲と既定値はデバイスによって異なります。
		/// </summary>
		Focus = 6,
	}

	#endregion

	#region CameraControlFlags

	/// <summary>
	/// 特定のカメラ設定プロパティを自動で制御するか手動で制御するかを示す定数 (see:IAMCameraControl)
	/// </summary>
	[Flags(), ComVisible(false)]
	public enum CameraControlFlags
	{
		/// <summary>
		/// 自動制御
		/// </summary>
		Auto = 0x0001,

		/// <summary>
		/// 手動制御
		/// </summary>
		Manual = 0x0002,
	}

	#endregion

	#region VideoProcAmpProperty

	/// <summary>
	/// ビデオ信号品質プロパティを示す定数 (see:IAMVideoProcAmp)
	/// </summary>
	/// <remarks>
	/// see: http://msdn.microsoft.com/ja-jp/library/cc371319.aspx
	/// </remarks>
	[ComVisible(false)]
	public enum VideoProcAmpProperty
	{
		/// <summary>
		/// 明るさ (NTSCの場合はIRE単位 * 100、それ以外は任意の単位)。範囲: -10,000~10,000。0=空白、10,000=純白。
		/// </summary>
		Brightness = 0,
		/// <summary>
		/// コントラスト (ゲイン係数 * 100)。範囲: 0~10,000。
		/// </summary>
		Contrast = 1,
		/// <summary>
		/// 色相 (度 * 100)。範囲: -180,000~180,000 (-180 ～ +180 度)。
		/// </summary>
		Hue = 2,
		/// <summary>
		/// 彩度。範囲: 0~10,000。
		/// </summary>
		Saturation = 3,
		/// <summary>
		/// 鮮明度。範囲: 0~100。
		/// </summary>
		Sharpness = 4,
		/// <summary>
		/// ガンマ (ガンマ * 100)。範囲: 1~500。
		/// </summary>
		Gamma = 5,
		/// <summary>
		/// 色の有効化設定。範囲: 0 (off) または 1 (on)。
		/// </summary>
		ColorEnable = 6,
		/// <summary>
		/// ホワイトバランス (絶対温度の色温度)。範囲はデバイスによって異なります。
		/// </summary>
		WhiteBalance = 7,
		/// <summary>
		/// バックライト補正設定。範囲: 0 (off) または 1 (on)。
		/// </summary>
		BacklightCompensation = 8,
		/// <summary>
		/// ゲイン調整。範囲： 0=標準。正の値=明、負の値=暗。(範囲の上下限はデバイスによって異なります。)
		/// </summary>
		Gain = 9
	}

	#endregion

	#region VideoProcAmpFlags

	/// <summary>
	/// 特定のビデオ信号品質プロパティが自動で制御されるか手動で制御されるかを示す定数 (see:IAMCameraControl)
	/// </summary>
	[Flags(), ComVisible(false)]
	public enum VideoProcAmpFlags
	{
		/// <summary>
		/// 自動制御
		/// </summary> 
		Auto = 0x0001,

		/// <summary>
		/// 手動制御
		/// </summary> 
		Manual = 0x0002,
	}

	#endregion

	#region VideoControlFlags

	/// <summary>
	/// ビデオ デバイスの動作ビデオ モードを指定する。
	/// </summary>
	/// <remarks>
	///	see: http://msdn.microsoft.com/ja-jp/library/cc371293.aspx
	/// </remarks>
	[Flags(), ComVisible(false)]
	public enum VideoControlFlags
	{
		/// <summary>
		/// 0x0001: ピクチャを水平方向にフリップすることを指定する。
		/// </summary>
		VideoControlFlag_FlipHorizontal = 0x0001,

		/// <summary>
		/// 0x0002: ピクチャを垂直方向にフリップすることを指定する。
		/// </summary>
		VideoControlFlag_FlipVertical = 0x0002,

		/// <summary>
		/// 0x0004: 外部ソースからトリガをキャプチャするようにストリームをセットアップする。(注意事項あり)
		/// </summary>
		VideoControlFlag_ExternalTriggerEnable = 0x0004,

		/// <summary>
		/// 0x0008: ストリームに VideoControlFlag_ExternalTriggerEnable フラグが設定されているときに、ソフトウェアで外部トリガをシミュレートする。
		/// </summary>
		VideoControlFlag_Trigger = 0x0008
	}

	#endregion

	#region EventCode

	/// <summary>
	/// フィルタグラフマネージャによって処理されるイベントの通知コード
	/// </summary>
	[ComVisible(false)]
	public enum EventCode
	{
		EC_COMPLETE = 0x1,
		EC_USERABORT = 0x2,
		EC_ERRORABORT = 0x3,
		EC_TIME = 0x4,
		EC_REPAINT = 0x5,
		EC_STREAM_ERROR_STOPPED = 0x6,
		EC_STREAM_ERROR_STILLPLAYING = 0x7,
		EC_ERROR_STILLPLAYING = 0x8,
		EC_PALETTE_CHANGED = 0x9,
		EC_VIDEO_SIZE_CHANGED = 0xa,
		EC_QUALITY_CHANGE = 0xb,
		EC_SHUTTING_DOWN = 0xc,
		EC_CLOCK_CHANGED = 0xd,
		EC_PAUSED = 0xe,
		EC_OPENING_FILE = 0x10,
		EC_BUFFERING_DATA = 0x11,
		EC_FULLSCREEN_LOST = 0x12,
		EC_ACTIVATE = 0x13,
		EC_NEED_RESTART = 0x14,
		EC_WINDOW_DESTROYED = 0x15,
		EC_DISPLAY_CHANGED = 0x16,
		EC_STARVATION = 0x17,
		EC_OLE_EVENT = 0x18,
		EC_NOTIFY_WINDOW = 0x19,
		EC_DVD_DOMAIN_CHANGE = 0x101,
		EC_DVD_TITLE_CHANGE = 0x102,
		EC_DVD_CHAPTER_START = 0x103,
		EC_DVD_AUDIO_STREAM_CHANGE = 0x104,
		EC_DVD_SUBPICTURE_STREAM_CHANGE = 0x105,
		EC_DVD_ANGLE_CHANGE = 0x106,
		EC_DVD_BUTTON_CHANGE = 0x107,
		EC_DVD_VALID_UOPS_CHANGE = 0x108,
		EC_DVD_STILL_ON = 0x109,
		EC_DVD_STILL_OFF = 0x10a,
		EC_DVD_CURRENT_TIME = 0x10b,
		EC_DVD_ERROR = 0x10c,
		EC_DVD_WARNING = 0x10d,
		EC_DVD_CHAPTER_AUTOSTOP = 0x10e,
		EC_DVD_NO_FP_PGC = 0x10f,
		EC_DVD_PLAYBACK_RATE_CHANGE = 0x110,
		EC_DVD_PARENTAL_LEVEL_CHANGE = 0x111,
		EC_DVD_PLAYBACK_STOPPED = 0x112,
		EC_DVD_ANGLES_AVAILABLE = 0x113,
		EC_DVD_PLAYPERIOD_AUTOSTOP = 0x114,
		EC_DVD_BUTTON_AUTO_ACTIVATED = 0x115,
		EC_DVD_CMD_START = 0x116,
		EC_DVD_CMD_END = 0x117,
		EC_DVD_DISC_EJECTED = 0x118,
		EC_DVD_DISC_INSERTED = 0x119,
		EC_DVD_CURRENT_HMSF_TIME = 0x11a,
		EC_DVD_KARAOKE_MODE = 0x11b
	}

	#endregion

	#region WMT_VERSION

	/// <summary>
	/// Windows Media Format SDK のバージョン定数
	/// </summary>
	public enum WMT_VERSION
	{
		WMT_VER_4_0  = 0x00040000,
		WMT_VER_7_0  = 0x00070000,
		WMT_VER_8_0  = 0x00080000,
		WMT_VER_9_0  = 0x00090000,
	}

	#endregion

	#region _AM_ASFWRITERCONFIG_PARAM

	/// <summary>
	/// WM ASF Writer の設定値の定数
	/// </summary>
	/// <remarks>
	/// https://msdn.microsoft.com/en-us/library/windows/desktop/dd391030(v=vs.85).aspx <br/>
	/// <br/>
	/// IConfigAsfWriter2 の SetParam/GetParam の第一引数 (<c>dwParam</c>) に指定する値です。
	/// </remarks>
	/// <seealso cref="T:DSLab.IConfigAsfWriter2"/>
	public enum _AM_ASFWRITERCONFIG_PARAM
	{
		AM_CONFIGASFWRITER_PARAM_AUTOINDEX,
		AM_CONFIGASFWRITER_PARAM_MULTIPASS,
		AM_CONFIGASFWRITER_PARAM_DONTCOMPRESS,
	}

	#endregion
}
