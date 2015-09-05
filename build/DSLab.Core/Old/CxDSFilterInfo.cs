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
using System.Xml.Serialization;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;
using System.Text;
using System.IO;

namespace DSLab
{
	/// <summary>
	/// フィルタ情報
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public class CxDSFilterInfo : System.Object
		, ICloneable
	{
		#region コンストラクタ.

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDSFilterInfo()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">フィルタ名称</param>
		/// <param name="clsid">フィルタの GUID 文字列</param>
		/// <param name="index">指標 (0~)</param>
		public CxDSFilterInfo(string name, string clsid, int index)
		{
			Name = name;
			CLSID = clsid;
			Index = index;
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// フィルタ名称
		/// </summary>
		[Description("P:DSLab.CxDSFilterInfo.Name")]
		public virtual string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}
		private string m_Name = "";

		/// <summary>
		/// フィルタのカテゴリ
		/// </summary>
		[Description("P:DSLab.CxDSFilterInfo.CLSID")]
		public virtual string CLSID
		{
			get { return m_CLSID; }
			set { m_CLSID = value; }
		}
		private string m_CLSID = "";

		/// <summary>
		/// フィルタの序数 (0~)
		/// </summary>
		[Description("P:DSLab.CxDSFilterInfo.Index")]
		public virtual int Index
		{
			get { return m_Index; }
			set { m_Index = value; }
		}
		private int m_Index = 0;

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual CxDSFilterInfo Clone()
		{
			DSLab.CxDSFilterInfo clone = new CxDSFilterInfo();
			clone.Name = this.Name;
			clone.CLSID = this.CLSID;
			clone.Index = this.Index;
			return clone;
		}

		#endregion

		/// <summary>
		/// 文字列化
		/// </summary>
		/// <returns>
		/// 	フィルタ名称を返します。
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0}, {1}", Name, Index);
		}

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
				if (destinationType == typeof(CxDSFilterInfo))
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
				if (destinationType == typeof(string) && value is CxDSFilterInfo)
				{
					CxDSFilterInfo _value = (CxDSFilterInfo)value;
					return _value.ToString();
				}
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
	}
}

		#region フィルタ情報リストの取得:

		/// <summary>
		/// フィルタ情報リストの取得
		/// </summary>
		/// <param name="category">列挙するカテゴリのGUID</param>
		/// <returns>
		///		フィルタ情報のコレクションを返します。
		///	</returns>
		public static List<CxDSFilterInfo> GetFilterList(string category)
		{
			return GetFilterList(new Guid(category));
		}

		/// <summary>
		/// フィルタ情報リストの取得
		/// </summary>
		/// <param name="category">列挙するカテゴリのGUID</param>
		/// <returns>
		///		フィルタ情報のコレクションを返します。
		///	</returns>
		public static List<CxDSFilterInfo> GetFilterList(Guid category)
		{
			var result = new List<CxDSFilterInfo>();

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
				IntPtr fetch = IntPtr.Zero;

				while (enumerator.Next(monikers.Length, monikers, fetch) == 0)
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
						var info = new CxDSFilterInfo();

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
