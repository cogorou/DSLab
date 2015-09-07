/*
	DSLab
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DSLab
{
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
}
