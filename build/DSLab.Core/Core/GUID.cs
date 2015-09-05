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
	/// DirectShow 関連 GUID 定義
	/// </summary>
	public static class GUID
	{
		#region 文字列操作:

		/// <summary>
		/// GUID文字列の比較
		/// </summary>
		/// <param name="guid1">GUID文字列</param>
		/// <param name="guid2">GUID文字列</param>
		/// <returns>
		///		一致する場合は true を返します。
		///		一致しない場合は、false を返します。
		///		英字の大文字小文字、括弧({}）の有無による違いは無視します。
		/// </returns>
		public static bool Compare(string guid1, string guid2)
		{
			return Normalize(guid1) == Normalize(guid2);
		}

		/// <summary>
		/// GUID文字列の正規化
		/// </summary>
		/// <param name="guid">GUID文字列</param>
		/// <returns>
		///		下記の書式に整形されたGUID文字列を返します。<br/>
		///		(書式: {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx})<br/>
		///		英字は大文字に変換されます。
		/// </returns>
		public static string Normalize(string guid)
		{
			if (!guid.StartsWith("{")) guid = "{" + guid;
			if (!guid.EndsWith("}")) guid = guid + "}";
			return guid.ToUpper();
		}

		/// <summary>
		/// GUID のニックネーム取得
		/// </summary>
		/// <param name="guid">GUID</param>
		/// <returns>
		///		GUID をニックネームに変換して返します。
		///		不明な場合は、空白を返します。<br/>
		///		現在対応している GUID は以下の通りです。
		///		<pre>
		///		DSLab.GUID
		///		DSLab.GUID
		///		</pre>
		/// </returns>
		public static string GetNickname(Guid guid)
		{
			return GetNickname(guid.ToString());
		}

		/// <summary>
		/// GUID のニックネーム取得
		/// </summary>
		/// <param name="guid">GUID 文字列</param>
		/// <returns>
		///		GUID 文字列をニックネームに変換して返します。
		///		不明な場合は、空白を返します。<br/>
		///		現在対応している GUID は以下の通りです。
		///		<pre>
		///		DSLab.GUID
		///		DSLab.GUID
		///		</pre>
		/// </returns>
		public static string GetNickname(string guid)
		{
			string ss = Normalize(guid);
			string name = "";

			#region MediaType
			switch (ss)
			{
				case GUID.MEDIATYPE_Video: name = "Video"; break;
				case GUID.MEDIATYPE_Audio: name = "Audio"; break;
				case GUID.MEDIATYPE_Text: name = "Text"; break;
				case GUID.MEDIATYPE_Midi: name = "Midi"; break;
				case GUID.MEDIATYPE_Stream: name = "Stream"; break;
				case GUID.MEDIATYPE_Interleaved: name = "Interleaved"; break;
				case GUID.MEDIATYPE_File: name = "File"; break;
				case GUID.MEDIATYPE_ScriptCommand: name = "ScriptCommand"; break;
				case GUID.MEDIATYPE_AUXLine21Data: name = "AUXLine21Data"; break;
				case GUID.MEDIATYPE_VBI: name = "VBI"; break;
				case GUID.MEDIATYPE_Timecode: name = "Timecode"; break;
				case GUID.MEDIATYPE_LMRT: name = "LMRT"; break;
				//case GUID.MEDIATYPE_URL_STREAM: name = "URL_STREAM"; break;
			}
			#endregion

			#region MediaSubType
			switch (ss)
			{
				case GUID.MEDIASUBTYPE_None: name = "None"; break;
				case GUID.MEDIASUBTYPE_CLPL: name = "CLPL"; break;
				case GUID.MEDIASUBTYPE_YUYV: name = "YUYV"; break;
				case GUID.MEDIASUBTYPE_IYUV: name = "IYUV"; break;
				case GUID.MEDIASUBTYPE_YVU9: name = "YVU9"; break;
				case GUID.MEDIASUBTYPE_Y411: name = "Y411"; break;
				case GUID.MEDIASUBTYPE_Y41P: name = "Y41P"; break;
				case GUID.MEDIASUBTYPE_YUY2: name = "YUY2"; break;
				case GUID.MEDIASUBTYPE_YVYU: name = "YVYU"; break;
				case GUID.MEDIASUBTYPE_UYVY: name = "UYVY"; break;
				case GUID.MEDIASUBTYPE_Y211: name = "Y211"; break;
				case GUID.MEDIASUBTYPE_CLJR: name = "CLJR"; break;
				case GUID.MEDIASUBTYPE_IF09: name = "IF09"; break;
				case GUID.MEDIASUBTYPE_CPLA: name = "CPLA"; break;
				case GUID.MEDIASUBTYPE_MJPG: name = "MJPG"; break;
				case GUID.MEDIASUBTYPE_TVMJ: name = "TVMJ"; break;
				case GUID.MEDIASUBTYPE_WAKE: name = "WAKE"; break;
				case GUID.MEDIASUBTYPE_CFCC: name = "CFCC"; break;
				case GUID.MEDIASUBTYPE_IJPG: name = "IJPG"; break;
				case GUID.MEDIASUBTYPE_Plum: name = "Plum"; break;
				case GUID.MEDIASUBTYPE_DVSD: name = "DVSD"; break;
				case GUID.MEDIASUBTYPE_MDVF: name = "MDVF"; break;
				case GUID.MEDIASUBTYPE_RGB1: name = "RGB1"; break;
				case GUID.MEDIASUBTYPE_RGB4: name = "RGB4"; break;
				case GUID.MEDIASUBTYPE_RGB8: name = "RGB8"; break;
				case GUID.MEDIASUBTYPE_RGB565: name = "RGB565"; break;
				case GUID.MEDIASUBTYPE_RGB555: name = "RGB555"; break;
				case GUID.MEDIASUBTYPE_RGB24: name = "RGB24"; break;
				case GUID.MEDIASUBTYPE_RGB32: name = "RGB32"; break;
				case GUID.MEDIASUBTYPE_ARGB1555: name = "ARGB1555"; break;
				case GUID.MEDIASUBTYPE_ARGB4444: name = "ARGB4444"; break;
				case GUID.MEDIASUBTYPE_ARGB32: name = "ARGB32"; break;
				case GUID.MEDIASUBTYPE_A2R10G10B10: name = "A2R10G10B10"; break;
				case GUID.MEDIASUBTYPE_A2B10G10R10: name = "A2B10G10R10"; break;
				case GUID.MEDIASUBTYPE_AYUV: name = "AYUV"; break;
				case GUID.MEDIASUBTYPE_AI44: name = "AI44"; break;
				case GUID.MEDIASUBTYPE_IA44: name = "IA44"; break;
				case GUID.MEDIASUBTYPE_YV12: name = "YV12"; break;
				case GUID.MEDIASUBTYPE_NV12: name = "NV12"; break;
				case GUID.MEDIASUBTYPE_IMC1: name = "IMC1"; break;
				case GUID.MEDIASUBTYPE_IMC2: name = "IMC2"; break;
				case GUID.MEDIASUBTYPE_IMC3: name = "IMC3"; break;
				case GUID.MEDIASUBTYPE_IMC4: name = "IMC4"; break;
				case GUID.MEDIASUBTYPE_S340: name = "S340"; break;
				case GUID.MEDIASUBTYPE_S342: name = "S342"; break;
				case GUID.MEDIASUBTYPE_I420: name = "I420"; break;
				case GUID.MEDIASUBTYPE_Overlay: name = "Overlay"; break;
				case GUID.MEDIASUBTYPE_MPEGPacket: name = "MPEGPacket"; break;
				case GUID.MEDIASUBTYPE_MPEG1Payload: name = "MPEG1Payload"; break;
				case GUID.MEDIASUBTYPE_MPEG1AudioPayload: name = "MPEG1AudioPayload"; break;
				case GUID.MEDIASUBTYPE_MPEG1SystemStream: name = "MPEG1SystemStream"; break;
				case GUID.MEDIASUBTYPE_MPEG1VideoCD: name = "MPEG1VideoCD"; break;
				case GUID.MEDIASUBTYPE_MPEG1Video: name = "MPEG1Video"; break;
				case GUID.MEDIASUBTYPE_MPEG1Audio: name = "MPEG1Audio"; break;
				case GUID.MEDIASUBTYPE_Avi: name = "Avi"; break;
				case GUID.MEDIASUBTYPE_Asf: name = "Asf"; break;
				case GUID.MEDIASUBTYPE_QTMovie: name = "QTMovie"; break;
				case GUID.MEDIASUBTYPE_Rpza: name = "Rpza"; break;
				case GUID.MEDIASUBTYPE_Smc: name = "Smc"; break;
				case GUID.MEDIASUBTYPE_Rle: name = "Rle"; break;
				case GUID.MEDIASUBTYPE_Jpeg: name = "Jpeg"; break;
				case GUID.MEDIASUBTYPE_PCMAudio_Obsolete: name = "PCMAudio_Obsolete"; break;
				case GUID.MEDIASUBTYPE_PCM: name = "PCM"; break;
				case GUID.MEDIASUBTYPE_WAVE: name = "WAVE"; break;
				case GUID.MEDIASUBTYPE_AU: name = "AU"; break;
				case GUID.MEDIASUBTYPE_AIFF: name = "AIFF"; break;
				case GUID.MEDIASUBTYPE_dvsd2: name = "dvsd2"; break;
				case GUID.MEDIASUBTYPE_dvhd: name = "dvhd"; break;
				case GUID.MEDIASUBTYPE_dvsl: name = "dvsl"; break;
				case GUID.MEDIASUBTYPE_dv25: name = "dv25"; break;
				case GUID.MEDIASUBTYPE_dv50: name = "dv50"; break;
				case GUID.MEDIASUBTYPE_dvh1: name = "dvh1"; break;
				case GUID.MEDIASUBTYPE_Line21_BytePair: name = "Line21_BytePair"; break;
				case GUID.MEDIASUBTYPE_Line21_GOPPacket: name = "Line21_GOPPacket"; break;
				case GUID.MEDIASUBTYPE_Line21_VBIRawData: name = "Line21_VBIRawData"; break;
				case GUID.MEDIASUBTYPE_TELETEXT: name = "TELETEXT"; break;
			}
			#endregion

			return name;
		}
		#endregion

		#region MediaType

		// メディアタイプ(メジャータイプ)
		// http://msdn.microsoft.com/ja-jp/library/cc370108.aspx

		public const string MEDIATYPE_Video = "{73646976-0000-0010-8000-00AA00389B71}";
		public const string MEDIATYPE_Audio = "{73647561-0000-0010-8000-00AA00389B71}";
		public const string MEDIATYPE_Text = "{73747874-0000-0010-8000-00AA00389B71}";
		public const string MEDIATYPE_Midi = "{7364696D-0000-0010-8000-00AA00389B71}";
		public const string MEDIATYPE_Stream = "{E436EB83-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIATYPE_Interleaved = "{73766169-0000-0010-8000-00AA00389B71}";
		public const string MEDIATYPE_File = "{656C6966-0000-0010-8000-00AA00389B71}";
		public const string MEDIATYPE_ScriptCommand = "{73636D64-0000-0010-8000-00AA00389B71}";
		public const string MEDIATYPE_AUXLine21Data = "{670AEA80-3A82-11D0-B79B-00AA003767A7}";
		public const string MEDIATYPE_VBI = "{F72A76E1-EB0A-11D0-ACE4-0000C0CC16BA}";
		public const string MEDIATYPE_Timecode = "{0482DEE3-7817-11CF-8A03-00AA006ECB65}";
		public const string MEDIATYPE_LMRT = "{74726C6D-0000-0010-8000-00AA00389B71}";
		public const string MEDIATYPE_URL_STREAM = "{74726C6D-0000-0010-8000-00AA00389B71}";

		#endregion

		#region MediaSubType

		// メディアタイプ(サブタイプ)
		// http://msdn.microsoft.com/ja-jp/library/cc371321.aspx

		public const string MEDIASUBTYPE_None = "{E436EB8E-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_CLPL = "{4C504C43-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_YUYV = "{56595559-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_IYUV = "{56555949-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_YVU9 = "{39555659-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_Y411 = "{31313459-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_Y41P = "{50313459-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_YUY2 = "{32595559-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_YVYU = "{55595659-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_UYVY = "{59565955-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_Y211 = "{31313259-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_CLJR = "{524A4C43-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_IF09 = "{39304649-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_CPLA = "{414C5043-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_MJPG = "{47504A4D-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_TVMJ = "{4A4D5654-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_WAKE = "{454B4157-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_CFCC = "{43434643-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_IJPG = "{47504A49-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_Plum = "{6D756C50-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_DVSD = "{44535644-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_MDVF = "{4656444D-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_RGB1 = "{E436EB78-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_RGB4 = "{E436EB79-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_RGB8 = "{E436EB7A-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_RGB565 = "{E436EB7B-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_RGB555 = "{E436EB7C-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_RGB24 = "{E436EB7D-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_RGB32 = "{E436EB7E-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_ARGB1555 = "{297C55AF-E209-4CB3-B757-C76D6B9C88A8}";
		public const string MEDIASUBTYPE_ARGB4444 = "{6E6415E6-5C24-425F-93CD-80102B3D1CCA}";
		public const string MEDIASUBTYPE_ARGB32 = "{773C9AC0-3274-11D0-B724-00AA006C1A01}";
		public const string MEDIASUBTYPE_A2R10G10B10 = "{2F8BB76D-B644-4550-ACF3-D30CAA65D5C5}";
		public const string MEDIASUBTYPE_A2B10G10R10 = "{576F7893-BDF6-48C4-875F-AE7B81834567}";
		public const string MEDIASUBTYPE_AYUV = "{56555941-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_AI44 = "{34344941-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_IA44 = "{34344149-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_YV12 = "{32315659-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_NV12 = "{3231564E-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_IMC1 = "{31434D49-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_IMC2 = "{32434d49-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_IMC3 = "{33434d49-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_IMC4 = "{34434d49-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_S340 = "{30343353-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_S342 = "{32343353-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_I420 = "{30323449-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_Overlay = "{E436EB7F-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_MPEGPacket = "{E436EB80-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_MPEG1Payload = "{E436EB81-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_MPEG1AudioPayload = "{00000050-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_MPEG1SystemStream = "{E436EB82-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_MPEG1VideoCD = "{E436EB85-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_MPEG1Video = "{E436EB86-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_MPEG1Audio = "{E436EB87-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_Avi = "{E436EB88-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_Asf = "{3DB80F90-9412-11D1-ADED-0000F8754B99}";
		public const string MEDIASUBTYPE_QTMovie = "{E436EB89-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_Rpza = "{617A7072-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_Smc = "{20636D73-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_Rle = "{20656C72-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_Jpeg = "{6765706A-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_PCMAudio_Obsolete = "{E436EB8A-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_PCM = "{00000001-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_WAVE = "{E436EB8B-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_AU = "{E436EB8C-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_AIFF = "{E436EB8D-524F-11CE-9F53-0020AF0BA770}";
		public const string MEDIASUBTYPE_dvsd2 = "{64737664-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_dvhd = "{64687664-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_dvsl = "{6C737664-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_dv25 = "{35327664-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_dv50 = "{30357664-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_dvh1 = "{31687664-0000-0010-8000-00AA00389B71}";
		public const string MEDIASUBTYPE_Line21_BytePair = "{6E8D4A22-310C-11D0-B79A-00AA003767A7}";
		public const string MEDIASUBTYPE_Line21_GOPPacket = "{6E8D4A23-310C-11D0-B79A-00AA003767A7}";
		public const string MEDIASUBTYPE_Line21_VBIRawData = "{6E8D4A24-310C-11D0-B79A-00AA003767A7}";
		public const string MEDIASUBTYPE_TELETEXT = "{F72A76E3-EB0A-11D0-ACE4-0000C0CC16BA}";

		#endregion

		#region FormatType

		// フォーマットタイプ

		public const string FORMAT_None = "{0F6417D6-C318-11D0-A43F-00A0C9223196}";
		public const string FORMAT_VideoInfo = "{05589F80-C356-11CE-BF01-00AA0055595A}";
		public const string FORMAT_VideoInfo2 = "{F72A76A0-EB0A-11d0-ACE4-0000C0CC16BA}";
		public const string FORMAT_WaveFormatEx = "{05589F81-C356-11CE-BF01-00AA0055595A}";

		#endregion

		#region FilterCategory

		// フィルタカテゴリ
		// https://msdn.microsoft.com/ja-jp/library/cc353921.aspx

		public const string CLSID_AudioInputDeviceCategory = "{33D9A762-90C8-11d0-BD43-00A0C911CE86}";
		public const string CLSID_AudioCompressorCategory = "{33D9A761-90C8-11d0-BD43-00A0C911CE86}";
		public const string CLSID_AudioRendererCategory = "{E0F158E1-CB04-11d0-BD4E-00A0C911CE86}";
		public const string CLSID_AviSplitter = "{1b544c20-fd0b-11ce-8c63-00aa0044b51e}";
		public const string CLSID_AviReader = "{1b544c21-fd0b-11ce-8c63-00aa0044b51e}";
		public const string CLSID_DeviceControlCategory = "{CC7BFB46-F175-11d1-A392-00E0291F3959}";
		public const string CLSID_LegacyAmFilterCategory = "{083863F1-70DE-11d0-BD40-00A0C911CE86}";
		public const string CLSID_TransmitCategory = "{CC7BFB41-F175-11d1-A392-00E0291F3959}";
		public const string CLSID_MidiRendererCategory = "{4EFE2452-168A-11d1-BC76-00C04FB9453B}";
		public const string CLSID_VideoInputDeviceCategory = "{860BB310-5D01-11d0-BD3B-00A0C911CE86}";
		public const string CLSID_VideoCompressorCategory = "{33D9A760-90C8-11d0-BD43-00A0C911CE86}";
		public const string CLSID_AM_KSCATEGORY_CAPTURE = "{65E8773D-8F56-11D0-A3B9-00A0C9223196}";
		public const string CLSID_AM_KSCATEGORY_CROSSBAR = "{A799A801-A46D-11d0-A18C-00A02401DCD4}";
		public const string CLSID_AM_KSCATEGORY_RENDER = "{65e8773e-8f56-11d0-a3b9-00a0c9223196}";
		public const string CLSID_AM_KSCATEGORY_SPLITTER = "{0A4252A0-7E70-11D0-A5D6-28DB04C10000}";
		public const string CLSID_AM_KSCATEGORY_TVAUDIO = "{A799A802-A46D-11d0-A18C-00A02401DCD4}";
		public const string CLSID_AM_KSCATEGORY_TVTUNER = "{A799A800-A46D-11d0-A18C-00A02401DCD4}";
		public const string CLSID_AM_KSCATEGORY_VBICODEC = "{07DAD660-22F1-11d1-A9F4-00C04FBBDE8F}";
		public const string CLSID_ActiveMovieCategories = "{DA4E3DA0-D07D-11D0-BD50-00A0C911CE86}";
		public const string CLSID_KSCATEGORY_COMMUNICATIONSTRANSFORM = "{CF1DDA2C-9743-11D0-A3EE-00A0C9223196}";
		public const string CLSID_KSCATEGORY_DATATRANSFORM = "{2EB07EA0-7E70-11D0-A5D6-28DB04C10000}";
		public const string CLSID_KSCATEGORY_INTERFACETRANSFORM = "{CF1DDA2D-9743-11D0-A3EE-00A0C9223196}";
		public const string CLSID_KSCATEGORY_MIXER = "{AD809C00-7B88-11D0-A5D6-28DB04C10000}";
		public const string CLSID_KSCATEGORY_BDA_NETWORK_PROVIDER = "{71985F4B-1CA1-11D3-9CC8-00C04F7971E0}";
		public const string CLSID_KSCATEGORY_BDA_RECEIVER_COMPONENT = "{FD0A5AF4-B41D-11d2-9C95-00C04F7971E0}";
		public const string CLSID_KSCATEGORY_BDA_TRANSPORT_INFORMATION = "{A2E3074F-6C3D-11D3-B653-00C04F79498E}";
		public const string CLSID_WMAsfReader = "{187463A0-5BB7-11d3-ACBE-0080C75E246E}";
		public const string CLSID_WMAsfWriter = "{7c23220e-55bb-11d3-8b16-00c04fb6bd3d}";

		#endregion

		#region Filter

		// フィルタ
		// https://msdn.microsoft.com/ja-jp/library/cc353911.aspx
		// file: uuids.h (VC/PlatformSDK/Include)
		// C:\Program Files\Windows Kits\8.1\Include\shared\uuids.h
		// C:\Program Files (x86)\Windows Kits\8.1\Include\shared\uuids.h
		//	~~~~~~~~~~~~~~~~~~~~~ (1)          ~~~ (2)

		public const string CLSID_AVISplitter = "{1B544C20-FD0B-11CE-8C63-00AA0044B51E}";
		public const string CLSID_AVIReader = "{1B544C21-FD0B-11CE-8C63-00AA0044B51E}";
		public const string CLSID_AVIDec = "{CF49D4E0-1115-11CE-B03A-0020AF0BA770}";
		public const string CLSID_AVIDraw = "{A888DF60-1E90-11CF-AC98-00AA004C0FA9}";
		public const string CLSID_AVIMux = "{E2510970-F137-11CE-8B67-00AA00A3F1A6}";
		public const string CLSID_AVIWAVFileSource = "{D3588AB0-0781-11CE-B03A-0020AF0BA770}";
		public const string CLSID_MPEGIStreamSplitter = "{336475D0-942A-11CE-A870-00AA002FEAB5}";
		public const string CLSID_MPEGLayer3Decoder = "{38BE3000-DBF4-11D0-860E-00A024CFEF6D}";
		public const string CLSID_MPEG2Splitter = "{3AE86B20-7BE8-11D1-ABE6-00A0C905F375}";
		public const string CLSID_MPEGAudioDecoder = "{4A2286E0-7BEF-11CE-9BD9-0000E202599C}";
		public const string CLSID_MPEG2Demultiplexer = "{AFB6C280-2C41-11D3-8A60-0000F81E0E4A}";
		public const string CLSID_MPEG2SectionsandTables = "{C666E115-BB62-4027-A113-82D643FE2D99}";
		public const string CLSID_MPEGVideoDecoder = "{FEB50740-7BEF-11CE-9BD9-0000E202599C}";
		public const string CLSID_DivXDecoderFilter = "{78766964-0000-0010-8000-00AA00389B71}";
		public const string CLSID_AC3ParserFilter = "{280A3020-86CF-11D1-ABE6-00A0C905F375}";
		public const string CLSID_ACELPnetSiproLabAudioDecoder = "{4009F700-AEBA-11D1-8344-00C04FB92EB7}";
		public const string CLSID_ACMWrapper = "{6A08CF80-0E18-11CF-A24D-0020AFD79767}";
		public const string CLSID_ASFACMHandler = "{B9D1F321-C401-11D0-A520-000000000000}";
		public const string CLSID_ASFDIBHandler = "{B9D1F320-C401-11D0-A520-000000000000}";
		public const string CLSID_ASFDJPEGHandler = "{B9D1F325-C401-11D0-A520-000000000000}";
		public const string CLSID_ASFembeddedstuffHandler = "{B9D1F32E-C401-11D0-A520-000000000000}";
		public const string CLSID_ASFICMHandler = "{B9D1F322-C401-11D0-A520-000000000000}";
		public const string CLSID_ASFJPEGHandler = "{B9D1F324-C401-11D0-A520-000000000000}";
		public const string CLSID_ASFURLHandler = "{B9D1F323-C401-11D0-A520-000000000000}";
		public const string CLSID_ASXfileParser = "{640999A0-A946-11D0-A520-000000000000}";
		public const string CLSID_ASXv2fileParser = "{640999A1-A946-11D0-A520-000000000000}";
		public const string CLSID_AudioSource = "{18F39114-6AA8-11D2-8B55-00C04F797443}";
		public const string CLSID_AudioSynthesizer = "{79A98DE0-BC00-11CE-AC2E-444553540000}";
		public const string CLSID_AVDummyFilter = "{D74E6C66-DC8A-4BCD-808F-066FC71A1618}";
		public const string CLSID_BDAMPEG2TransportInformationFilter = "{FC772AB0-0C7F-11D3-8FF2-00A0C9224CF4}";
		public const string CLSID_BouncingBall = "{FD501041-8EBE-11CE-8183-00AA00577DA1}";
		public const string CLSID_ClipperFilter = "{501B7653-CA15-432C-AAD3-41ED64201217}";
		public const string CLSID_ColorConverter = "{637E3E39-462F-477E-9DAF-F07B9B1C00D2}";
		public const string CLSID_ColorSpaceConverter = "{1643E180-90F5-11CE-97D5-00AA0055595A}";
		public const string CLSID_CutlistFileSource = "{A5EA8D20-253D-11D1-B3F1-00AA003761C5}";
		public const string CLSID_CyberLinkAudioDecoder = "{9BC1B780-85E3-11D2-98D0-0080C84E9C39}";
		public const string CLSID_CyberLinkAudioEffect = "{3D5455E5-D8E8-4B4C-84AF-4703C5542042}";
		public const string CLSID_CyberLinkDxVAFilter2 = "{C494A68B-A398-4E2B-A63A-02578A84DFF4}";
		public const string CLSID_CyberLinkVideoSPDecoder = "{9BC1B781-85E3-11D2-98D0-0080C84E9C39}";
		public const string CLSID_DiskRecordQueue = "{5BB4BE4A-09B3-4689-BB4B-6F33E1E82797}";
		public const string CLSID_DMOWrapperFilter = "{94297043-BD82-4DFD-B0DE-8177739C6D20}";
		public const string CLSID_Dump = "{36A5F770-FE4C-11CE-A8ED-00AA002FEAB5}";
		public const string CLSID_DVDNavigator = "{9B8C4620-2C1A-11D0-8493-00A02438AD48}";
		public const string CLSID_DVMuxer = "{129D7E40-C10D-11D0-AFB9-00AA00B67A42}";
		public const string CLSID_DVSplitter = "{4EB31670-9FC6-11CF-AF6E-00AA00B67A42}";
		public const string CLSID_DVVideoDecoder = "{B1B77C00-C3E4-11CF-AF79-00AA00B67A42}";
		public const string CLSID_DVVideoEnc = "{13AA3650-BB6F-11d0-AFB9-00AA00B67A42}";
		public const string CLSID_FileSource_Async = "{E436EBB5-524F-11CE-9F53-0020AF0BA770}";
		public const string CLSID_FileSource_NetshowURL = "{4B428940-263C-11D1-A520-000000000000}";
		public const string CLSID_FileSource_URL = "{E436EBB6-524F-11CE-9F53-0020AF0BA770}";
		public const string CLSID_Filestreamrenderer = "{D51BD5A5-7548-11CF-A520-0080C77EF58A}";
		public const string CLSID_Filewriter = "{8596E5F0-0DA5-11D0-BD21-00A0C911CE86}";
		public const string CLSID_FullScreenRenderer = "{07167665-5011-11CF-BF33-00AA0055595A}";
		public const string CLSID_G711Codec = "{AF7D8180-A8F9-11CF-9A46-00AA00B7DAD1}";
		public const string CLSID_H261DecodeFilter = "{31363248-0000-0010-8000-00AA00389B71}";
		public const string CLSID_H261EncodeFilter = "{EFD08EC1-EA11-11CF-9FEC-00AA00A59F69}";
		public const string CLSID_H263DecodeFilter = "{33363248-0000-0010-8000-00AA00389B71}";
		public const string CLSID_H263EncodeFilter = "{C9076CE2-FB56-11CF-906C-00AA00A59F69}";
		public const string CLSID_hunuaaCropping = "{8ED21D59-66E8-4B62-819A-3FB86F05A7C1}";
		public const string CLSID_ImageEffects = "{8B498501-1218-11CF-ADC4-00A0D100041B}";
		public const string CLSID_IndeoRaudiosoftware = "{B4CA2970-DD2B-11D0-9DFA-00AA00AF3494}";
		public const string CLSID_IndeoRvideo44CompressionFilter = "{A2551F60-705F-11CF-A424-00AA003735BE}";
		public const string CLSID_IndeoRvideo44DecompressionFilter = "{31345649-0000-0010-8000-00AA00389B71}";
		public const string CLSID_IndeoRvideo511CompressionFilter = "{1F73E9B1-8C3A-11D0-A3BE-00A0C9244436}";
		public const string CLSID_IndeoRvideo511DecompressionFilter = "{30355649-0000-0010-8000-00AA00389B71}";
		public const string CLSID_InfinitePinTeeFilter = "{F8388A40-D5BB-11D0-BE5A-0080C706568E}";
		public const string CLSID_IntelRTPDemuxFilter = "{399D5C90-74AB-11D0-9CCF-00A0C9081C19}";
		public const string CLSID_IntelRTPRPHforG711G7231 = "{D42FEAC0-82A1-11D0-9643-00AA00A89C1D}";
		public const string CLSID_IntelRTPRPHforGenericAudio = "{ECB29E60-88ED-11D0-9643-00AA00A89C1D}";
		public const string CLSID_IntelRTPRPHforH263H261_B = "{EC941961-7DF6-11D0-9643-00AA00A89C1D}";
		public const string CLSID_IntelRTPSPHforG711G7231 = "{1AE60860-8297-11D0-9643-00AA00A89C1D}";
		public const string CLSID_IntelRTPSPHforGenericAudio = "{3DDDA000-88E4-11D0-9643-00AA00A89C1D}";
		public const string CLSID_IntelRTPSPHforH263H261_A = "{EC941960-7DF6-11D0-9643-00AA00A89C1D}";
		public const string CLSID_InternalScriptCommandRenderer = "{48025243-2D39-11CE-875D-00608CB78066}";
		public const string CLSID_IVFsourcefilter = "{C69E8F40-D5C8-11D0-A520-145405C10000}";
		public const string CLSID_Line21Decoder = "{6E8D4A20-310C-11D0-B79A-00AA003767A7}";
		public const string CLSID_Line21Decoder2 = "{E4206432-01A1-4BEE-B3E1-3702C8EDC574}";
		public const string CLSID_LyricParser = "{D51BD5A4-7548-11CF-A520-0080C77EF58A}";
		public const string CLSID_MemoFilter = "{7C4C601E-F338-46E6-9EA0-F047A5A2EFF1}";
		public const string CLSID_MicrosoftMPEG4VideoDecompressor = "{82CCD3E0-F71A-11D0-9FE5-00609778EA66}";
		public const string CLSID_MicrosoftPCMAudioMixer = "{CDCDD6A0-C016-11D0-82A4-00AA00B5CA1B}";
		public const string CLSID_MicrosoftScreenVideoDecompressor = "{3301A7C4-0A8D-11D4-914D-00C04F610D24}";
		public const string CLSID_MIDIParser = "{D51BD5A2-7548-11CF-A520-0080C77EF58A}";
		public const string CLSID_MinimalNull = "{08AF6540-4F21-11CF-AACB-0020AF0B99A3}";
		public const string CLSID_MJPEGDecompressor = "{301056D0-6DFF-11D2-9EEB-006008039E37}";
		public const string CLSID_MultifileParser = "{D51BD5A3-7548-11CF-A520-0080C77EF58A}";
		public const string CLSID_NSCfileParser = "{640999A2-A946-11D0-A520-000000000000}";
		public const string CLSID_NullRenderer = "{C1F400A4-3F08-11D3-9F0B-006008039E37}";
		public const string CLSID_Oscilloscope = "{35919F40-E904-11CE-8A03-00AA006ECB65}";
		public const string CLSID_OverlayMixer = "{CD8743A1-3736-11D0-9E69-00C04FD7C15B}";
		public const string CLSID_OverlayMixer2 = "{A0025E90-E45B-11D1-ABE9-00A0C905F375}";
		public const string CLSID_PCMSilenceSuppressor = "{26721E10-390C-11D0-8A22-00A0C90C9156}";
		public const string CLSID_QTDecompressor = "{FDFE9681-74A3-11D0-AFA7-00AA00B67A42}";
		public const string CLSID_QuickTimeMovieParser = "{D51BD5A0-7548-11CF-A520-0080C77EF58A}";
		public const string CLSID_RAMfileParser = "{A98C8400-4181-11D1-A520-00A0D10129C0}";
		public const string CLSID_RealPlayerAudioFilter = "{CEF4D40F-ACA5-40BA-8F3B-161A594A1A39}";
		public const string CLSID_RTPRenderFilter = "{00D20921-7E20-11D0-B291-00C04FC31D18}";
		public const string CLSID_RTPSourceFilter = "{00D20920-7E20-11D0-B291-00C04FC31D18}";
		public const string CLSID_SAMIParser = "{33FACFE0-A9BE-11D0-A520-00A0D10129C0}";
		public const string CLSID_SampleGrabber = "{C1F400A0-3F08-11D3-9F0B-006008039E37}";
		public const string CLSID_SampleGrabberExample = "{2FA4F053-6D60-4CB0-9503-8E89234F3F73}";
		public const string CLSID_SampleVideoRenderer = "{4D4B1600-33AC-11CF-BF30-00AA0055595A}";
		public const string CLSID_ScreenCapturefilter = "{B9330878-C1DB-11D3-B36B-00C04F6108FF}";
		public const string CLSID_SmartTee = "{CC58E280-8AA1-11D1-B3F1-00AA003761C5}";
		public const string CLSID_StreamBufferSink = "{2DB47AE5-CF39-43c2-B4D6-0CD8D90946F4}";
		public const string CLSID_StreamBufferSource = "{C9F5FE02-F851-4eb5-99EE-AD602AF1E619}";
		public const string CLSID_TextDisplay = "{E30629D3-27E5-11CE-875D-00608CB78066}";
		public const string CLSID_VBISurfaceAllocator = "{814B9800-1C88-11D1-BAD9-00609744111A}";
		public const string CLSID_VGA16ColorDitherer = "{1DA08500-9EDC-11CF-BC10-00AA00AC74F6}";
		public const string CLSID_VideoContrast = "{FD501043-8EBE-11CE-8183-00AA00577DA1}";
		public const string CLSID_VideoMixingRenderer9 = "{51B4ABF3-748F-4E3B-A276-C828330E926A}";
		public const string CLSID_VideoRenderer = "{70E102B0-5556-11CE-97C0-00AA0055595A}";
		public const string CLSID_VideoSource = "{DC0DADF0-8E0D-11D2-8B62-00C04F797443}";
		public const string CLSID_VoxwareMetaSoundAudioDecoder = "{73F7A062-8829-11D1-B550-006097242D8D}";
		public const string CLSID_VoxwareMetaVoiceAudioDecoder = "{46E32B01-A465-11D1-B550-006097242D8D}";
		public const string CLSID_WaveParser = "{D51BD5A1-7548-11CF-A520-0080C77EF58A}";
		public const string CLSID_WDMCaptureDevice = "{33156162-81D6-11D3-8006-00C04FA30A73}";
		public const string CLSID_WinampDSPtoDirectShow = "{87491715-CC71-4160-BDB1-24FF4FD250D8}";
		public const string CLSID_WindowsMediaAudioDecoder = "{22E24591-49D0-11D2-BB50-006008320064}";
		public const string CLSID_WindowsMediaMultiplexer = "{63F8AA94-E2B9-11D0-ADF6-00C04FB66DAD}";
		public const string CLSID_WindowsMediasourcefilter = "{6B6D0800-9ADA-11D0-A520-00A0D10129C0}";
		public const string CLSID_WindowsMediaUpdateFilter = "{B6353564-96C4-11D2-8DDB-006097C9A2B2}";
		public const string CLSID_WindowsMediaVideoDecoder_A = "{4FACBBA1-FFD8-4CD7-8228-61E2F65CB1AE}";
		public const string CLSID_WindowsMediaVideoDecoder_B = "{521FB373-7654-49F2-BDB1-0C6E6660714F}";
		public const string CLSID_WMASFReader = "{187463A0-5BB7-11D3-ACBE-0080C75E246E}";
		public const string CLSID_WMASFWriter = "{7C23220E-55BB-11D3-8B16-00C04FB6BD3D}";
		public const string CLSID_WSTDecoder = "{70BC06E0-5666-11D3-A184-00105AEF9F33}";
		public const string CLSID_XingRVideoCDNavigator = "{2D300C60-73EE-11D2-AFD4-006008AFEA28}";
		public const string CLSID_XMLPlaylist = "{D51BD5AE-7548-11CF-A520-0080C77EF58A}";

		#endregion

		#region InterfaceID

		// インターフェースＩＤ

		public const string IID_IPropertyBag = "{55272A00-42CB-11CE-8135-00AA004BB851}";
		public const string IID_IBaseFilter = "{56a86895-0ad4-11ce-b03a-0020af0ba770}";
		public const string IID_IAMStreamConfig = "{C6E13340-30AC-11d0-A18C-00A0C9118956}";
		public const string IID_IDMOWrapperFilter = "{52D6F586-9F0F-4824-8FC8-E32CA04930C2}";

		#endregion

		#region PinCategory

		// ピンプロパティセット
		// http://msdn.microsoft.com/ja-jp/library/cc370455.aspx
		// file: uuids.h (VC/PlatformSDK/Include)

		public const string PIN_CATEGORY_CAPTURE = "{fb6c4281-0353-11d1-905f-0000c0cc16ba}";
		public const string PIN_CATEGORY_PREVIEW = "{fb6c4282-0353-11d1-905f-0000c0cc16ba}";
		public const string PIN_CATEGORY_STILL = "{fb6c428a-0353-11d1-905f-0000c0cc16ba}";

		#endregion

		#region DirectShowObject

		// DirectShowオブジェクト	

		public const string CLSID_FilterGraph = "{E436EBB3-524F-11CE-9F53-0020AF0BA770}";
		public const string CLSID_SystemDeviceEnum = "{62BE5D10-60EB-11d0-BD3B-00A0C911CE86}";
		public const string CLSID_CaptureGraphBuilder2 = "{BF87B6E1-8C27-11d0-B3F0-00AA003761C5}";

		#endregion

		#region WMProfile

		// System Profiles
		// https://msdn.microsoft.com/ja-jp/library/windows/desktop/dd743748(v=vs.85).aspx
		// file: wmsysprf.h
		//       C:\%ProgramFiles%\Windows Kits\8.1\Include\um

		public const string WMProfile_V40_DialUpMBR = "{fd7f47f1-72a6-45a4-80f0-3aecefc32c07}";
		public const string WMProfile_V40_IntranetMBR = "{82cd3321-a94a-4ffc-9c2b-092c10ca16e7}";
		public const string WMProfile_V40_2856100MBR = "{5a1c2206-dc5e-4186-beb2-4c5a994b132e}";
		public const string WMProfile_V40_6VoiceAudio = "{D508978A-11A0-4d15-B0DA-ACDC99D4F890}";
		public const string WMProfile_V40_16AMRadio = "{0f4be81f-d57d-41e1-b2e3-2fad986bfec2}";
		public const string WMProfile_V40_288FMRadioMono = "{7fa57fc8-6ea4-4645-8abf-b6e5a8f814a1}";
		public const string WMProfile_V40_288FMRadioStereo = "{22fcf466-aa40-431f-a289-06d0ea1a1e40}";
		public const string WMProfile_V40_56DialUpStereo = "{e8026f87-e905-4594-a3c7-00d00041d1d9}";
		public const string WMProfile_V40_64Audio = "{4820b3f7-cbec-41dc-9391-78598714c8e5}";
		public const string WMProfile_V40_96Audio = "{0efa0ee3-9e64-41e2-837f-3c0038f327ba}";
		public const string WMProfile_V40_128Audio = "{93ddbe12-13dc-4e32-a35e-40378e34279a}";
		public const string WMProfile_V40_288VideoVoice = "{bb2bc274-0eb6-4da9-b550-ecf7f2b9948f}";
		public const string WMProfile_V40_288VideoAudio = "{ac617f2d-6cbe-4e84-8e9a-ce151a12a354}";
		public const string WMProfile_V40_288VideoWebServer = "{abf2f00d-d555-4815-94ce-8275f3a70bfe}";
		public const string WMProfile_V40_56DialUpVideo = "{e21713bb-652f-4dab-99de-71e04400270f}";
		public const string WMProfile_V40_56DialUpVideoWebServer = "{b756ff10-520f-4749-a399-b780e2fc9250}";
		public const string WMProfile_V40_100Video = "{8f99ddd8-6684-456b-a0a3-33e1316895f0}";
		public const string WMProfile_V40_250Video = "{541841c3-9339-4f7b-9a22-b11540894e42}";
		public const string WMProfile_V40_512Video = "{70440e6d-c4ef-4f84-8cd0-d5c28686e784}";
		public const string WMProfile_V40_1MBVideo = "{b4482a4c-cc17-4b07-a94e-9818d5e0f13f}";
		public const string WMProfile_V40_3MBVideo = "{55374ac0-309b-4396-b88f-e6e292113f28}";
		public const string WMProfile_V70_DialUpMBR = "{5B16E74B-4068-45b5-B80E-7BF8C80D2C2F}";
		public const string WMProfile_V70_IntranetMBR = "{045880DC-34B6-4ca9-A326-73557ED143F3}";
		public const string WMProfile_V70_2856100MBR = "{07DF7A25-3FE2-4a5b-8B1E-348B0721CA70}";
		public const string WMProfile_V70_288VideoVoice = "{B952F38E-7DBC-4533-A9CA-B00B1C6E9800}";
		public const string WMProfile_V70_288VideoAudio = "{58BBA0EE-896A-4948-9953-85B736F83947}";
		public const string WMProfile_V70_288VideoWebServer = "{70A32E2B-E2DF-4ebd-9105-D9CA194A2D50}";
		public const string WMProfile_V70_56VideoWebServer = "{DEF99E40-57BC-4ab3-B2D1-B6E3CAF64257}";
		public const string WMProfile_V70_64VideoISDN = "{C2B7A7E9-7B8E-4992-A1A1-068217A3B311}";
		public const string WMProfile_V70_100Video = "{D9F3C932-5EA9-4c6d-89B4-2686E515426E}";
		public const string WMProfile_V70_256Video = "{AFE69B3A-403F-4a1b-8007-0E21CFB3DF84}";
		public const string WMProfile_V70_384Video = "{F3D45FBB-8782-44df-97C6-8678E2F9B13D}";
		public const string WMProfile_V70_768Video = "{0326EBB6-F76E-4964-B0DB-E729978D35EE}";
		public const string WMProfile_V70_1500Video = "{0B89164A-5490-4686-9E37-5A80884E5146}";
		public const string WMProfile_V70_2000Video = "{AA980124-BF10-4e4f-9AFD-4329A7395CFF}";
		public const string WMProfile_V70_700FilmContentVideo = "{7A747920-2449-4d76-99CB-FDB0C90484D4}";
		public const string WMProfile_V70_1500FilmContentVideo = "{F6A5F6DF-EE3F-434c-A433-523CE55F516B}";
		public const string WMProfile_V70_6VoiceAudio = "{EABA9FBF-B64F-49b3-AA0C-73FBDD150AD0}";
		public const string WMProfile_V70_288FMRadioMono = "{C012A833-A03B-44a5-96DC-ED95CC65582D}";
		public const string WMProfile_V70_288FMRadioStereo = "{E96D67C9-1A39-4dc4-B900-B1184DC83620}";
		public const string WMProfile_V70_56DialUpStereo = "{674EE767-0949-4fac-875E-F4C9C292013B}";
		public const string WMProfile_V70_64AudioISDN = "{91DEA458-9D60-4212-9C59-D40919C939E4}";
		public const string WMProfile_V70_64Audio = "{B29CFFC6-F131-41db-B5E8-99D8B0B945F4}";
		public const string WMProfile_V70_96Audio = "{A9D4B819-16CC-4a59-9F37-693DBB0302D6}";
		public const string WMProfile_V70_128Audio = "{C64CF5DA-DF45-40d3-8027-DE698D68DC66}";
		public const string WMProfile_V70_225VideoPDA = "{F55EA573-4C02-42b5-9026-A8260C438A9F}";
		public const string WMProfile_V70_150VideoPDA = "{0F472967-E3C6-4797-9694-F0304C5E2F17}";
		public const string WMProfile_V80_255VideoPDA = "{FEEDBCDF-3FAC-4c93-AC0D-47941EC72C0B}";
		public const string WMProfile_V80_150VideoPDA = "{AEE16DFA-2C14-4a2f-AD3F-A3034031784F}";
		public const string WMProfile_V80_28856VideoMBR = "{D66920C4-C21F-4ec8-A0B4-95CF2BD57FC4}";
		public const string WMProfile_V80_100768VideoMBR = "{5BDB5A0E-979E-47d3-9596-73B386392A55}";
		public const string WMProfile_V80_288100VideoMBR = "{D8722C69-2419-4b36-B4E0-6E17B60564E5}";
		public const string WMProfile_V80_288Video = "{3DF678D9-1352-4186-BBF8-74F0C19B6AE2}";
		public const string WMProfile_V80_56Video = "{254E8A96-2612-405c-8039-F0BF725CED7D}";
		public const string WMProfile_V80_100Video = "{A2E300B4-C2D4-4fc0-B5DD-ECBD948DC0DF}";
		public const string WMProfile_V80_256Video = "{BBC75500-33D2-4466-B86B-122B201CC9AE}";
		public const string WMProfile_V80_384Video = "{29B00C2B-09A9-48bd-AD09-CDAE117D1DA7}";
		public const string WMProfile_V80_768Video = "{74D01102-E71A-4820-8F0D-13D2EC1E4872}";
		public const string WMProfile_V80_700NTSCVideo = "{C8C2985F-E5D9-4538-9E23-9B21BF78F745}";
		public const string WMProfile_V80_1400NTSCVideo = "{931D1BEE-617A-4bcd-9905-CCD0786683EE}";
		public const string WMProfile_V80_384PALVideo = "{9227C692-AE62-4f72-A7EA-736062D0E21E}";
		public const string WMProfile_V80_700PALVideo = "{EC298949-639B-45e2-96FD-4AB32D5919C2}";
		public const string WMProfile_V80_288MonoAudio = "{7EA3126D-E1BA-4716-89AF-F65CEE0C0C67}";
		public const string WMProfile_V80_288StereoAudio = "{7E4CAB5C-35DC-45bb-A7C0-19B28070D0CC}";
		public const string WMProfile_V80_32StereoAudio = "{60907F9F-B352-47e5-B210-0EF1F47E9F9D}";
		public const string WMProfile_V80_48StereoAudio = "{5EE06BE5-492B-480a-8A8F-12F373ECF9D4}";
		public const string WMProfile_V80_64StereoAudio = "{09BB5BC4-3176-457f-8DD6-3CD919123E2D}";
		public const string WMProfile_V80_96StereoAudio = "{1FC81930-61F2-436f-9D33-349F2A1C0F10}";
		public const string WMProfile_V80_128StereoAudio = "{407B9450-8BDC-4ee5-88B8-6F527BD941F2}";
		public const string WMProfile_V80_288VideoOnly = "{8C45B4C7-4AEB-4f78-A5EC-88420B9DADEF}";
		public const string WMProfile_V80_56VideoOnly = "{6E2A6955-81DF-4943-BA50-68A986A708F6}";
		public const string WMProfile_V80_FAIRVBRVideo = "{3510A862-5850-4886-835F-D78EC6A64042}";
		public const string WMProfile_V80_HIGHVBRVideo = "{0F10D9D3-3B04-4fb0-A3D3-88D4AC854ACC}";
		public const string WMProfile_V80_BESTVBRVideo = "{048439BA-309C-440e-9CB4-3DCCA3756423}";

		#endregion
	}
}
