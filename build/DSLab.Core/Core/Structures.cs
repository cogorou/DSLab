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
using System.ComponentModel;

namespace DSLab
{
	// 
	// https://msdn.microsoft.com/ja-jp/library/cc353919.aspx
	// 

	#region AM_MEDIA_TYPE

	/// <summary>
	/// AM_MEDIA_TYPE
	/// </summary>
	/// <remarks>
	/// メディア サンプルのフォーマットを記述します。
	/// </remarks>
	[Serializable]
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public class AM_MEDIA_TYPE
	{
		/// <summary>
		/// メディア サンプルの メジャー タイプを指定するグローバル一意識別子 (GUID)。
		/// </summary>
		public Guid majortype;

		/// <summary>
		/// メディア サンプルのサブタイプを指定する GUID。
		/// </summary>
		public Guid subtype;

		/// <summary>
		/// TRUE の場合、サンプルのサイズは固定である。このフィールドは参照専用である。オーディオの場合は、通常、TRUE に設定される。ビデオの場合、通常、非圧縮ビデオは TRUE、圧縮ビデオは FALSE に設定される。
		/// </summary>
		[MarshalAs(UnmanagedType.Bool)]
		public bool bFixedSizeSamples;

		/// <summary>
		/// TRUE の場合、サンプルは時系列 (フレーム間) 圧縮方式で圧縮されている。値 TRUE はフレームがすべてキー フレームとは限らないことを示す。このフィールドは参照専用である。
		/// </summary>
		[MarshalAs(UnmanagedType.Bool)]
		public bool bTemporalCompression;

		/// <summary>
		/// サンプルのサイズ (バイト単位)。圧縮データの場合は、0 も可能。
		/// </summary>
		public uint lSampleSize;

		/// <summary>
		/// フォーマット ブロックに使う構造体を指定する GUID。pbFormat メンバは、対応するフォーマット構造体を指す。
		/// </summary>
		public Guid formattype;

		/// <summary>
		/// 未使用。(IUnknown*)
		/// </summary>
		public IntPtr pUnk;

		/// <summary>
		/// フォーマット ブロックのサイズ (バイト単位)。
		/// </summary>
		public uint cbFormat;

		/// <summary>
		/// フォーマット ブロックへのポインタ。(BYTE*) 構造体の種類は、formattype メンバによって指定される。formattype が GUID_NULL または FORMAT_None でない限り、フォーマット構造体が存在しなければならない。
		/// </summary>
		public IntPtr pbFormat;
	}

	#endregion

	#region FILTER_INFO

	/// <summary>
	/// FILTER_INFO
	/// </summary>
	/// <remarks>
	/// フィルタに関する情報を格納します。
	/// </remarks>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode), ComVisible(false)]
	public class FILTER_INFO
	{
		/// <summary>
		/// achName の最大文字列長 (NULL を含む)
		/// </summary>
		public const int MAX_FILTER_NAME = 128;

