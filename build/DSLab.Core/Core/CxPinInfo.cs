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
}
