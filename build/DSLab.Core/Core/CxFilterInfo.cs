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
}
