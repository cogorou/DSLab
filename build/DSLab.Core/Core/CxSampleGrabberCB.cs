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
	/// �T���v���O���o�[�R�[���o�b�N
	/// </summary>
	public class CxSampleGrabberCB : System.Object
		, ISampleGrabberCB
	{
		#region �R���X�g���N�^:

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public CxSampleGrabberCB()
		{
		}

		#endregion

		#region ISampleGrabberCB �̎���:

		/// <summary>
		/// �ʒm�C�x���g  
		/// </summary>
		public virtual event CxSampleGrabberEventHandler Notify;

		/// <summary>
		/// �t���[���L���v�`���������ɃC�x���g�𔭍s���邩�ۂ�
		/// </summary>
		public bool Enable = true;

		/// <summary>
		/// �t���[���L���v�`���������ɌĂяo�����R�[���o�b�N�֐�
		/// </summary>
		/// <param name="sample_time">�T���v���^�C��</param>
		/// <param name="sample_data">�T���v���f�[�^</param>
		/// <returns>
		///		DSLab.HRESULT.S_OK ��Ԃ��܂��B
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
		/// �t���[���L���v�`���������ɌĂяo�����R�[���o�b�N�֐�
		/// </summary>
		/// <param name="sample_time">�T���v���^�C��</param>
		/// <param name="addr">�T���v���f�[�^�̐擪�A�h���X</param>
		/// <param name="length">�T���v���f�[�^�� (bytes)</param>
		/// <returns>
		///		DSLab.HRESULT.S_OK ��Ԃ��܂��B
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
	/// �T���v���O���o�[�C�x���g�f���Q�[�g
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param> 
	public delegate void CxSampleGrabberEventHandler(object sender, CxSampleGrabberEventArgs e);

	/// <summary>
	/// �T���v���O���o�[�C�x���g�����N���X
	/// </summary>
	public partial class CxSampleGrabberEventArgs : EventArgs
	{
		#region �R���X�g���N�^:

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public CxSampleGrabberEventArgs()
		{
		}

		/// <summary>
		/// �R���X�g���N�^ (�����l�w��)
		/// </summary>
		/// <param name="sample_time">�T���v���^�C��</param>
		/// <param name="addr">�T���v���f�[�^�̐擪�A�h���X</param>
		/// <param name="length">�T���v���f�[�^�� (bytes)</param>
		public CxSampleGrabberEventArgs(double sample_time, IntPtr addr, int length)
		{
			SampleTime = sample_time;
			Address = addr;
			Length = length;
		}

		/// <summary>
		/// �R���X�g���N�^ (�����l�w��)
		/// </summary>
		/// <param name="sample_time">�T���v���^�C��</param>
		/// <param name="sample_data">�T���v���f�[�^</param>
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

		#region �v���p�e�B:

		/// <summary>
		/// �T���v���^�C��
		/// </summary>
		[Description("P:DSLab.CxSampleGrabberEventArgs.SampleTime")]
		public virtual double SampleTime
		{
			get { return m_SampleTime; }
			set { m_SampleTime = value; }
		}
		private double m_SampleTime = 0;

		/// <summary>
		/// �T���v���f�[�^ (SampleCB �g�p���̂ݗL���ABufferCB �g�p���� null)
		/// </summary>
		[Description("P:DSLab.CxSampleGrabberEventArgs.Sample")]
		public virtual IMediaSample SampleData
		{
			get { return m_SampleData; }
			set { m_SampleData = value; }
		}
		private IMediaSample m_SampleData = null;

		/// <summary>
		/// �T���v���f�[�^�̐擪�A�h���X
		/// </summary>
		[Description("P:DSLab.CxSampleGrabberEventArgs.Address")]
		public virtual IntPtr Address
		{
			get { return m_Address; }
			set { m_Address = value; }
		}
		private IntPtr m_Address = IntPtr.Zero;

		/// <summary>
		/// �T���v���f�[�^��
		/// </summary>
		[Description("P:DSLab.CxSampleGrabberEventArgs.Length")]
		public virtual int Length
		{
			get { return m_Length; }
			set { m_Length = value; }
		}
		private int m_Length = 0;

		/// <summary>
		/// �L�����Z���v�� [false:�p���Atrue:���f]
		/// </summary>
		[Description("P:DSLab.CxSampleGrabberEventArgs.Cancellation")]
		public virtual bool Cancellation
		{
			get { return m_Cancellation; }
			set { m_Cancellation = value; }
		}
		private bool m_Cancellation = false;

		#endregion

		#region ���\�b�h:

		/// <summary>
		/// Bitmap �ւ̕ϊ�
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

			#region �f�[�^����:
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
