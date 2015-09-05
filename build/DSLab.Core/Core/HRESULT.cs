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
	/// <summary>
	/// エラーコード定数
	/// </summary>
	/// <remarks>
	/// 各メソッドが返すコードの定数です。
	/// VFW_E, E_ で始まるコードはエラーを示します。
	/// VFW_S, S_ で始まるコードは成功を示します。
	/// https://msdn.microsoft.com/ja-jp/library/cc354470.aspx
	/// </remarks>
	[ComVisible(false)]
	public enum HRESULT : uint
	{
		/// <summary>
		/// 指定されたメディア タイプは無効です。
		/// </summary>
		VFW_E_INVALIDMEDIATYPE = 0x80040200,
		/// <summary>
		/// 指定されたメディア サブタイプは無効です。
		/// </summary>
		VFW_E_INVALIDSUBTYPE = 0x80040201,
		/// <summary>
		/// このオブジェクトは集成オブジェクトとしてのみ作成できる。
		/// </summary>
		VFW_E_NEED_OWNER = 0x80040202,
		/// <summary>
		/// 列挙オブジェクトの状態が変化して、列挙子の状態との矛盾が発生した。
		/// </summary>
		VFW_E_ENUM_OUT_OF_SYNC = 0x80040203,
		/// <summary>
		/// 処理に含まれるピンが既に少なくとも 1 つ接続されている。
		/// </summary>
		VFW_E_ALREADY_CONNECTED = 0x80040204,
		/// <summary>
		/// フィルタがアクティブなので、この処理を実行できない。
		/// </summary>
		VFW_E_FILTER_ACTIVE = 0x80040205,
		/// <summary>
		/// 指定されたいずれかのピンがメディア タイプをサポートしていない。
		/// </summary>
		VFW_E_NO_TYPES = 0x80040206,
		/// <summary>
		/// これらのピンに共通のメディア タイプがない。
		/// </summary>
		VFW_E_NO_ACCEPTABLE_TYPES = 0x80040207,
		/// <summary>
		/// 同じ方向のピンを 2 つ接続することはできない。
		/// </summary>
		VFW_E_INVALID_DIRECTION = 0x80040208,
		/// <summary>
		/// ピンが接続されていないため、処理を実行できない。
		/// </summary>
		VFW_E_NOT_CONNECTED = 0x80040209,
		/// <summary>
		/// サンプル バッファ アロケータが利用不可能。
		/// </summary>
		VFW_E_NO_ALLOCATOR = 0x8004020a,
		/// <summary>
		/// 実行時エラーが発生した。
		/// </summary>
		VFW_E_RUNTIME_ERROR = 0x8004020b,
		/// <summary>
		/// バッファ空間が設定されていない。
		/// </summary>
		VFW_E_BUFFER_NOTSET = 0x8004020c,
		/// <summary>
		/// バッファの大きさが足りない。
		/// </summary>
		VFW_E_BUFFER_OVERFLOW = 0x8004020d,
		/// <summary>
		/// 無効なアラインメントが指定された。
		/// </summary>
		VFW_E_BADALIGN = 0x8004020e,
		/// <summary>
		/// アロケータはコミットされなかった。「IMemAllocator::Commit」を参照すること。
		/// </summary>
		VFW_E_ALREADY_COMMITTED = 0x8004020f,
		/// <summary>
		/// 1 つまたは複数のバッファがアクティブです。
		/// </summary>
		VFW_E_BUFFERS_OUTSTANDING = 0x80040210,
		/// <summary>
		/// アロケータがアクティブでないときはサンプルを割り当てることができない。
		/// </summary>
		VFW_E_NOT_COMMITTED = 0x80040211,
		/// <summary>
		/// サイズが設定されていないので、メモリを割り当てることができない。
		/// </summary>
		VFW_E_SIZENOTSET = 0x80040212,
		/// <summary>
		/// クロックが定義されていないので、同期を行えない。
		/// </summary>
		VFW_E_NO_CLOCK = 0x80040213,
		/// <summary>
		/// 品質シンクが定義されていないので、品質メッセージを送信できない。
		/// </summary>
		VFW_E_NO_SINK = 0x80040214,
		/// <summary>
		/// 必要なインターフェイスが実装されていない。
		/// </summary>
		VFW_E_NO_INTERFACE = 0x80040215,
		/// <summary>
		/// オブジェクトまたは名前が見つからなかった。
		/// </summary>
		VFW_E_NOT_FOUND = 0x80040216,
		/// <summary>
		/// 接続を確立する中間フィルタの組み合わせが見つからなかった。
		/// </summary>
		VFW_E_CANNOT_CONNECT = 0x80040217,
		/// <summary>
		/// ストリームをレンダリングするフィルタの組み合わせが見つからなかった。
		/// </summary>
		VFW_E_CANNOT_RENDER = 0x80040218,
		/// <summary>
		/// フォーマットを動的に変更できない。
		/// </summary>
		VFW_E_CHANGING_FORMAT = 0x80040219,
		/// <summary>
		/// カラー キーが設定されていない。
		/// </summary>
		VFW_E_NO_COLOR_KEY_SET = 0x8004021a,
		/// <summary>
		/// 現在のピン接続は IOverlay 転送を使っていない。
		/// </summary>
		VFW_E_NOT_OVERLAY_CONNECTION = 0x8004021b,
		/// <summary>
		/// 現在のピン接続は IMemInputPin 転送を使っていない。
		/// </summary>
		VFW_E_NOT_SAMPLE_CONNECTION = 0x8004021c,
		/// <summary>
		/// カラー キーを設定すると、既に設定されているパレットと矛盾する可能性がある。
		/// </summary>
		VFW_E_PALETTE_SET = 0x8004021d,
		/// <summary>
		/// パレットを設定すると、既に設定されているカラー キーと矛盾する可能性がある。
		/// </summary>
		VFW_E_COLOR_KEY_SET = 0x8004021e,
		/// <summary>
		/// 一致するカラー キーがない。
		/// </summary>
		VFW_E_NO_COLOR_KEY_FOUND = 0x8004021f,
		/// <summary>
		/// パレットが利用不可能。
		/// </summary>
		VFW_E_NO_PALETTE_AVAILABLE = 0x80040220,
		/// <summary>
		/// ディスプレイはパレットを使わない。
		/// </summary>
		VFW_E_NO_DISPLAY_PALETTE = 0x80040221,
		/// <summary>
		/// 現在のディスプレイ設定に対して色が多すぎる。
		/// </summary>
		VFW_E_TOO_MANY_COLORS = 0x80040222,
		/// <summary>
		/// サンプルの処理を待っている間に状態が変化した。
		/// </summary>
		VFW_E_STATE_CHANGED = 0x80040223,
		/// <summary>
		/// フィルタが停止していないので、処理を実行できない。
		/// </summary>
		VFW_E_NOT_STOPPED = 0x80040224,
		/// <summary>
		/// フィルタが停止していないため、処理を実行できなかった。
		/// </summary>
		VFW_E_NOT_PAUSED = 0x80040225,
		/// <summary>
		/// フィルタが実行されていないので、処理を実行できない。
		/// </summary>
		VFW_E_NOT_RUNNING = 0x80040226,
		/// <summary>
		/// フィルタが不正な状態にあるため、処理を実行できなかった。
		/// </summary>
		VFW_E_WRONG_STATE = 0x80040227,
		/// <summary>
		/// サンプルの開始タイムがサンプルの終了タイムの後になっている。
		/// </summary>
		VFW_E_START_TIME_AFTER_END = 0x80040228,
		/// <summary>
		/// 提供された矩形が無効です。
		/// </summary>
		VFW_E_INVALID_RECT = 0x80040229,
		/// <summary>
		/// このピンは、提供されたメディア タイプを使えない。
		/// </summary>
		VFW_E_TYPE_NOT_ACCEPTED = 0x8004022a,
		/// <summary>
		/// このサンプルはレンダリングできない。
		/// </summary>
		VFW_E_SAMPLE_REJECTED = 0x8004022b,
		/// <summary>
		/// ストリームの終わりに到達しているので、このサンプルをレンダリングできない。
		/// </summary>
		VFW_E_SAMPLE_REJECTED_EOS = 0x8004022c,
		/// <summary>
		/// 同じ名前のフィルタを追加しようとしたが失敗した。
		/// </summary>
		VFW_E_DUPLICATE_NAME = 0x8004022d,
		/// <summary>
		/// 同じ名前のフィルタを追加しようとしたところ、名前を変更して処理が成功した。
		/// </summary>
		VFW_S_DUPLICATE_NAME = 0x4022d,
		/// <summary>
		/// タイムアウト期間が過ぎた。
		/// </summary>
		VFW_E_TIMEOUT = 0x8004022e,
		/// <summary>
		/// ファイル フォーマットが無効です。
		/// </summary>
		VFW_E_INVALID_FILE_FORMAT = 0x8004022f,
		/// <summary>
		/// リストが使い果たされた。
		/// </summary>
		VFW_E_ENUM_OUT_OF_RANGE = 0x80040230,
		/// <summary>
		/// フィルタ グラフが循環している。
		/// </summary>
		VFW_E_CIRCULAR_GRAPH = 0x80040231,
		/// <summary>
		/// この状態での更新は許されない。
		/// </summary>
		VFW_E_NOT_ALLOWED_TO_SAVE = 0x80040232,
		/// <summary>
		/// 過去のタイムのコマンドをキューに入れようとした。
		/// </summary>
		VFW_E_TIME_ALREADY_PASSED = 0x80040233,
		/// <summary>
		/// キューに入れられたコマンドは既にキャンセルされていた。
		/// </summary>
		VFW_E_ALREADY_CANCELLED = 0x80040234,
		/// <summary>
		/// ファイルが壊れているのでレンダリングできない。
		/// </summary>
		VFW_E_CORRUPT_GRAPH_FILE = 0x80040235,
		/// <summary>
		/// IOverlay アドバイズ リンクが既に存在している。
		/// </summary>
		VFW_E_ADVISE_ALREADY_SET = 0x80040236,
		/// <summary>
		/// 状態の移行が完了していない。
		/// </summary>
		VFW_S_STATE_INTERMEDIATE = 0x40237,
		/// <summary>
		/// フルスクリーン モードは利用できない。
		/// </summary>
		VFW_E_NO_MODEX_AVAILABLE = 0x80040238,
		/// <summary>
		/// このアドバイズは正常に設定されていないのでキャンセルできない。
		/// </summary>
		VFW_E_NO_ADVISE_SET = 0x80040239,
		/// <summary>
		/// フルスクリーン モードは利用できない。
		/// </summary>
		VFW_E_NO_FULLSCREEN = 0x8004023a,
		/// <summary>
		/// フルスクリーン モードでは IVideoWindow メソッドを呼び出せない。
		/// </summary>
		VFW_E_IN_FULLSCREEN_MODE = 0x8004023b,
		/// <summary>
		/// このファイルのメディア タイプが認識されない。
		/// </summary>
		VFW_E_UNKNOWN_FILE_TYPE = 0x80040240,
		/// <summary>
		/// このファイルのソース フィルタをロードできない。
		/// </summary>
		VFW_E_CANNOT_LOAD_SOURCE_FILTER = 0x80040241,
		/// <summary>
		/// このムービーにサポートされないフォーマットのストリームが含まれている。
		/// </summary>
		VFW_S_PARTIAL_RENDER = 0x40242,
		/// <summary>
		/// ファイルが不完全です。
		/// </summary>
		VFW_E_FILE_TOO_SHORT = 0x80040243,
		/// <summary>
		/// ファイルのバージョン番号が無効です。
		/// </summary>
		VFW_E_INVALID_FILE_VERSION = 0x80040244,
		/// <summary>
		/// ファイルにいくつかの使用されていないプロパティ設定が含まれている。
		/// </summary>
		VFW_S_SOME_DATA_IGNORED = 0x40245,
		/// <summary>
		/// 一部の接続が失敗して遅延した。
		/// </summary>
		VFW_S_CONNECTIONS_DEFERRED = 0x40246,
		/// <summary>
		/// このファイルは壊れている。無効なクラス識別子が含まれている。
		/// </summary>
		VFW_E_INVALID_CLSID = 0x80040247,
		/// <summary>
		/// このファイルは壊れている。無効なメディア タイプが含まれている。
		/// </summary>
		VFW_E_INVALID_MEDIA_TYPE = 0x80040248,
		/// <summary>
		/// このサンプルにはタイム スタンプが設定されていない。
		/// </summary>
		VFW_E_SAMPLE_TIME_NOT_SET = 0x80040249,
		/// <summary>
		/// 指定されたリソースはもはや必要ない。
		/// </summary>
		VFW_S_RESOURCE_NOT_NEEDED = 0x40250,
		/// <summary>
		/// このサンプルにはメディア タイムが設定されていない。
		/// </summary>
		VFW_E_MEDIA_TIME_NOT_SET = 0x80040251,
		/// <summary>
		/// メディア タイム フォーマットが選択されていない。
		/// </summary>
		VFW_E_NO_TIME_FORMAT_SET = 0x80040252,
		/// <summary>
		/// オーディオ デバイスがモノラル専用なので、バランスを変更できない。
		/// </summary>
		VFW_E_MONO_AUDIO_HW = 0x80040253,
		/// <summary>
		/// 永続グラフのメディア タイプに接続できない。
		/// </summary>
		VFW_S_MEDIA_TYPE_IGNORED = 0x40254,
		/// <summary>
		/// ビデオ ストリームを再生できない。適切なデコンプレッサが見つからなかった。
		/// </summary>
		VFW_E_NO_DECOMPRESSOR = 0x80040255,
		/// <summary>
		/// オーディオ ストリームを再生できない。オーディオ ハードウェアが利用できない、またはハードウェアがサポートされていない。
		/// </summary>
		VFW_E_NO_AUDIO_HARDWARE = 0x80040256,
		/// <summary>
		/// ビデオ ストリームを再生できない。適切なレンダラが見つからなかった。
		/// </summary>
		VFW_S_VIDEO_NOT_RENDERED = 0x40257,
		/// <summary>
		/// オーディオ ストリームを再生できない。適切なレンダラが見つからなかった。
		/// </summary>
		VFW_S_AUDIO_NOT_RENDERED = 0x40258,
		/// <summary>
		/// ビデオ ストリームを再生できない。フォーマット 'RPZA' はサポートされていない。
		/// </summary>
		VFW_E_RPZA = 0x80040259,
		/// <summary>
		/// ビデオ ストリームを再生できない。フォーマット 'RPZA' はサポートされていない。
		/// </summary>
		VFW_S_RPZA = 0x4025a,
		/// <summary>
		/// DirectShow はこのプロセッサ上で MPEG ムービーを再生できない。
		/// </summary>
		VFW_E_PROCESSOR_NOT_SUITABLE = 0x8004025b,
		/// <summary>
		/// オーディオ ストリームを再生できない。このオーディオ フォーマットはサポートされていない。
		/// </summary>
		VFW_E_UNSUPPORTED_AUDIO = 0x8004025c,
		/// <summary>
		/// ビデオ ストリームを再生できない。このビデオ フォーマットはサポートされていない。
		/// </summary>
		VFW_E_UNSUPPORTED_VIDEO = 0x8004025d,
		/// <summary>
		/// このビデオ ストリームは規格に準拠していないので DirectShow で再生できない。
		/// </summary>
		VFW_E_MPEG_NOT_CONSTRAINED = 0x8004025e,
		/// <summary>
		/// フィルタ グラフに存在しないオブジェクトに要求された関数を実行できない。
		/// </summary>
		VFW_E_NOT_IN_GRAPH = 0x8004025f,
		/// <summary>
		/// 返された値は予測値です。値の正確さを保証できない。
		/// </summary>
		VFW_S_ESTIMATED = 0x40260,
		/// <summary>
		/// オブジェクトのタイム フォーマットにアクセスできない。
		/// </summary>
		VFW_E_NO_TIME_FORMAT = 0x80040261,
		/// <summary>
		/// ストリームが読み出し専用で、フィルタによってデータが変更されているので、接続を確立できない。
		/// </summary>
		VFW_E_READ_ONLY = 0x80040262,
		/// <summary>
		/// この成功コードは、DirectShow の内部処理用に予約されている。
		/// </summary>
		VFW_S_RESERVED = 0x40263,
		/// <summary>
		/// バッファが十分に満たされていない。
		/// </summary>
		VFW_E_BUFFER_UNDERFLOW = 0x80040264,
		/// <summary>
		/// ファイルを再生できない。フォーマットがサポートされていない。
		/// </summary>
		VFW_E_UNSUPPORTED_STREAM = 0x80040265,
		/// <summary>
		/// 同じ転送をサポートしていないのでピンどうしを接続できない。
		/// </summary>
		VFW_E_NO_TRANSPORT = 0x80040266,
		/// <summary>
		/// ストリームがオフになった。
		/// </summary>
		VFW_S_STREAM_OFF = 0x40267,
		/// <summary>
		/// フィルタはアクティブだが、データを出力することができない。「IMediaFilter::GetState」を参照すること。
		/// </summary>
		VFW_S_CANT_CUE = 0x40268,
		/// <summary>
		/// デバイスがビデオ CD を正常に読み出せない、またはビデオ CD のデータが壊れている。
		/// </summary>
		VFW_E_BAD_VIDEOCD = 0x80040269,
		/// <summary>
		/// サンプルに終了タイムではなく開始タイムが設定されていた。この場合、返される終了タイムは開始タイムに 1 を加えた値に設定される。
		/// </summary>
		VFW_S_NO_STOP_TIME = 0x80040270,
		/// <summary>
		/// このディスプレイ解像度と色数に対してビデオ メモリが不十分です。解像度を低くするとよい。
		/// </summary>
		VFW_E_OUT_OF_VIDEO_MEMORY = 0x80040271,
		/// <summary>
		/// ビデオ ポート接続ネゴシエーション プロセスが失敗した。
		/// </summary>
		VFW_E_VP_NEGOTIATION_FAILED = 0x80040272,
		/// <summary>
		/// Microsoft DirectDraw がインストールされていない、またはビデオ カードの能力が適切でない。ディスプレイが 16 色モードでないことを確認すること。
		/// </summary>
		VFW_E_DDRAW_CAPS_NOT_SUITABLE = 0x80040273,
		/// <summary>
		/// ビデオ ポート ハードウェアが利用できない、またはハードウェアが応答しない。
		/// </summary>
		VFW_E_NO_VP_HARDWARE = 0x80040274,
		/// <summary>
		/// キャプチャ ハードウェアが利用できない、またはハードウェアが応答しない。
		/// </summary>
		VFW_E_NO_CAPTURE_HARDWARE = 0x80040275,
		/// <summary>
		/// この時点でこのユーザー操作は DVD コンテンツによって禁止されている。
		/// </summary>
		VFW_E_DVD_OPERATION_INHIBITED = 0x80040276,
		/// <summary>
		/// 現在のドメインでこの処理は許可されていない。
		/// </summary>
		VFW_E_DVD_INVALIDDOMAIN = 0x80040277,
		/// <summary>
		/// 要求されたボタンが利用できない。
		/// </summary>
		VFW_E_DVD_NO_BUTTON = 0x80040278,
		/// <summary>
		/// DVD-Video 再生グラフが作成されていない。
		/// </summary>
		VFW_E_DVD_GRAPHNOTREADY = 0x80040279,
		/// <summary>
		/// DVD-Video 再生グラフの作成が失敗した。
		/// </summary>
		VFW_E_DVD_RENDERFAIL = 0x8004027a,
		/// <summary>
		/// デコーダが不十分だったために、DVD-Video 再生グラフが作成できなかった。
		/// </summary>
		VFW_E_DVD_DECNOTENOUGH = 0x8004027b,
		/// <summary>
		/// Version number of DirectDraw not suitable. Make sure to install dx5 or higher version.%0
		/// </summary>
		VFW_E_DDRAW_VERSION_NOT_SUITABLE = 0x8004027c,
		/// <summary>
		/// Copy protection cannot be enabled. Please make sure any other copy protected content is not being shown now.%0
		/// </summary>
		VFW_E_COPYPROT_FAILED = 0x8004027d,
		/// <summary>
		/// There was no preview pin available, so the capture pin output is being split to provide both capture and preview.%0
		/// </summary>
		VFW_S_NOPREVIEWPIN = 0x4027e,
		/// <summary>
		/// This object cannot be used anymore as its time has expired.%0
		/// </summary>
		VFW_E_TIME_EXPIRED = 0x8004027f,
		/// <summary>
		/// The current title was not a sequential set of chapters (PGC), and the returned timing information might not be continuous.%0
		/// </summary>
		VFW_S_DVD_NON_ONE_SEQUENTIAL = 0x40280,
		/// <summary>
		/// The operation cannot be performed at the current playback speed.%0
		/// </summary>
		VFW_E_DVD_WRONG_SPEED = 0x80040281,
		/// <summary>
		/// The specified menu doesn't exist.%0
		/// </summary>
		VFW_E_DVD_MENU_DOES_NOT_EXIST = 0x80040282,
		/// <summary>
		/// The specified command was either cancelled or no longer exists.%0
		/// </summary>
		VFW_E_DVD_CMD_CANCELLED = 0x80040283,
		/// <summary>
		/// The data did not contain a recognized version.%0
		/// </summary>
		VFW_E_DVD_STATE_WRONG_VERSION = 0x80040284,
		/// <summary>
		/// The state data was corrupt.%0
		/// </summary>
		VFW_E_DVD_STATE_CORRUPT = 0x80040285,
		/// <summary>
		/// The state data is from a different disc.%0
		/// </summary>
		VFW_E_DVD_STATE_WRONG_DISC = 0x80040286,
		/// <summary>
		/// The region was not compatible with the current drive.%0
		/// </summary>
		VFW_E_DVD_INCOMPATIBLE_REGION = 0x80040287,
		/// <summary>
		/// The requested DVD stream attribute does not exist.%0
		/// </summary>
		VFW_E_DVD_NO_ATTRIBUTES = 0x80040288,
		/// <summary>
		/// Currently there is no GoUp (Annex J user function) program chain (PGC).%0
		/// </summary>
		VFW_E_DVD_NO_GOUP_PGC = 0x80040289,
		/// <summary>
		/// The current parental level was too low.%0
		/// </summary>
		VFW_E_DVD_LOW_PARENTAL_LEVEL = 0x8004028a,
		/// <summary>
		/// DVD ナビゲータはカラオケ モードではない。
		/// </summary>
		VFW_E_DVD_NOT_IN_KARAOKE_MODE = 0x8004028b,
		/// <summary>
		/// The audio stream did not contain sufficient information to determine the contents of each channel.%0
		/// </summary>
		VFW_S_DVD_CHANNEL_CONTENTS_NOT_AVAILABLE = 0x4028c,
		/// <summary>
		/// The seek into the movie was not frame accurate.%0
		/// </summary>
		VFW_S_DVD_NOT_ACCURATE = 0x4028d,
		/// <summary>
		/// コマ送りはサポートされていない。
		/// </summary>
		VFW_E_FRAME_STEP_UNSUPPORTED = 0x8004028e,
		/// <summary>
		/// The specified stream is disabled and cannot be selected.%0
		/// </summary>
		VFW_E_DVD_STREAM_DISABLED = 0x8004028f,
		/// <summary>
		/// The operation depends on the current title number, however the navigator has not yet entered the VTSM or the title domains,
		/// </summary>
		VFW_E_DVD_TITLE_UNKNOWN = 0x80040290,
		/// <summary>
		/// The specified path does not point to a valid DVD disc.%0
		/// </summary>
		VFW_E_DVD_INVALID_DISC = 0x80040291,
		/// <summary>
		/// There is currently no resume information.%0
		/// </summary>
		VFW_E_DVD_NO_RESUME_INFORMATION = 0x80040292,
		/// <summary>
		/// ピンは既に呼び出し元のスレッドでブロックされている。
		/// </summary>
		VFW_E_PIN_ALREADY_BLOCKED_ON_THIS_THREAD = 0x80040293,
		/// <summary>
		/// ピンは既に他のスレッドでブロックされている。
		/// </summary>
		VFW_E_PIN_ALREADY_BLOCKED = 0x80040294,
		/// <summary>
		/// このフィルタの使用は、ソフトウェア キーによって制限されている。アプリケーションは、フィルタのロックを解除しなければならない。
		/// </summary>
		VFW_E_CERTIFICATION_FAILURE = 0x80040295,
		/// <summary>
		/// The VMR has not yet created a mixing component.  That is, IVMRFilterConfig::SetNumberofStreams has not yet been called.%0
		/// </summary>
		VFW_E_VMR_NOT_IN_MIXER_MODE = 0x80040296,
		/// <summary>
		/// The application has not yet provided the VMR filter with a valid allocator-presenter object.%0
		/// </summary>
		VFW_E_VMR_NO_AP_SUPPLIED = 0x80040297,
		/// <summary>
		/// The VMR could not find any de-interlacing hardware on the current display device.%0
		/// </summary>
		VFW_E_VMR_NO_DEINTERLACE_HW = 0x80040298,
		/// <summary>
		/// The VMR could not find any ProcAmp hardware on the current display device.%0
		/// </summary>
		VFW_E_VMR_NO_PROCAMP_HW = 0x80040299,
		/// <summary>
		/// VMR9 does not work with VPE-based hardware decoders.%0
		/// </summary>
		VFW_E_DVD_VMR9_INCOMPATIBLEDEC = 0x8004029a,
		/// <summary>
		/// レジストリ エントリが壊れている。
		/// </summary>
		VFW_E_BAD_KEY = 0x800403f2,
		/// <summary>
		/// The Specified property set is not supported.%0
		/// </summary>
		E_PROP_SET_UNSUPPORTED = 0x80070492,
		/// <summary>
		/// The specified property ID is not supported for the specified property set.%0
		/// </summary>
		E_PROP_ID_UNSUPPORTED = 0x80070490,
		/// <summary>
		/// 操作は正常に終了した。
		/// </summary>
		S_OK = 0,
		/// <summary>
		/// メソッドは成功したが、行うことが何もなかった。
		/// </summary>
		S_FALSE = 1,
		/// <summary>
		/// このメソッドは実装されていない。
		/// </summary>
		E_NOTIMPL = 0x80004001,
		/// <summary>
		/// Ran out of memory
		/// </summary>
		E_OUTOFMEMORY = 0x8007000e,
		/// <summary>
		/// One or more arguments are invalid
		/// </summary>
		E_INVALIDARG = 0x80070057,
		/// <summary>
		/// No such interface supported
		/// </summary>
		E_NOINTERFACE = 0x80004002,
		/// <summary>
		/// Invalid pointer
		/// </summary>
		E_POINTER = 0x80004003,
		/// <summary>
		/// Invalid handle
		/// </summary>
		E_HANDLE = 0x80070006,
		/// <summary>
		/// Operation aborted
		/// </summary>
		E_ABORT = 0x80004004,
		/// <summary>
		/// Unspecified error
		/// </summary>
		E_FAIL = 0x80004005,
		/// <summary>
		/// General access denied error
		/// </summary>
		E_ACCESSDENIED = 0x80070005
	}
}
