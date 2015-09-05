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
using System.Drawing.Imaging;
using System.Reflection;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using DSLab;

namespace DSLab
{
	/// <summary>
	/// サンプルグラバーコールバック
	/// </summary>
	public class CxSampleGrabberCB : System.Object
		, ISampleGrabberCB
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxSampleGrabberCB()
		{
		}

		#endregion

		#region ISampleGrabberCB の実装:

		/// <summary>
		/// 通知イベント  
		/// </summary>
		public virtual event CxSampleGrabberEventHandler Notify;

		/// <summary>
		/// フレームキャプチャ完了時にイベントを発行するか否か
		/// </summary>
		public bool Enable = true;

		/// <summary>
		/// フレームキャプチャ完了時に呼び出されるコールバック関数
		/// </summary>
		/// <param name="sample_time">サンプルタイム</param>
		/// <param name="sample_data">サンプルデータ</param>
		/// <returns>
		///		DSLab.HRESULT.S_OK を返します。
		/// </returns>
		int ISampleGrabberCB.SampleCB(double sample_time, IMediaSample sample_data)
		{
			if (this.Enable)
			{
				var args = new CxSampleGrabberEventArgs(sample_time, sample_data);
				if (this.Notify != null)
					this.Notify(this, args);
				if (args.Cancellation)
					return (int)HRESULT.S_FALSE;
			}
			return (int)HRESULT.S_OK;
		}

		/// <summary>
		/// フレームキャプチャ完了時に呼び出されるコールバック関数
		/// </summary>
		/// <param name="sample_time">サンプルタイム</param>
		/// <param name="addr">サンプルデータの先頭アドレス</param>
		/// <param name="length">サンプルデータ長 (bytes)</param>
		/// <returns>
		///		DSLab.HRESULT.S_OK を返します。
		/// </returns>
		int ISampleGrabberCB.BufferCB(double sample_time, IntPtr addr, int length)
		{
			if (this.Enable)
			{
				var args = new CxSampleGrabberEventArgs(sample_time, addr, length);
				if (this.Notify != null)
					this.Notify(this, args);
				if (args.Cancellation)
					return (int)HRESULT.S_FALSE;
			}
			return (int)HRESULT.S_OK;
		}

		#endregion
	}

	/// <summary>
	/// サンプルグラバーイベントデリゲート
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param> 
	public delegate void CxSampleGrabberEventHandler(object sender, CxSampleGrabberEventArgs e);

	/// <summary>
	/// サンプルグラバーイベント引数クラス
	/// </summary>
	public partial class CxSampleGrabberEventArgs : EventArgs
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxSampleGrabberEventArgs()
		{
		}

		/// <summary>
		/// コンストラクタ (初期値指定)
		/// </summary>
		/// <param name="sample_time">サンプルタイム</param>
		/// <param name="addr">サンプルデータの先頭アドレス</param>
		/// <param name="length">サンプルデータ長 (bytes)</param>
		public CxSampleGrabberEventArgs(double sample_time, IntPtr addr, int length)
		{
			SampleTime = sample_time;
			Address = addr;
			Length = length;
		}

		/// <summary>
		/// コンストラクタ (初期値指定)
		/// </summary>
		/// <param name="sample_time">サンプルタイム</param>
		/// <param name="sample_data">サンプルデータ</param>
		public CxSampleGrabberEventArgs(double sample_time, IMediaSample sample_data)
		{
			SampleTime = sample_time;
			SampleData = sample_data;
			if (sample_data != null)
			{
				sample_data.GetPointer(ref m_Address);
				m_Length = sample_data.GetSize();
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// サンプルタイム
		/// </summary>
		[Description("P:DSLab.CxSampleGrabberEventArgs.SampleTime")]
		public virtual double SampleTime
		{
			get { return m_SampleTime; }
			set { m_SampleTime = value; }
		}
		private double m_SampleTime = 0;

		/// <summary>
		/// サンプルデータ (SampleCB 使用時のみ有効、BufferCB 使用時は null)
		/// </summary>
		[Description("P:DSLab.CxSampleGrabberEventArgs.Sample")]
		public virtual IMediaSample SampleData
		{
			get { return m_SampleData; }
			set { m_SampleData = value; }
		}
		private IMediaSample m_SampleData = null;

		/// <summary>
		/// サンプルデータの先頭アドレス
		/// </summary>
		[Description("P:DSLab.CxSampleGrabberEventArgs.Address")]
		public virtual IntPtr Address
		{
			get { return m_Address; }
			set { m_Address = value; }
		}
		private IntPtr m_Address = IntPtr.Zero;

		/// <summary>
		/// サンプルデータ長
		/// </summary>
		[Description("P:DSLab.CxSampleGrabberEventArgs.Length")]
		public virtual int Length
		{
			get { return m_Length; }
			set { m_Length = value; }
		}
		private int m_Length = 0;

		/// <summary>
		/// キャンセル要求 [false:継続、true:中断]
		/// </summary>
		[Description("P:DSLab.CxSampleGrabberEventArgs.Cancellation")]
		public virtual bool Cancellation
		{
			get { return m_Cancellation; }
			set { m_Cancellation = value; }
		}
		private bool m_Cancellation = false;

		#endregion

		#region メソッド:

		/// <summary>
		/// Bitmap への変換
		/// </summary>
		/// <param name="vih"></param>
		/// <returns>
		/// </returns>
		public System.Drawing.Bitmap ToImage(VIDEOINFOHEADER vih)
		{
			if (this.Address == IntPtr.Zero)
				throw new System.InvalidOperationException("Address is Zero.");

			IntPtr address = this.Address;
			int width = vih.bmiHeader.biWidth;
			int height = System.Math.Abs(vih.bmiHeader.biHeight);
			int bpp = vih.bmiHeader.biBitCount;
			int stride = ((width * (bpp / 8) + 3) / 4) * 4;
			if (0 < vih.bmiHeader.biHeight)
			{
				address = new IntPtr(address.ToInt64() + ((height - 1) * stride));
				stride = stride * -1;
			}

			PixelFormat format;
			switch (bpp)
			{
				case 24: format = PixelFormat.Format24bppRgb; break;
				case 32: format = PixelFormat.Format32bppRgb; break;
				default:
					throw new System.NotSupportedException("vih.bmiHeader.biBitCount must be one of the following. (24, 32)");
			}

			var dst = new System.Drawing.Bitmap(width, height, format);

			#region データ複製:
			BitmapData bmpData = null;
			try
			{
				bmpData = new BitmapData();
				bmpData.Width = width;
				bmpData.Height = height;
				bmpData.Stride = stride;
				bmpData.PixelFormat = format;
				bmpData.Scan0 = address;

				dst.LockBits(
					new Rectangle(0, 0, dst.Width, dst.Height),
					ImageLockMode.WriteOnly | ImageLockMode.UserInputBuffer,
					dst.PixelFormat,
					bmpData
					);
			}
			finally
			{
				dst.UnlockBits(bmpData);
			}
			#endregion

			return dst;
		}

		#endregion
	}
}
