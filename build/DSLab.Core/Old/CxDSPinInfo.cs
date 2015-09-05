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
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;
using System.Text;
using System.IO;

namespace DSLab
{
	/// <summary>
	/// ピン情報
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public class CxDSPinInfo : System.Object
		, ICloneable
	{
		#region コンストラクタ.

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDSPinInfo()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">ピン名称</param>
		/// <param name="direction">ピンの向き</param>
		public CxDSPinInfo(string name, PIN_DIRECTION direction)
		{
			Name = name;
			Direction = direction;
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// ピン名称
		/// </summary>
		[Description("P:DSLab.CxPinInfo.Name")]
		public virtual string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}
		private string m_Name = "";

		/// <summary>
		/// ピンの方向
		/// </summary>
		[Description("P:DSLab.CxPinInfo.Direction")]
		public virtual PIN_DIRECTION Direction
		{
			get { return m_Direction; }
			set { m_Direction = value; }
		}
		private PIN_DIRECTION m_Direction = PIN_DIRECTION.PINDIR_OUTPUT;

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
		public virtual CxDSPinInfo Clone()
		{
			DSLab.CxDSPinInfo clone = new CxDSPinInfo();
			clone.Name = this.Name;
			clone.Direction = this.Direction;
			return clone;
		}

		#endregion

		/// <summary>
		/// 文字列化
		/// </summary>
		/// <returns>
		/// 	ピン名称を返します。
		/// </returns>
		public override string ToString()
		{
			return Name;
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
				if (destinationType == typeof(CxDSPinInfo))
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
				if (destinationType == typeof(string) && value is CxDSPinInfo)
				{
					CxDSPinInfo _value = (CxDSPinInfo)value;
					return string.Format("{0}", _value.Name);
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

		#region ピン情報リストの取得:

		/// <summary>
		/// ピン情報リストの取得
		/// </summary>
		/// <param name="filter">フィルタ</param>
		/// <returns>
		///		ピンの情報(CxPinInfo)のリストを返します。
		/// </returns>
		public static List<CxDSPinInfo> GetPinList(IBaseFilter filter)
		{
			var result = new List<CxDSPinInfo>();
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
						var dpi = new CxDSPinInfo(info.achName, info.dir);
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
				if (pin != null)
					Marshal.ReleaseComObject(pin);
			}

			return result;
		}

		#endregion
