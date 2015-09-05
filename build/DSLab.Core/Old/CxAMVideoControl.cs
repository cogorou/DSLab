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
	/// フリッピング／外部トリガを提供するクラス
	/// </summary>
	/// <remarks>
	/// このクラスは、DirectShow の IAMVideoControl インターフェースをラッピングしたものです。
	/// 詳しくは、IAMVideoControl インターフェースの説明をご参照ください。
	/// 現在は、下記のページに記載されています。<br/>
	/// see: http://msdn.microsoft.com/ja-jp/library/cc355345.aspx
	/// </remarks>
	public class CxAMVideoControl
	{
		#region コンストラクタ.

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAMVideoControl()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="graph">フィルタグラフ</param>
		public CxAMVideoControl(IxDSGraphBuilderProvider graph)
		{
			Graph = graph;
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

				CaptureFilter = null;
				CaptureOutPin = null;
				if (m_Graph != null && m_Graph.GraphBuilder != null)
				{
					m_Graph.GraphBuilder.FindFilterByName("CaptureFilter", ref CaptureFilter);
					if (CaptureFilter != null)
						CaptureOutPin = DSLab.Axi.FindPin(CaptureFilter, 0, PIN_DIRECTION.PINDIR_OUTPUT);
				}
			}
		}
		private IxDSGraphBuilderProvider m_Graph = null;

		/// <summary>
		/// インターフェース
		/// </summary>
		[XmlIgnore]
		[Browsable(false)]
		internal virtual IAMVideoControl Interface
		{
			get { return m_Interface; }
			set { m_Interface = value; }
		}
		private IAMVideoControl m_Interface = null;

		/// <summary>
		/// 画像入力フィルタ
		/// </summary>
		internal IBaseFilter CaptureFilter = null;

		/// <summary>
		/// 画像入力フィルタの出力ピン
		/// </summary>
		internal IPin CaptureOutPin = null;

		#endregion

		#region プロパティ: (設定と取得)

		/// <summary>
		/// 操作のビデオ制御モード
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual VideoControlFlags Mode
		{
			get
			{
				int flag = 0;
				DSLab.CxDSCamera graber = (DSLab.CxDSCamera)Graph;
				int status = Interface.GetMode(graber.CaptureOutPin, out flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
				return (VideoControlFlags)flag;
			}
			set
			{
				int flag = (int)value;
				DSLab.CxDSCamera graber = (DSLab.CxDSCamera)Graph;
				int status = Interface.SetMode(graber.CaptureOutPin, flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
			}
		}

		#endregion

		#region プロパティ: (取得のみ)

		/// <summary>
		/// 使用するハードウェアの能力 (取得のみ)
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual VideoControlFlags Caps
		{
			get
			{
				int flag = 0;
				int status = Interface.GetCaps(this.CaptureOutPin, out flag);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
				return (VideoControlFlags)flag;
			}
		}

		/// <summary>
		/// デバイスがストリーミングしている実際のフレームレート (取得のみ)
		/// </summary>
		[XmlIgnore]
		[Browsable(true)]
		public virtual long CurrentActualFrameRate
		{
			get
			{
				long ActualFrameRate = 0;
				int status = Interface.GetCurrentActualFrameRate(this.CaptureOutPin, out ActualFrameRate);
				if (status != 0)
					throw new DSLab.CxDSException((HRESULT)status);
				return ActualFrameRate;
			}
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 現在利用可能な最大フレームレートを取得する。
		/// </summary>
		/// <param name="index">最大フレーム レートのクエリ対象となるフォーマットのインデックス。このインデックスは、フォーマットが IAMStreamConfig::GetStreamCaps で列挙される順序に対応している。値の範囲は、0 から MStreamConfig::GetNumberOfCapabilities メソッドが返す、サポートされている VIDEO_STREAM_CONFIG_CAPS 構造体の数から 1 を減算した値までである。</param>
		/// <param name="Dimensions">フレーム イメージのピクセル単位のサイズ (幅と高さ)。</param>
		/// <returns>
		///		利用可能な最大フレーム レートへのポインタ。フレーム レートは、100 ナノ秒単位のフレーム時間幅として表される。
		/// </returns>
		public long GetMaxAvailableFrameRate(int index, Size Dimensions)
		{
			long MaxAvailableFrameRate = 0;
			int status = Interface.GetMaxAvailableFrameRate(this.CaptureOutPin, index, Dimensions, out MaxAvailableFrameRate);
			if (status != 0)
				throw new DSLab.CxDSException((HRESULT)status);
			return MaxAvailableFrameRate;
		}

		/// <summary>
		/// 利用可能なフレームレートのリストを取得する。
		/// </summary>
		/// <param name="index">フレーム レートのクエリ対象となるフォーマットのインデックス。このインデックスは、フォーマットが IAMStreamConfig::GetStreamCaps で列挙される順序に対応している。値の範囲は、0 から MStreamConfig::GetNumberOfCapabilities メソッドが返す、サポートされている VIDEO_STREAM_CONFIG_CAPS 構造体の数から 1 を減算した値までである。</param>
		/// <param name="Dimensions">フレーム イメージのピクセル単位のサイズ (幅と高さ)。</param>
		/// <param name="ListSize">フレーム レートのリスト内の要素数へのポインタ。</param>
		/// <param name="FrameRates">100 ナノ秒単位のフレーム レートの配列へのポインタのアドレス。ListSize だけを必要とする場合、このデータは NULL でもよい。</param>
		public void GetFrameRateList(int index, Size Dimensions, out int ListSize, out IntPtr FrameRates)
		{
			int status = Interface.GetFrameRateList(this.CaptureOutPin, index, Dimensions, out ListSize, out FrameRates);
			if (status != 0)
				throw new DSLab.CxDSException((HRESULT)status);
		}

		#endregion

		#region インターフェースの取得.

		/// <summary>
		/// 指定されたフィルタグラフが IAMVideoControl インターフェースをサポートしているか検査します.
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
		/// 指定されたフィルタグラフから IAMVideoControl インターフェースを取得します。
		/// </summary>
		/// <param name="graph">検査対象のフィルタグラフ</param>
		/// <returns>
		///		指定されたグラフに含まれるソースフィルタを IAMVideoControl にキャストして返します。
		///		サポートしていない場合は null を返します。
		/// </returns>
		internal static IAMVideoControl GetInterface(IxDSGraphBuilderProvider graph)
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

				if (filter is IAMVideoControl)
					return (IAMVideoControl)filter;
			}

			return null;
		}

		#endregion
	}
}
