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
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using DSLab;

namespace demo
{
	public class Util
	{
		#region GetFilterList

		/// <summary>
		/// �t�B���^�ꗗ�̎擾
		/// </summary>
		/// <param name="category">�f�o�C�X�̃J�e�S��</param>
		/// <returns>
		///		�擾�����t�B���^���̃R���N�V������Ԃ��܂��B
		/// </returns>
		public static List<CxFilterInfo> GetFilterList(Guid category)
		{
			var result = new List<CxFilterInfo>();
			System.Runtime.InteropServices.ComTypes.IEnumMoniker enumerator = null;
			ICreateDevEnum device = null;

			try
			{
				// ICreateDevEnum �C���^�[�t�F�[�X�擾.
				device = (ICreateDevEnum)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(GUID.CLSID_SystemDeviceEnum)));

				// EnumMoniker�̍쐬.
				device.CreateClassEnumerator(ref category, ref enumerator, 0);
				if (enumerator == null)
					return result;

				// ��.
				var monikers = new System.Runtime.InteropServices.ComTypes.IMoniker[1];
				var fetched = IntPtr.Zero;

				while (enumerator.Next(monikers.Length, monikers, fetched) == 0)
				{
					// �v���p�e�B�o�b�O�ւ̃o�C���h.
					IPropertyBag propbag = null;
					{
						object tmp = null;
						Guid guid = new Guid(GUID.IID_IPropertyBag);
						monikers[0].BindToStorage(null, null, ref guid, out tmp);
						propbag = (IPropertyBag)tmp;
					}

					try
					{
						var info = new CxFilterInfo();

						// ���O�擾.
						try
						{
							object friendly_name = null;
							propbag.Read("FriendlyName", ref friendly_name, 0);
							info.Name = (string)friendly_name;
						}
						catch (Exception)
						{
						}

						// CLSID�擾.
						try
						{
							object clsid = null;
							propbag.Read("CLSID", ref clsid, 0);
							info.CLSID = (string)clsid;
						}
						catch (Exception)
						{
						}

						// �R���N�V�����ɒǉ�.
						result.Add(info);
					}
					finally
					{
						// �v���p�e�B�o�b�O�̉��.
						Marshal.ReleaseComObject(propbag);

						// �񋓂������j�J�̉��.
						for (int mmm = 0; mmm < monikers.Length; mmm++)
						{
							if (monikers[mmm] != null)
								Marshal.ReleaseComObject(monikers[mmm]);
							monikers[mmm] = null;
						}
					}
				}

				// �����t�B���^�̏��ԕt��.
				for (int i = 0; i < result.Count - 1; i++)
				{
					for (int j = i + 1; j < result.Count; j++)
					{
						if (result[j].Name == result[i].Name)
						{
							result[j].Index = result[i].Index + 1;
							break;
						}
					}
				}
			}
			finally
			{
				if (enumerator != null)
					Marshal.ReleaseComObject(enumerator);
				if (device != null)
					Marshal.ReleaseComObject(device);
			}

