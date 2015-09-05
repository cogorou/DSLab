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
	/// カメラパラメータクラス (DirectShow)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public class CxDSCameraParam : System.Object
		, ICloneable
		, IxEquatable
		, IxFileAccess
	{
		#region コンストラクタ.

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDSCameraParam()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="filter_info">フィルタ情報</param>
		/// <param name="pin_info">ピン情報</param>
		/// <param name="format_info">フォーマット情報</param>
		public CxDSCameraParam(CxDSFilterInfo filter_info, CxDSPinInfo pin_info, CxDSFormatInfo format_info)
		{
			FilterInfo = filter_info;
			PinInfo = pin_info;
			FormatInfo = format_info;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="filename">パラメータファイル(XML)</param>
		/// <param name="options">オプション。(未使用)</param>
		public CxDSCameraParam(string filename, params object[] options)
		{
			this.Load(filename, options);
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 入力フィルタ - フィルタ情報
		/// </summary>
		[CxDescription("P:DSLab.CxGrabberParam.FilterInfo")]
		public virtual CxDSFilterInfo FilterInfo
		{
			get { return m_FilterInfo; }
			set { m_FilterInfo = value; }
		}
		private CxDSFilterInfo m_FilterInfo = new CxDSFilterInfo();

		/// <summary>
		/// 入力フィルタ - ピン情報
		/// </summary>
		[CxDescription("P:DSLab.CxGrabberParam.PinInfo")]
		public virtual CxDSPinInfo PinInfo
		{
			get { return m_PinInfo; }
			set { m_PinInfo = value; }
		}
		private CxDSPinInfo m_PinInfo = new CxDSPinInfo();

		/// <summary>
		/// 入力フィルタ - フォーマット情報
		/// </summary>
		[CxDescription("P:DSLab.CxGrabberParam.FormatInfo")]
		public virtual CxDSFormatInfo FormatInfo
		{
			get { return m_FormatInfo; }
			set { m_FormatInfo = value; }
		}
		private CxDSFormatInfo m_FormatInfo = new CxDSFormatInfo();

		#endregion

		#region IxFileAccess の実装:

		/// <summary>
		/// パラメータの読み込み
		/// </summary>
		/// <param name="filename">パラメータファイル(XML)</param>
		/// <param name="options">オプション。(未使用)</param>
		public virtual void Load(string filename, params object[] options)
		{
			object result = DSLab.Axi.ReadAsXml(filename, this.GetType());
			this.CopyFrom(result);
		}

		/// <summary>
		/// パラメータの保存
		/// </summary>
		/// <param name="filename">パラメータファイル(XML)</param>
		/// <param name="options">オプション。(未使用)</param>
		public virtual void Save(string filename, params object[] options)
		{
			DSLab.Axi.WriteAsXml(filename, this);
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// ICloneable の実装: インスタンスのクローンの生成
		/// </summary>
		/// <returns>
		///		自身のクローンを返します。
		/// </returns>
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// ICloneable の実装: インスタンスのクローンの生成
		/// </summary>
		/// <returns>
		///		自身のクローンを返します。
		/// </returns>
		public virtual CxDSCameraParam Clone()
		{
			var clone = new CxDSCameraParam();
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// IxEquatable の実装: インスタンスの複製
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		複製後の自身への参照を返します。
		/// </returns>
		object IxEquatable.CopyFrom(object src)
		{
			return this.CopyFrom(src);
		}

		/// <summary>
		/// IxEquatable の実装: インスタンスの複製
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		複製後の自身への参照を返します。
		/// </returns>
		public virtual CxDSCameraParam CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return this;

			var _src = (CxDSCameraParam)src;

			this.FilterInfo = _src.FilterInfo.Clone();
			this.PinInfo = _src.PinInfo.Clone();
			this.FormatInfo = _src.FormatInfo.Clone();

			return this;
		}

		/// <summary>
		/// IxEquatable の比較: インスタンスの比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true を返します。
		///		そうでなければ false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			try
			{
				var _src = (CxDSCameraParam)src;
				if (ReferenceEquals(_src, null)) return false;

				if (this.FilterInfo != _src.FilterInfo) return false;
				if (this.PinInfo != _src.PinInfo) return false;
				if (this.FormatInfo != _src.FormatInfo) return false;

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		#endregion

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
				if (destinationType == typeof(CxDSCameraParam))
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
