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
using System.Reflection;
using System.Xml.Serialization;
using System.ComponentModel;

namespace DSLab
{
	/// <summary>
	/// カメラ制御クラス
	/// </summary>
	/// <remarks>
	/// このクラスは、DirectShow の IAMCameraControl インターフェースをラッピングしたものです。
	/// 詳しくは、IAMCameraControl インターフェースの説明をご参照ください。
	/// 現在は、下記のページに記載されています。<br/>
	/// see: http://msdn.microsoft.com/ja-jp/library/cc354791.aspx
	/// </remarks>
	public class CxAMCameraControl
	{
		#region コンストラクタ.

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAMCameraControl()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="graph">フィルタグラフ</param>
		public CxAMCameraControl(IxDSGraphBuilderProvider graph)
		{
			Graph = graph;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="graph">フィルタグラフ</param>
		/// <param name="property">プロパティの種別</param>
		public CxAMCameraControl(IxDSGraphBuilderProvider graph, CameraControlProperty property)
		{
			Graph = graph;
			Property = property;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// フィルタグラフ
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual IxDSGraphBuilderProvider Graph
		{
			get { return m_Graph; }
			set
			{
				m_Graph = value;
				m_Interface = GetInterface(value);
			}
		}
		private IxDSGraphBuilderProvider m_Graph = null;

		/// <summary>
		/// プロパティの種別
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual CameraControlProperty Property
		{
			get { return m_Property; }
			set { m_Property = value; }
		}
		private CameraControlProperty m_Property = CameraControlProperty.Pan;

		/// <summary>
		/// インターフェース
		/// </summary>
		[XmlIgnore]
		[Browsable(false)]
		internal virtual IAMCameraControl Interface
		{
			get { return m_Interface; }
			set { m_Interface = value;  }
		}
		private IAMCameraControl m_Interface = null;

		#endregion

		#region 設定と取得.

		/// <summary>
		/// プロパティの値
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual int Value
		{
			get
			{
				int val = 0;
				int flag = 0;
				int status = Interface.Get(Property, ref val, ref flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
				return val;
			}
			set
			{
				int val = 0;
				int flag = 0;
				int status = Interface.Get(Property, ref val, ref flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);

				status = Interface.Set(Property, value, flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
			}
		}

		/// <summary>
		/// プロパティの自動制御／手動制御を示す値
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual bool IsManual
		{
			get
			{
				int val = 0;
				int flag = 0;
				int status = Interface.Get(Property, ref val, ref flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
				return ((flag & (int)CameraControlFlags.Manual) == (int)CameraControlFlags.Manual);
			}
			set
			{
				int val = this.Value;
				int flag = (value) ? (int)CameraControlFlags.Manual : (int)CameraControlFlags.Auto;
				int status = Interface.Set(Property, val, flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
			}
		}

		#endregion

		#region 設定と取得.(メソッド)

		/// <summary>
		/// プロパティの値の取得
		/// </summary>
		/// <param name="val">プロパティの値</param>
		/// <param name="flag">プロパティの自動制御(0x01)／手動制御(0x02)を示す値</param>
		public virtual void GetValue(ref int val, ref int flag)
		{
			int status = Interface.Get(Property, ref val, ref flag);
			if (status != 0)
				throw new DSLab.CxDSException((HRESULT)status);
		}

		/// <summary>
		/// プロパティの値の設定
		/// </summary>
		/// <param name="val">プロパティの値</param>
		/// <param name="flag">プロパティの自動制御(0x01)／手動制御(0x02)を示す値。相対値を指定する場合は 0x10 を論理和してください。</param>
		public virtual void SetValue(int val, int flag)
		{
			int status = Interface.Set(Property, val, flag);
			if (status != 0)
				throw new DSLab.CxDSException((HRESULT)status);
		}

		/// <summary>
		/// プロパティの値の設定
		/// </summary>
		/// <param name="val">プロパティの値</param>
		/// <param name="is_manual">プロパティの自動制御／手動制御を示す値</param>
		/// <param name="is_relative">プロパティの値が相対値か否かを示す値</param>
		public virtual void SetValue(int val, bool is_manual, bool is_relative)
		{
			int flag = (is_manual) ? (int)CameraControlFlags.Manual : (int)CameraControlFlags.Auto;
			if (is_relative)
				flag |= 0x0010;	// relative;

			int status = Interface.Set(Property, val, flag);
			if (status != 0)
				throw new DSLab.CxDSException((HRESULT)status);
		}

		#endregion

		#region 範囲と既定値の取得.

		/// <summary>
		/// プロパティの最小値
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual int Min
		{
			get
			{
				int min = 0;
				int max = 0;
				int step = 0;
				int def = 0;
				int flag = 0;
				int status = Interface.GetRange(Property, ref min, ref max, ref step, ref def, ref flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
				return min;
			}
		}

		/// <summary>
		/// プロパティの最大値
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual int Max
		{
			get
			{
				int min = 0;
				int max = 0;
				int step = 0;
				int def = 0;
				int flag = 0;
				int status = Interface.GetRange(Property, ref min, ref max, ref step, ref def, ref flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
				return max;
			}
		}

		/// <summary>
		/// プロパティのステップサイズ
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual int Step
		{
			get
			{
				int min = 0;
				int max = 0;
				int step = 0;
				int def = 0;
				int flag = 0;
				int status = Interface.GetRange(Property, ref min, ref max, ref step, ref def, ref flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
				return step;
			}
		}

		/// <summary>
		/// プロパティの既定値
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual int Default
		{
			get
			{
				int min = 0;
				int max = 0;
				int step = 0;
				int def = 0;
				int flag = 0;
				int status = Interface.GetRange(Property, ref min, ref max, ref step, ref def, ref flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
				return def;
			}
		}

		#endregion

		#region 範囲と既定値の取得.(メソッド)

		/// <summary>
		/// 範囲と既定値の取得
		/// </summary>
		/// <param name="min">最小値</param>
		/// <param name="max">最大値</param>
		/// <param name="step">ステップサイズ</param>
		/// <param name="def">既定値</param>
		/// <param name="flag">プロパティの自動制御(0x01)／手動制御(0x02)を示す値</param>
		public virtual void GetRange(ref int min, ref int max, ref int step, ref int def, ref int flag)
		{
			int status = Interface.GetRange(Property, ref min, ref max, ref step, ref def, ref flag);
			if (status != 0)
				throw new DSLab.CxDSException((HRESULT)status);
		}

		#endregion

		#region インターフェースの取得.

		/// <summary>
		/// 指定されたフィルタグラフが IAMCameraControl インターフェースをサポートしているか検査します。
		/// </summary>
		/// <param name="graph">検査対象のフィルタグラフ</param>
		/// <returns>
		///		サポートしている場合は true を返します。
		///		そうでなければ false を返します。
		/// </returns>
		public static bool IsSupported(IxDSGraphBuilderProvider graph)
		{
			return (GetInterface(graph) != null);
		}

		/// <summary>
		/// 指定されたフィルタグラフから IAMCameraControl インターフェースを取得します。
		/// </summary>
		/// <param name="graph"></param>
		/// <returns>
		///		指定されたグラフに含まれるソースフィルタを IAMCameraControl にキャストして返します。
		///		サポートしていない場合は null を返します。
		/// </returns>
		internal static IAMCameraControl GetInterface(IxDSGraphBuilderProvider graph)
		{
			if (graph == null) return null;
			if (graph.GraphBuilder == null) return null;

			IEnumFilters pEnum = null;
			graph.GraphBuilder.EnumFilters(ref pEnum);
			if (pEnum == null) return null;

			while (true)
			{
				IBaseFilter filter = null;
				int fetched = 0;
				int status = pEnum.Next(1, ref filter, ref fetched);
				if (status != 0) break;

				if (filter is IAMCameraControl)
					return (IAMCameraControl)filter;
			}

			return null;
		}

		#endregion
	}
}