			return result;
		}

		#endregion

		#region GetPinList

		/// <summary>
		/// �s���ꗗ�̎擾
		/// </summary>
		/// <param name="filter">�Ώۂ̃t�B���^</param>
		/// <returns>
		///		�擾�����s�����̃R���N�V������Ԃ��܂��B
		/// </returns>
		public static List<CxPinInfo> GetPinList(IBaseFilter filter)
		{
			var result = new List<CxPinInfo>();
			IEnumPins enumpins = null;

			try
			{
				filter.EnumPins(ref enumpins);

				while (true)
				{
					IPin pin = null;
					int fetched = 0;
					if (enumpins.Next(1, ref pin, ref fetched) < 0) break;
					if (fetched == 0) break;

					var info = new PIN_INFO();

					try
					{
						pin.QueryPinInfo(info);
						var dpi = new CxPinInfo(info.achName, info.dir);
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
			}

			return result;
		}

		#endregion

		#region GetFormatList

		/// <summary>
		/// �t�H�[�}�b�g�ꗗ�̎擾
		/// </summary>
		/// <param name="pin">�Ώۂ̃s��</param>
		/// <returns>
		///		�擾�����t�H�[�}�b�g���̃R���N�V������Ԃ��܂��B
		/// </returns>
		public static List<CxFormatInfo> GetFormatList(IPin pin)
		{
			var result = new List<CxFormatInfo>();
			if (pin == null)
				return result;

			var config = pin as IAMStreamConfig;
			if (config == null)
				return result;

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

						// ��{���̎擾.
						var info = new CxFormatInfo();
						info.MediaType = GUID.Normalize(mt.majortype.ToString());
						info.MediaSubType = GUID.Normalize(mt.subtype.ToString());
						info.FormatType = GUID.Normalize(mt.formattype.ToString());

						// �f���`�����ۂ�.
						if (GUID.Compare(info.FormatType, GUID.FORMAT_VideoInfo))
						{
							var vih = new VIDEOINFOHEADER();
							vih = (VIDEOINFOHEADER)Marshal.PtrToStructure(mt.pbFormat, typeof(VIDEOINFOHEADER));
							info.VideoSize = new Size(vih.bmiHeader.biWidth, vih.bmiHeader.biHeight);
						}

						// �R���N�V�����ɒǉ�.
						result.Add(info);
					}
					finally
					{
						if (mt != null)
							Axi.FreeMediaType(ref mt);
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

		#region GetInterface

		/// <summary>
		/// �C�ӂ̃C���^�[�t�F�[�X�̎擾
		/// </summary>
		/// <param name="graph">�Ώۂ̃O���t</param>
		/// <returns>
		///		�擾�����C���^�[�t�F�[�X��Ԃ��܂��B
		/// </returns>
		public static TI GetInterface<TI>(IGraphBuilder graph)
		{
			if (graph == null) return default(TI);

			IEnumFilters pEnum = null;
			graph.EnumFilters(ref pEnum);
			if (pEnum == null) return default(TI);

			while (true)
			{
				IBaseFilter filter = null;
				int fetched = 0;
				int status = pEnum.Next(1, ref filter, ref fetched);
				if (status != 0) break;

				if (filter is TI)
					return (TI)filter;
			}

			return default(TI);
		}

		#endregion
	}

	#region CxFilterInfo

	/// <summary>
	/// �t�B���^���
	/// </summary>
	public class CxFilterInfo
	{
		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public CxFilterInfo()
		{
			Name = "";
			CLSID = "";
			Index = 0;
		}

		/// <summary>
		/// ����
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// CLSID
		/// </summary>
		public virtual string CLSID { get; set; }

		/// <summary>
		/// �w�W [0~] �������̃t�B���^����ʂ���ׂ̎w�W�ł��B
		/// </summary>
		public virtual int Index { get; set; }
	}

	#endregion

	#region CxPinInfo

	/// <summary>
	/// �s�����
	/// </summary>
	public class CxPinInfo
	{
		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public CxPinInfo()
		{
			Name = "";
			Direction = PIN_DIRECTION.PINDIR_OUTPUT;
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="name">����</param>
		/// <param name="dir">����</param>
		public CxPinInfo(string name, PIN_DIRECTION dir)
		{
			Name = name;
			Direction = dir;
		}

		/// <summary>
		/// ����
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// ����
		/// </summary>
		public virtual PIN_DIRECTION Direction { get; set; }
	}

	#endregion

	#region CxFormatInfo

	/// <summary>
	/// �t�H�[�}�b�g���
	/// </summary>
	public class CxFormatInfo
	{
		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public CxFormatInfo()
		{
			MediaType = "";
			MediaSubType = "";
			FormatType = "";
			VideoSize = new Size();
		}

		/// <summary>
		/// ���f�B�A�^�C�v
		/// </summary>
		public virtual string MediaType { get; set; }

		/// <summary>
		/// ���f�B�A�T�u�^�C�v
		/// </summary>
		public virtual string MediaSubType { get; set; }

		/// <summary>
		/// �t�H�[�}�b�g�^�C�v
		/// </summary>
		public virtual string FormatType { get; set; }

		/// <summary>
		/// �r�f�I�T�C�Y
		/// </summary>
		public virtual Size VideoSize { get; set; }
	}

	#endregion
}
