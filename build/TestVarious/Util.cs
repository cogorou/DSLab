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
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using DSLab;

namespace demo
{
	public class Util
	{
		#region GetFilterList

		/// <summary>
		/// フィルタ一覧の取得
		/// </summary>
		/// <param name="category">デバイスのカテゴリ</param>
		/// <returns>
		///		取得したフィルタ情報のコレクションを返します。
		/// </returns>
		public static List<CxFilterInfo> GetFilterList(Guid category)
		{
			var result = new List<CxFilterInfo>();
			System.Runtime.InteropServices.ComTypes.IEnumMoniker enumerator = null;
			ICreateDevEnum device = null;

			try
			{
				// ICreateDevEnum インターフェース取得.
				device = (ICreateDevEnum)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_SystemDeviceEnum)));

				// EnumMonikerの作成.
				device.CreateClassEnumerator(ref category, ref enumerator, 0);
				if (enumerator == null)
					return result;

				// 列挙.
				var monikers = new System.Runtime.InteropServices.ComTypes.IMoniker[1];
				var fetched = IntPtr.Zero;

				while (enumerator.Next(monikers.Length, monikers, fetched) == 0)
				{
					// プロパティバッグへのバインド.
					IPropertyBag propbag = null;
					{
						object tmp = null;
						Guid guid = new Guid(GUID.IID_IPropertyBag);
						monikers[0].BindToStorage(null, null, ref guid, out tmp);
						propbag = (IPropertyBag)tmp;
					}

					try
					{
						var info = new CxFilterInfo();

						// 名前取得.
						try
						{
							object friendly_name = null;
							propbag.Read("FriendlyName", ref friendly_name, 0);
							info.Name = (string)friendly_name;
						}
						catch (Exception)
						{
						}

						// CLSID取得.
						try
						{
							object clsid = null;
							propbag.Read("CLSID", ref clsid, 0);
							info.CLSID = (string)clsid;
						}
						catch (Exception)
						{
						}

						// コレクションに追加.
						result.Add(info);
					}
					finally
					{
						// プロパティバッグの解放.
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

				// 同名フィルタの順番付け.
				for (int i = 0; i < result.Count - 1; i++)
				{
					for (int j = i + 1; j < result.Count; j++)
					{
						if (result[j].Name == result[i].Name)
						{
							result[j].Index = result[i].Index + 1;
							break;
						}
					}
				}
			}
			finally
			{
				if (enumerator != null)
					Marshal.ReleaseComObject(enumerator);
				if (device != null)
					Marshal.ReleaseComObject(device);
			}

			return result;
		}

		#endregion

		#region GetPinList

		/// <summary>
		/// ピン一覧の取得
		/// </summary>
		/// <param name="filter">対象のフィルタ</param>
		/// <returns>
		///		取得したピン情報のコレクションを返します。
		/// </returns>
		public static List<CxPinInfo> GetPinList(IBaseFilter filter)
		{
			var result = new List<CxPinInfo>();
			IEnumPins enumpins = null;

			try
			{
				filter.EnumPins(ref enumpins);

				while (true)
				{
					IPin pin = null;
					int fetched = 0;
					if (enumpins.Next(1, ref pin, ref fetched) < 0) break;
					if (fetched == 0) break;

					var info = new PIN_INFO();

					try
					{
						pin.QueryPinInfo(info);
						var dpi = new CxPinInfo(info.achName, info.dir);
						result.Add(dpi);
					}
					finally
					{
						if (info.pFilter != null)
							Marshal.ReleaseComObject(info.pFilter);
						if (pin != null)
							Marshal.ReleaseComObject(pin);
						pin = null;
					}
				}
			}
			finally
			{
				if (enumpins != null)
					Marshal.ReleaseComObject(enumpins);
			}

			return result;
		}

		#endregion

		#region GetFormatList