		/// <summary>
		/// フィルタ名が格納された、NULL で終了する文字列。
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_FILTER_NAME)]
		public string achName;

		/// <summary>
		/// フィルタがフィルタ グラフに属している場合は、フィルタ グラフの IFilterGraph インターフェイスへのポインタが設定される。フィルタがフィルタ グラフに属していない場合は NULLを設定する。
		/// </summary>
		[MarshalAs(UnmanagedType.IUnknown)]
		public object pGraph;
	}

	#endregion

	#region PIN_INFO

	/// <summary>
	/// PIN_INFO
	/// </summary>
	/// <remarks>
	/// ピンに関する情報を格納します。
	/// </remarks>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode), ComVisible(false)]
	public class PIN_INFO
	{
		/// <summary>
		/// achName の最大文字列長 (NULL を含む)
		/// </summary>
		public const int MAX_PIN_NAME = 128;

		/// <summary>
		/// 所有者フィルタの IBaseFilter インターフェイスへのポインタ。 
		/// </summary>
		public IBaseFilter pFilter;

		/// <summary>
		/// ピンの方向 (入力/出力)。 
		/// </summary>
		public PIN_DIRECTION dir;

		/// <summary>
		/// ピンの名前。 
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PIN_NAME)]
		public string achName;
	}

	#endregion

	#region VIDEOINFOHEADER

	/// <summary>
	/// VIDEOINFOHEADER
	/// </summary>
	/// <remarks>
	/// ビデオ イメージのビットマップと色情報を記述します。 
	/// </remarks>
	[Serializable]
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct VIDEOINFOHEADER
	{
		/// <summary>
		/// 転送元ビデオ ウィンドウを指定する RECT 構造体。ソース ビデオ ストリームの一部を選択するため、この構造体をクリッピング矩形としてもよい。 
		/// </summary>
		public RECT rcSource;

		/// <summary>
		/// 転送先ビデオ ウィンドウを指定する RECT 構造体。 
		/// </summary>
		public RECT rcTarget;

		/// <summary>
		/// ビデオ ストリームのおおよそのデータ レート (ビット/秒)。 
		/// </summary>
		public int dwBitRate;

		/// <summary>
		/// データ エラー レート (ビット エラー/秒) 。 
		/// </summary>
		public int dwBitErrorRate;

		/// <summary>
		/// 必要なビデオ フレームの平均表示時間 (100 ナノ秒単位) 。実際のフレームあたりの表示時間は長くなる可能性がある。
		/// </summary>
		public long AvgTimePerFrame;

		/// <summary>
		/// ビデオ イメージのビットマップの色情報およびディメンジョン情報が格納された BITMAPINFOHEADER 構造体。
		/// </summary>
		public BITMAPINFOHEADER bmiHeader;
	}

	#endregion

	#region WAVEFORMATEX

	/// <summary>
	/// WAVEFORMATEX
	/// </summary>
	/// <remarks>
	/// 波形オーディオ データのフォーマットを記述します。
	/// </remarks>
	[Serializable]
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct WAVEFORMATEX
	{
		/// <summary>
		/// 波形オーディオのフォーマット タイプ。
		/// </summary>
		public ushort wFormatTag;

		/// <summary>
		/// 波形オーディオ データに含まれるチャンネル数。モノラル データは 1 チャンネルを使い、ステレオ データは 2 チャンネルを使う。
		/// </summary>
		public ushort nChannels;

		/// <summary>
		/// サンプル/秒で表すサンプル レート (単位 Hz)。
		/// </summary>
		public uint nSamplesPerSec;

		/// <summary>
		/// フォーマット タグに必要な平均データ転送レート (単位 バイト/秒)。
		/// </summary>
		public uint nAvgBytesPerSec;

		/// <summary>
		/// ブロック アラインメント (単位 バイト)。
		/// </summary>
		public short nBlockAlign;

		/// <summary>
		/// wFormatTag フォーマット タイプの 1 サンプルあたりのビット数。
		/// </summary>
		public short wBitsPerSample;

		/// <summary>
		/// 追加フォーマット情報のサイズ (単位バイト)。
		/// </summary>
		public short cbSize;
	}

	#endregion

	#region VIDEO_STREAM_CONFIG_CAPS

	/// <summary>
	/// VIDEO_STREAM_CONFIG_CAPS 構造体
	/// </summary>
	/// <remarks>
	/// 一定の範囲内のビデオ フォーマットを記述する。
	/// ビデオ圧縮フィルタやビデオ キャプチャ フィルタはこの構造体を使って、生成できるフォーマットを記述する。
	/// http://msdn.microsoft.com/ja-jp/library/cc371332.aspx
	/// </remarks>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 8), ComVisible(false)]
	public struct VIDEO_STREAM_CONFIG_CAPS
	{
		public Guid guid;
		public uint VideoStandard;
		public SIZE InputSize;
		public SIZE MinCroppingSize;
		SIZE MaxCroppingSize;
		public int CropGranularityX;
		public int CropGranularityY;
		public int CropAlignX;
		public int CropAlignY;
		public SIZE MinOutputSize;
		public SIZE MaxOutputSize;
		public int OutputGranularityX;
		public int OutputGranularityY;
		public int StretchTapsX;
		public int StretchTapsY;
		public int ShrinkTapsX;
		public int ShrinkTapsY;
		public long MinFrameInterval;
		public long MaxFrameInterval;
		public int MinBitsPerSecond;
		public int MaxBitsPerSecond;
	}

	#endregion

	#region SIZE

	/// <summary>
	/// サイズ構造体
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 8), ComVisible(false)]
	public struct SIZE
	{
		public int cx;
		public int cy;
	}

	#endregion

	#region WM_MEDIA_TYPE

	/// <summary>
	/// Windows Media Format SDK 関連:
	/// </summary>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct WM_MEDIA_TYPE
	{
		public Guid majortype;
		public Guid subtype;
		public int bFixedSizeSamples;
		public int bTemporalCompression;
		public uint lSampleSize;
		public Guid formattype;
		public IntPtr pUnk;		// IUnknown*
		public uint cbFormat;
		public IntPtr pbFormat;	// BYTE*
	}

	#endregion
}
