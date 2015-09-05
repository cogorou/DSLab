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
	#region 定数

	/// <summary>
	/// 
	/// </summary>
	[Flags()]
	public enum STREAM_SEEK
	{
		STREAM_SEEK_SET = 0,
		STREAM_SEEK_CUR = 1,
		STREAM_SEEK_END = 2
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags()]
	public enum STGTY
	{
		STGTY_STORAGE = 1,
		STGTY_STREAM = 2,
		STGTY_LOCKBYTES = 3,
		STGTY_PROPERTY = 4
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags()]
	public enum STGMOVE
	{
		STGMOVE_MOVE = 0,
		STGMOVE_COPY = 1
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags()]
	public enum STGM
	{
		STGM_READ = 0x0,
		STGM_WRITE = 0x1,
		STGM_READWRITE = 0x2,
		STGM_SHARE_DENY_NONE = 0x40,
		STGM_SHARE_DENY_READ = 0x30,
		STGM_SHARE_DENY_WRITE = 0x20,
		STGM_SHARE_EXCLUSIVE = 0x10,
		STGM_PRIORITY = 0x40000,
		STGM_CREATE = 0x1000,
		STGM_CONVERT = 0x20000,
		STGM_FAILIFTHERE = 0x0,
		STGM_DIRECT = 0x0,
		STGM_TRANSACTED = 0x10000,
		STGM_NOSCRATCH = 0x100000,
		STGM_NOSNAPSHOT = 0x200000,
		STGM_SIMPLE = 0x8000000,
		STGM_DIRECT_SWMR = 0x400000,
		STGM_DELETEONRELEASE = 0x4000000
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags()]
	public enum STGFMT
	{
		STGFMT_STORAGE = 0,
		STGFMT_FILE = 3,
		STGFMT_ANY = 4,
		STGFMT_DOCFILE = 5
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags()]
	public enum STGC
	{
		STGC_DEFAULT = 0,
		STGC_OVERWRITE = 1,
		STGC_ONLYIFCURRENT = 2,
		STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 4,
		STGC_CONSOLIDATE = 8
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags()]
	public enum STATFLAG
	{
		STATFLAG_DEFAULT = 0,
		STATFLAG_NONAME = 1
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags()]
	public enum LOCKTYPE
	{
		LOCK_WRITE = 1,
		LOCK_EXCLUSIVE = 2,
		LOCK_ONLYONCE = 4
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags()]
	public enum PROPSETFLAG
	{
		PROPSETFLAG_DEFAULT = 0,
		PROPSETFLAG_NONSIMPLE = 1,
		PROPSETFLAG_ANSI = 2,
		PROPSETFLAG_UNBUFFERED = 4,
		PROPSETFLAG_CASE_SENSITIVE = 8
	}

	#endregion

	#region 構造体

	/// <summary>
	/// RECT
	/// </summary>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct RECT
	{
		/// <summary>
		/// 
		/// </summary>
		public int Left;

		/// <summary>
		/// 
		/// </summary>
		public int Top;

		/// <summary>
		/// 
		/// </summary>
		public int Right;

		/// <summary>
		/// 
		/// </summary>
		public int Bottom;
	}

	/// <summary>
	/// CAUUID
	/// </summary>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct CAUUID
	{
		/// <summary>
		/// pElems 配列によってポイントされる配列のサイズ.
		/// </summary>
		public int cElems;

		/// <summary>
		/// UUID 値の配列へのポインタ.
		/// </summary>
		public IntPtr pElems;
	}

	/// <summary>
	/// RGBQUAD
	/// </summary>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct RGBQUAD
	{
		/// <summary>
		/// カラーコンポーネント(青)
		/// </summary>
		public byte rgbBlue;

		/// <summary>
		/// カラーコンポーネント(緑)
		/// </summary>
		public byte rgbGreen;

		/// <summary>
		/// カラーコンポーネント(赤)
		/// </summary>
		public byte rgbRed;

		/// <summary>
		/// 予備
		/// </summary>
		public byte rgbReserved;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="R">赤</param>
		/// <param name="G">緑</param>
		/// <param name="B">青</param>
		public RGBQUAD(byte R, byte G, byte B)
		{
			rgbRed = R;
			rgbGreen = G;
			rgbBlue = B;
			rgbReserved = 0;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="R">赤</param>
		/// <param name="G">緑</param>
		/// <param name="B">青</param>
		/// <param name="A">アルファ</param>
		public RGBQUAD(byte R, byte G, byte B, byte A)
		{
			rgbRed = R;
			rgbGreen = G;
			rgbBlue = B;
			rgbReserved = A;
		}
	}

	/// <summary>
	/// RGBTRIPLE
	/// </summary>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct RGBTRIPLE
	{
		/// <summary>
		/// カラーコンポーネント(青)
		/// </summary>
		public byte rgbtBlue;

		/// <summary>
		/// カラーコンポーネント(緑)
		/// </summary>
		public byte rgbtGreen;

		/// <summary>
		/// カラーコンポーネント(赤)
		/// </summary>
		public byte rgbtRed;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="R">赤</param>
		/// <param name="G">緑</param>
		/// <param name="B">青</param>
		public RGBTRIPLE(byte R, byte G, byte B)
		{
			rgbtRed = R;
			rgbtGreen = G;
			rgbtBlue = B;
		}
	}

	/// <summary>
	/// BITMAPINFO
	/// </summary>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct BITMAPINFO
	{
		/// <summary>
		/// BITMAPINFOHEADER
		/// </summary>
		public BITMAPINFOHEADER bmiHeader;

		/// <summary>
		/// カラーパレット
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
		public RGBQUAD[] bmiColors;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="palette_size">パレットの要素数 (1~)</param>
		public BITMAPINFO(int palette_size)
		{
			bmiHeader = new BITMAPINFOHEADER();
			bmiColors = new RGBQUAD[palette_size];
		}
	}

	/// <summary>
	/// BITMAPINFOHEADER
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 2), ComVisible(false)]
	public struct BITMAPINFOHEADER
	{
		/// <summary>
		/// 構造体が必要とするバイト数.
		/// </summary>
		public int biSize;

		/// <summary>
		/// ビットマップの幅.
		/// </summary>
		public int biWidth;

		/// <summary>
		/// ビットマップの高さ(ピクセル単位).
		/// </summary>
		public int biHeight;

		/// <summary>
		/// ターゲット デバイスに対する面の数.これは必ず 1 に設定する。
		/// </summary>
		public short biPlanes;

		/// <summary>
		/// 1 ピクセルあたりのビット数.
		/// </summary>
		public short biBitCount;

		/// <summary>
		/// ビットマップが圧縮されている場合、FOURCC を指定する.
		/// 非圧縮フォーマットの場合、非圧縮 RGB ではBI_RGB、カラー マスクが指定された非圧縮 RGB ではBI_BITFIELDS を指定する.
		/// </summary>
		public int biCompression;

		/// <summary>
		/// イメージのサイズ(バイト単位).
		/// 非圧縮 RGB ビットマップの場合は、0 に設定できる。 
		/// </summary>
		public int biSizeImage;

		/// <summary>
		/// ビットマップのターゲット デバイスの水平解像度.
		/// </summary>
		public int biXPelsPerMeter;

		/// <summary>
		/// ビットマップのターゲット デバイスの垂直解像度.
		/// </summary>
		public int biYPelsPerMeter;

		/// <summary>
		/// カラー テーブル内のカラー インデックスのうち、ビットマップ内で実際に使うインデックスの数.
		/// </summary>
		public int biClrUsed;

		/// <summary>
		/// ビットマップを表示するために重要と見なされるカラー インデックス数.
		/// </summary>
		public int biClrImportant;
	}

	#endregion

	#region 関数

	/// <summary>
	/// Win32API API
	/// </summary>
	public class Win32API
	{
		#region DllImport (ole32)

		[DllImport("ole32.dll")]
		public static extern int StgCreateDocfile([MarshalAs(UnmanagedType.LPWStr)] string wcsName, [In, MarshalAs(UnmanagedType.U4)] STGM grfMode, System.UInt32 reserved, [MarshalAs(UnmanagedType.Interface)] ref IStorage stgOpen);

		[DllImport("ole32.dll"), PreserveSig()]
		public static extern int StgOpenStorage([MarshalAs(UnmanagedType.LPWStr)] string pwcsName, IStorage pstgPriority, STGM grfMode, IntPtr snbExclude, int reserved, [MarshalAs(UnmanagedType.Interface)] ref IStorage storage);

		[DllImport("ole32.dll"), PreserveSig()]
		public static extern int StgIsStorageFile([MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

		#endregion

		#region DllImport (oleaut32)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hwndOwner"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="lpszCaption"></param>
		/// <param name="cObjects"></param>
		/// <param name="ppUnk"></param>
		/// <param name="cPages"></param>
		/// <param name="pPageClsID"></param>
		/// <param name="lcid"></param>
		/// <param name="dwReserved"></param>
		/// <param name="pvReserved"></param>
		/// <returns>
		/// </returns>
		//[DllImport("olepro32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int OleCreatePropertyFrame(IntPtr hwndOwner, int x, int y, string lpszCaption, int cObjects, [In, MarshalAs(UnmanagedType.Interface)] ref object ppUnk, int cPages, IntPtr pPageClsID, int lcid, int dwReserved, IntPtr pvReserved);

		#endregion

		#region DllImport (kernel32)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="timeOut"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]
		public static extern int WaitForSingleObject(IntPtr handle, int timeOut);

		/// <summary>
		/// 指定されたメモリブロックの内容を、他の場所へコピーします。
		/// </summary>
		/// <param name="Destination">コピー先の開始アドレスへのポインタを指定します。</param>
		/// <param name="Source">コピー元のメモリブロックの開始アドレスへのポインタを指定します。</param>
		/// <param name="Length">コピーしたいメモリブロックのバイト数を指定します。</param>
		[DllImport("kernel32.dll")]
		public static extern void CopyMemory(IntPtr Destination, IntPtr Source, uint Length);

		#endregion

		#region DllImport (user32)

		/// <summary>
		/// 指定されたウィンドウのクライアント領域またはスクリーン全体に対応するディスプレイデバイスコンテキストのハンドルを取得します。
		/// </summary>
		/// <param name="hWnd">ウィンドウハンドル</param>
		/// <returns>
		/// 成功するとデバイスコンテキストのハンドルが返ります。
		/// 失敗すると 0 (NULL) が返ります。
		/// </returns>
		[DllImport("user32")]
		public static extern HDC GetDC(HWND hWnd);

		/// <summary>
		/// デバイスコンテキストを解放します。
		/// </summary>
		/// <param name="hWnd">ウィンドウハンドル</param>
		/// <param name="hDC">デバイスコンテキストのハンドル</param>
		/// <returns>
		/// デバイスコンテキストが解放されたときは 1 が返ります。
		/// デバイスコンテキスト解放されなかったときは 0 が返ります。
		/// </returns>
		[DllImport("user32")]
		public static extern int ReleaseDC(HWND hWnd, HDC hDC);

		#endregion

		#region DllImport (gdi32)

		/// <summary>
		/// 指定されたデバイスコンテキストに関連するデバイスと互換性のあるメモリデバイスコンテキストを作成します。
		/// </summary>
		/// <param name="hDC">デバイスコンテキストのハンドル</param>
		/// <returns>
		/// 成功すると作成したメモリデバイスコンテキストのハンドルが返ります。
		/// 失敗すると0 (NULL) が返ります。
		/// </returns>
		[DllImport("gdi32")]
		public static extern HDC CreateCompatibleDC(HDC hDC);

		/// <summary>
		/// 指定されたデバイスコンテキストを削除します。
		/// </summary>
		/// <param name="hDC">デバイスコンテキストのハンドル</param>
		/// <returns>
		/// 成功すると 0 以外の値が返ります。
		/// 失敗すると 0 が返ります。
		/// </returns>
		[DllImport("gdi32")]
		public static extern BOOL DeleteDC(HDC hDC);

		/// <summary>
		/// 指定されたデバイスコンテキストで、指定された 1 個のオブジェクトを選択します。新しいオブジェクトは、同じタイプの以前のオブジェクトを置き換えます。
		/// </summary>
		/// <param name="hdc">デバイスコンテキストのハンドル</param>
		/// <param name="hgdiobj">オブジェクトのハンドル</param>
		/// <returns>
		/// リージョン以外のオブジェクトを指定した場合に関数が成功すると、置き換えが発生する前のオブジェクトのハンドルが返ります。
		/// </returns>
		[DllImport("gdi32")]
		public static extern HGDIOBJ SelectObject(HDC hdc, HGDIOBJ hgdiobj);

		/// <summary>
		/// ペン、ブラシ、フォント、ビットマップ、リージョン、パレットのいずれかの論理オブジェクトを削除し、そのオブジェクトに関連付けられていたすべてのシステムリソースを解放します。オブジェクトを削除した後は、指定されたハンドルは無効になります。
		/// </summary>
		/// <param name="hObject">グラフィックオブジェクトのハンドル</param>
		/// <returns>
		/// 関数が成功すると、0 以外の値が返ります。
		/// 指定したハンドルが有効でない場合、またはデバイスコンテキストでそのオブジェクトが選択されている場合は、0 が返ります。
		/// </returns>
		[DllImport("gdi32")]
		public static extern int DeleteObject(HGDIOBJ hObject);

		/// <summary>
		/// 画像のビットブロック転送を行ないます。
		/// </summary>
		/// <param name="hdcDest">コピー先デバイスコンテキスト</param>
		/// <param name="nXDest">コピー先x座標</param>
		/// <param name="nYDest">コピー先y座標</param>
		/// <param name="nWidth">コピーする幅</param>
		/// <param name="nHeight">コピーする高さ</param>
		/// <param name="hdcSource">コピー元デバイスコンテキスト</param>
		/// <param name="nXSource">コピー元x座標</param>
		/// <param name="nYSource">コピー元y座標</param>
		/// <param name="dwRaster">ラスタオペレーションコード</param>
		/// <returns>
		/// </returns>
		[DllImport("gdi32")]
		public static extern BOOL BitBlt(HDC hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, HDC hdcSource, int nXSource, int nYSource, DWORD dwRaster);

		/// <summary>
		/// 指定されたデバイス独立ビットマップ（DIB）内の長方形ピクセルの色データを、指定された長方形へコピーします。
		/// </summary>
		/// <param name="hdc">コピー先のデバイスコンテキストのハンドル (HDC)</param>
		/// <param name="XDest">コピー先長方形の左上隅の x 座標</param>
		/// <param name="YDest">コピー先長方形の左上隅の y 座標</param>
		/// <param name="nDestWidth">コピー先長方形の幅</param>
		/// <param name="nDestHeight">コピー先長方形の高さ</param>
		/// <param name="XSrc">コピー元長方形の左上隅の x 座標</param>
		/// <param name="YSrc">コピー元長方形の左上隅の x 座標</param>
		/// <param name="nSrcWidth">コピー元長方形の幅</param>
		/// <param name="nSrcHeight">コピー元長方形の高さ</param>
		/// <param name="lpBits">ビットマップのビット (const void*)</param>
		/// <param name="lpBitsInfo">ビットマップのデータ (BITMAPINFO*)</param>
		/// <param name="iUsage">データの種類 (DIB_PAL_COLORS/DIB_RGB_COLORS)</param>
		/// <param name="dwRop">ラスタオペレーションコード</param>
		/// <returns>
		/// 関数が成功すると、コピーされた走査行の数が返ります。
		/// 関数が失敗すると、GDI_ERROR が返ります。
		/// </returns>
		[DllImport("gdi32")]
		public static extern int StretchDIBits(HDC hdc, int XDest, int YDest, int nDestWidth, int nDestHeight, int XSrc, int YSrc, int nSrcWidth, int nSrcHeight, IntPtr lpBits, ref BITMAPINFO lpBitsInfo, uint iUsage, uint dwRop);

		/// <summary>
		/// アプリケーションから直接書き込み可能なデバイス独立のビットマップ（DIB）を作成します。
		/// </summary>
		/// <param name="hdc">デバイスコンテキストのハンドル(HDC)</param>
		/// <param name="pbmi">ビットマップデータ(CONST BITMAPINFO *)</param>
		/// <param name="iUsage">データ種類のインジケータ</param>
		/// <param name="ppvBits">ビット値(VOID **)</param>
		/// <param name="hSection">ファイルマッピングオブジェクトのハンドル(HANDLE)</param>
		/// <param name="dwOffset">ビットマップのビット値へのオフセット</param>
		/// <returns>
		///		関数が成功すると、新しく作成された DIB のハンドルが返ります。
		///		そして、*ppvBits は、ビットマップのビット値を指します。
		///		関数が失敗すると、NULL が返ります。
		///		そして、*ppvBits は NULL になります。
		/// </returns>
		[DllImport("gdi32")]
		public static extern IntPtr CreateDIBSection(HDC hdc, ref BITMAPINFO pbmi, uint iUsage, ref IntPtr ppvBits, HANDLE hSection, uint dwOffset);

		/// <summary>
		/// デバイス独立ビットマップ（DIB）からデバイス依存ビットマップ（DDB）を作成します。オプションで、ビットマップ内のビットを設定することもできます。
		/// </summary>
		/// <param name="hdc">デバイスコンテキストのハンドル</param>
		/// <param name="lpbmih">ビットマップのデータ</param>
		/// <param name="fdwInit">初期化オプション (0 または CBM_INIT)</param>
		/// <param name="lpbInit">初期化データ(CONST VOID *)</param>
		/// <param name="lpbmi">色形式のデータ</param>
		/// <param name="fuUsage">色データの使い方(DIB_PAL_COLORS/DIB_RGB_COLORS)</param>
		/// <returns>
		///		関数が成功すると、ビットマップのハンドルが返ります。
		///		関数が失敗すると、NULL が返ります。
		/// </returns>
		[DllImport("gdi32")]
		public static extern HBITMAP CreateDIBitmap(HDC hdc, ref BITMAPINFOHEADER lpbmih, DWORD fdwInit, IntPtr lpbInit, ref BITMAPINFO lpbmi, uint fuUsage);

		#endregion

		#region 定数.(DIB)

		/// <summary>
		/// CreateDIBitmap の fdwInit に指定する定数
		/// </summary>
		/// <remarks>
		/// CBM_INIT を指定した場合、lpbInit と lpbmi の各パラメータが指すデータを使って、ビットマップのビットを初期化します。 
		/// このフラグを指定しなかった場合、これらのパラメータが指すデータを使いません。
		/// </remarks>
		public const uint CBM_INIT = 0x04;

		/// <summary>
		/// カラーテーブルを提供します。カラーテーブルは、ビットマップを選択するデバイスコンテキストの論理パレットに関係する 16 ビットのインデックスからなる 1 個の配列です。
		/// </summary>
		public const uint DIB_PAL_COLORS = 1;

		/// <summary>
		/// カラーテーブルを提供します。カラーテーブルは、RGB 値そのものです。
		/// </summary>
		public const uint DIB_RGB_COLORS = 0;

		#endregion

		#region 定数.(圧縮の有無)

		/// <summary>
		/// 圧縮の有無 (無圧縮拡張形式)
		/// </summary>
		public const uint BI_RGB = 0x00;
		
		/// <summary>
		/// 圧縮の有無 (8bit 連長圧縮形式)
		/// </summary>
		public const uint BI_RLE8 = 0x01;
		
		/// <summary>
		/// 圧縮の有無 (4bit 連長圧縮形式)
		/// </summary>
		public const uint BI_RLE4 = 0x02;

		/// <summary>
		/// 圧縮の有無 (無圧縮拡張形式(16/32bit))
		/// </summary>
		public const uint BI_BITFIELDS = 0x03;

		#endregion

		#region 定数.(ラスターオペレーション)

		/// <summary>
		/// コピー元長方形をコピー先長方形へそのままコピーします。
		/// </summary>
		public const uint SRCCOPY = (uint)0x00CC0020;
		/// <summary>
		/// 論理 OR 演算子を使って、コピー元の色とコピー先の色を組み合わせます。
		/// </summary>
		public const uint SRCPAINT = (uint)0x00EE0086;
		/// <summary>
		/// 論理 AND 演算子を使って、コピー元の色とコピー先の色を組み合わせます。
		/// </summary>
		public const uint SRCAND = (uint)0x008800C6;
		/// <summary>
		/// 論理 XOR 演算子を使って、コピー元の色とコピー先の色を組み合わせます。
		/// </summary>
		public const uint SRCINVERT = (uint)0x00660046;
		/// <summary>
		/// 論理 AND 演算子を使って、コピー先の色を反転した色と、コピー元の色を組み合わせます。
		/// </summary>
		public const uint SRCERASE = (uint)0x00440328;
		/// <summary>
		/// コピー元の色を反転して、コピー先へコピーします。
		/// </summary>
		public const uint NOTSRCCOPY = (uint)0x00330008;
		/// <summary>
		/// 論理 OR 演算子を使って、コピー元の色とコピー先の色を組み合わせ、さらに反転します。
		/// </summary>
		public const uint NOTSRCERASE = (uint)0x001100A6;
		/// <summary>
		/// 論理 AND 演算子を使って、コピー元の色とコピー先の色を組み合わせます。
		/// </summary>
		public const uint MERGECOPY = (uint)0x00C000CA;
		/// <summary>
		/// 論理 OR 演算子を使って、コピー元の色を反転した色と、コピー先の色を組み合わせます。
		/// </summary>
		public const uint MERGEPAINT = (uint)0x00BB0226;
		/// <summary>
		/// 指定したパターンをコピー先へコピーします。
		/// </summary>
		public const uint PATCOPY = (uint)0x00F00021;
		/// <summary>
		/// 論理 OR 演算子を使って、指定したパターンの色と、コピー元の色を反転した色を組み合わせます。さらに論理 OR 演算子を使って、その結果と、コピー先の色を組み合わせます。
		/// </summary>
		public const uint PATPAINT = (uint)0x00FB0A09;
		/// <summary>
		/// 論理 XOR 演算子を使って、指定したパターンの色と、コピー先の色を組み合わせます。
		/// </summary>
		public const uint PATINVERT = (uint)0x005A0049;
		/// <summary>
		/// コピー先長方形の色を反転します。
		/// </summary>
		public const uint DSTINVERT = (uint)0x00550009;
		/// <summary>
		/// 物理パレットのインデックス 0 に対応する色（既定の物理パレットでは黒）で、コピー先の長方形を塗りつぶします。
		/// </summary>
		public const uint BLACKNESS = (uint)0x00000042;
		/// <summary>
		/// 物理パレットのインデックス 1 に対応する色（既定の物理パレットでは白）で、コピー先の長方形を塗りつぶします。
		/// </summary>
		public const uint WHITENESS = (uint)0x00FF0062;

		#endregion
	}

	#endregion

}