		/// <summary>
		/// フォーマット一覧の取得
		/// </summary>
		/// <param name="pin">対象のピン</param>
		/// <returns>
		///		取得したフォーマット情報のコレクションを返します。
		/// </returns>
		public static List<CxFormatInfo> GetFormatList(IPin pin)
		{
			var result = new List<CxFormatInfo>();
			if (pin == null)
				return result;

			var config = pin as IAMStreamConfig;
			if (config == null)
				return result;

			IntPtr dataptr = IntPtr.Zero;

			try
			{
				int count = 0;
				int size = 0;
				config.GetNumberOfCapabilities(ref count, ref size);

				dataptr = Marshal.AllocHGlobal(size);

				for (int i = 0; i < count; i++)
				{
					AM_MEDIA_TYPE mt = null;

					try
					{
						config.GetStreamCaps(i, ref mt, dataptr);

						// 基本情報の取得.
						var info = new CxFormatInfo();
						info.MediaType = GUID.Normalize(mt.majortype.ToString());
						info.MediaSubType = GUID.Normalize(mt.subtype.ToString());
						info.FormatType = GUID.Normalize(mt.formattype.ToString());

						// 映像形式か否か.
						if (GUID.Compare(info.FormatType, GUID.FORMAT_VideoInfo))
						{
							var vih = new VIDEOINFOHEADER();
							vih = (VIDEOINFOHEADER)Marshal.PtrToStructure(mt.pbFormat, typeof(VIDEOINFOHEADER));
							info.VideoSize = new Size(vih.bmiHeader.biWidth, vih.bmiHeader.biHeight);
						}

						// コレクションに追加.
						result.Add(info);
					}
					finally
					{
						if (mt != null)
							Axi.FreeMediaType(ref mt);
					}
				}
			}
			finally
			{
				if (dataptr != IntPtr.Zero)
					Marshal.FreeHGlobal(dataptr);
			}

			return result;
		}

		#endregion

		#region GetInterface

		/// <summary>
		/// 任意のインターフェースの取得
		/// </summary>
		/// <param name="graph">対象のグラフ</param>
		/// <returns>
		///		取得したインターフェースを返します。
		/// </returns>
		public static TI GetInterface<TI>(IGraphBuilder graph)
		{
			if (graph == null) return default(TI);

			IEnumFilters pEnum = null;
			graph.EnumFilters(ref pEnum);
			if (pEnum == null) return default(TI);

			while (true)
			{
				IBaseFilter filter = null;
				int fetched = 0;
				int status = pEnum.Next(1, ref filter, ref fetched);
				if (status != 0) break;

				if (filter is TI)
					return (TI)filter;
			}

			return default(TI);
		}

		#endregion
	}

	#region CxFilterInfo

	/// <summary>
	/// フィルタ情報
	/// </summary>
	public class CxFilterInfo
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxFilterInfo()
		{
			Name = "";
			CLSID = "";
			Index = 0;
		}

		/// <summary>
		/// 名称
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// CLSID
		/// </summary>
		public virtual string CLSID { get; set; }

		/// <summary>
		/// 指標 [0~] ※同名のフィルタを区別する為の指標です。
		/// </summary>
		public virtual int Index { get; set; }
	}

	#endregion

	#region CxPinInfo

	/// <summary>
	/// ピン情報
	/// </summary>
	public class CxPinInfo
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPinInfo()
		{
			Name = "";
			Direction = PIN_DIRECTION.PINDIR_OUTPUT;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="dir">方向</param>
		public CxPinInfo(string name, PIN_DIRECTION dir)
		{
			Name = name;
			Direction = dir;
		}

		/// <summary>
		/// 名称
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// 方向
		/// </summary>
		public virtual PIN_DIRECTION Direction { get; set; }
	}

	#endregion

	#region CxFormatInfo

	/// <summary>
	/// フォーマット情報
	/// </summary>
	public class CxFormatInfo
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxFormatInfo()
		{
			MediaType = "";
			MediaSubType = "";
			FormatType = "";
			VideoSize = new Size();
		}

		/// <summary>
		/// メディアタイプ
		/// </summary>
		public virtual string MediaType { get; set; }

		/// <summary>
		/// メディアサブタイプ
		/// </summary>
		public virtual string MediaSubType { get; set; }

		/// <summary>
		/// フォーマットタイプ
		/// </summary>
		public virtual string FormatType { get; set; }

		/// <summary>
		/// ビデオサイズ
		/// </summary>
		public virtual Size VideoSize { get; set; }
	}

	#endregion
}
