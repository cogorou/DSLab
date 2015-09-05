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
	/// フォーマット形式
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public class CxDSFormatInfo : System.Object
		, ICloneable
	{
		#region コンストラクタ.

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDSFormatInfo()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="media_type">メディアタイプ</param>
		/// <param name="sub_type">メディアサブタイプ</param>
		/// <param name="format_type">フォーマットタイプ</param>
		public CxDSFormatInfo(string media_type, string sub_type, string format_type)
		{
			MediaType = media_type;
			MediaSubType = sub_type;
			FormatType = format_type;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="media_type">メディアタイプ</param>
		/// <param name="sub_type">メディアサブタイプ</param>
		/// <param name="format_type">フォーマットタイプ</param>
		/// <param name="video_size">ビデオサイズ</param>
		public CxDSFormatInfo(string media_type, string sub_type, string format_type, Size video_size)
		{
			MediaType = media_type;
			MediaSubType = sub_type;
			FormatType = format_type;
			VideoSize = video_size;
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// メディアタイプ
		/// </summary>
		/// <remarks>
		///		seealso: DSLab.GUID
		/// </remarks>
		[Description("P:DSLab.CxFormatInfo.MediaType")]
		public virtual string MediaType
		{
			get { return m_MediaType; }
			set { m_MediaType = value; }
		}
		private string m_MediaType = "";

		/// <summary>
		/// メディアサブタイプ
		/// </summary>
		/// <remarks>
		///		seealso: DSLab.GUID
		/// </remarks>
		[Description("P:DSLab.CxFormatInfo.MediaSubType")]
		public virtual string MediaSubType
		{
			get { return m_MediaSubType; }
			set { m_MediaSubType = value; }
		}
		private string m_MediaSubType = "";

		/// <summary>
		/// フォーマットタイプ
		/// </summary>
		/// <remarks>
		///		seealso: DSLab.GUID
		/// </remarks>
		[Description("P:DSLab.CxFormatInfo.FormatType")]
		public virtual string FormatType
		{
			get { return m_FormatType; }
			set { m_FormatType = value; }
		}
		private string m_FormatType = "";

		/// <summary>
		/// ビデオサイズ
		/// </summary>
		[Description("P:DSLab.CxFormatInfo.VideoSize")]
		public virtual Size VideoSize
		{
			get { return m_VideoSize; }
			set { m_VideoSize = value; }
		}
		private Size m_VideoSize = new Size(0, 0);

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
		/// インスタンスのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual CxDSFormatInfo Clone()
		{
			DSLab.CxDSFormatInfo clone = new CxDSFormatInfo();
			clone.MediaType = this.MediaType;
			clone.MediaSubType = this.MediaSubType;
			clone.FormatType = this.FormatType;
			clone.VideoSize = this.VideoSize;
			return clone;
		}

		#endregion

		/// <summary>
		/// 文字列化
		/// </summary>
		/// <returns>文字列化した情報を返します。</returns>
		public override string ToString()
		{
			string result =
				string.Format("{0}, {1}, {2}",
				DSLab.GUID.GetNickname(MediaSubType),
				VideoSize.Width,
				VideoSize.Height);
			return result;
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
				if (destinationType == typeof(CxDSFormatInfo))
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
				if (destinationType == typeof(string) && value is CxDSFormatInfo)
				{
					CxDSFormatInfo _value = (CxDSFormatInfo)value;
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

		#region フォーマット情報リストの取得:

		/// <summary>
		/// フォーマット情報リストの取得
		/// </summary>
		/// <param name="pin">ピン</param>
		/// <returns>
		///		フォーマット情報(CxFormatInfo)のリストを返します。
		///	</returns>
		public static List<CxDSFormatInfo> GetFormatList(IPin pin)
		{
			var result = new List<CxDSFormatInfo>();
			if (pin == null) return result;

			var config = pin as IAMStreamConfig;
			if (config == null)
				throw new DSLab.CxDSException(HRESULT.E_NOINTERFACE);

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
						CxDSFormatInfo info = new CxDSFormatInfo();
						info.MediaType = GUID.Normalize(mt.majortype.ToString());
						info.MediaSubType = GUID.Normalize(mt.subtype.ToString());
						info.FormatType = GUID.Normalize(mt.formattype.ToString());

						// 映像形式か否か.
						if (GUID.Compare(info.FormatType, GUID.FORMAT_VideoInfo))
						{
							VIDEOINFOHEADER vih = new VIDEOINFOHEADER();
							vih = (VIDEOINFOHEADER)Marshal.PtrToStructure(mt.pbFormat, typeof(VIDEOINFOHEADER));
							info.VideoSize = new Size(vih.bmiHeader.biWidth, vih.bmiHeader.biHeight);
						}

						// コレクションに追加.
						result.Add(info);
					}
					finally
					{
						if (mt != null)
							FreeMediaType(ref mt);
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
