/*
	DSLab
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DSLab
{
	#region 型定義:
	using FilterPair = KeyValuePair<
			CxDSFilterInfo,
			List<
				KeyValuePair<
					CxDSPinInfo,
					List<KeyValuePair<string, List<CxDSFormatInfo>>>
				>
			>>;
	using PinPair = KeyValuePair<
			CxDSPinInfo,
			List<
				KeyValuePair<string, List<CxDSFormatInfo>>
			>>;
	using FormatPair = KeyValuePair<string, List<CxDSFormatInfo>>;
	#endregion

	/// <summary>
	/// カメラ選択フォーム
	/// </summary>
	public partial class CxDSCameraSelectionForm : Form
	{
		#region 初期化と解放:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDSCameraSelectionForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxDSSelectionForm_Load(object sender, EventArgs e)
		{
			if (this.Param == null)
				this.Param = new CxDSCameraParam();
			this.Backup.CopyFrom(this.Param);

			this.propertyParam.SelectedObject = this.Backup;
			this.Grabber.Param = this.Backup;

			InitializeFilterPairs();

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxDSSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();
			this.Grabber.Stop();
			this.Grabber.GetState(5000);
			this.Grabber.Dispose();
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// グラバーパラメータ
		/// </summary>
		public CxDSCameraParam Param
		{
			get { return m_Param; }
			set { m_Param = value; }
		}
		private CxDSCameraParam m_Param = new CxDSCameraParam();

		/// <summary>
		/// グラバーパラメータ (バックアップ)
		/// </summary>
		private CxDSCameraParam Backup = new CxDSCameraParam();
		#endregion

		#region コントロールイベント: (OK/Cancel)

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOK_Click(object sender, EventArgs e)
		{
			if (this.Param == null)
				this.Param = new CxDSCameraParam();
			this.Param.CopyFrom(this.Backup);

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Cancel ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		#endregion

		#region コントロールイベント: (ファイル保存)

		/// <summary>
		/// ファイル保存
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolSave_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = "XML files (*.xml)|*.xml";
			dlg.Filter += "|All files (*.*)|*.*";
			dlg.DefaultExt = "xml";
			dlg.AddExtension = true;
			dlg.CheckFileExists = false;
			dlg.CheckPathExists = true;
			dlg.OverwritePrompt = true;
			dlg.FileName = string.Format("{0} {1} {2}x{3}.xml",
				this.Backup.FilterInfo.Name,
				this.Backup.FilterInfo.Index,
				this.Backup.FormatInfo.VideoSize.Width,
				this.Backup.FormatInfo.VideoSize.Height
				);
			if (string.IsNullOrEmpty(this.SaveDirectory))
				dlg.InitialDirectory = this.SaveDirectory;
			if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				this.SaveDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
				this.Backup.Save(dlg.FileName);
			}
		}
		private string SaveDirectory = "";

		#endregion

		#region コントロールイベント: (開始/停止)

		/// <summary>
		/// 開始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStart_Click(object sender, EventArgs e)
		{
			try
			{
				this.Grabber.Start();
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 一時停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPause_Click(object sender, EventArgs e)
		{
			try
			{
				this.Grabber.IsPaused = !this.Grabber.IsPaused;
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStop_Click(object sender, EventArgs e)
		{
			try
			{
				this.Grabber.Stop();
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
		}

		#endregion

		#region 定期的な表示更新処理:

		/// <summary>
		/// 定期的な表示更新処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			#region ツールバーの表示更新.
			toolStart.Enabled = (this.Grabber.IsValid && (!this.Grabber.IsRunning || this.Grabber.IsPaused));
			toolPause.Enabled = (this.Grabber.IsRunning);
			toolPause.Checked = (this.Grabber.IsPaused);
			toolStop.Enabled = (this.Grabber.IsRunning);
			#endregion

			#region 画像表示:
			{
				TxFrameIndex frame_index = this.Grabber.FrameIndex();
				if (PrevFrame.TimeStamp != frame_index.TimeStamp)
				{
					DateTime dt01 = TxDateTime.FromBinary(PrevFrame.TimeStamp, true);
					DateTime dt02 = TxDateTime.FromBinary(frame_index.TimeStamp, true);
					this.TimeStamps.Add(dt02);

					#region time stamp
					this.statusTimeStamp.Text = string.Format(
						"{0:00}:{1:00}:{2:00}.{3:000}",
						dt02.Hour, dt02.Minute, dt02.Second, dt02.Millisecond
						);
					#endregion

					#region frame rate (fps)
					if ((this.FpsTime - dt02).Seconds < -1)
					{
						this.FpsTime = dt02;

						lock (this.TimeStamps)
						{
							if (this.TimeStamps.Count > 1)
							{
								int length = this.TimeStamps.Count - 1;
								TimeSpan sum = new TimeSpan();
								for (int i = 0; i < length; i++)
								{
									sum += (this.TimeStamps[i + 1] - this.TimeStamps[i]);
								}
								double fps = (sum.TotalSeconds == 0)
									? 0.0
									: length / sum.TotalSeconds;

								this.statusFps.Text = string.Format("{0:0.00} fps", fps);
							}
						}
					}
					#endregion

					if (frame_index.Frame < this.Buffer.Length)
					{
						var image = this.Buffer[frame_index.Frame];

						#region image
						if (image.IsValid)
						{
							Bitmap bitmap = image.ToBitmap();
							if (pictureView.Image != null)
								pictureView.Image.Dispose();
							pictureView.Image = bitmap;

							Size size1 = pictureView.Size;
							Size size2 = pictureView.Image.Size;
							if (size1.Width < size2.Width ||
								size1.Height < size2.Height)
								pictureView.SizeMode = PictureBoxSizeMode.Zoom;
							else
								pictureView.SizeMode = PictureBoxSizeMode.CenterImage;

							pictureView.Refresh();
						}
						#endregion
					}

					PrevFrame = frame_index;
				}
			}
			#endregion
		}
		private TxFrameIndex PrevFrame = new TxFrameIndex();
		private DateTime PrevTime = DateTime.Now;
		private DateTime FpsTime = DateTime.Now;

		#endregion

		#region コントロールイベント: (画像ビュー)

		/// <summary>
		/// 画像ビュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureView_Paint(object sender, PaintEventArgs e)
		{
			if (pictureView.Image != null)
			{
				double mag = 100;
				if (pictureView.SizeMode == PictureBoxSizeMode.Zoom)
				{
					Size size1 = pictureView.Size;
					Size size2 = pictureView.Image.Size;
					double magx = (double)size1.Width / size2.Width;
					double magy = (double)size1.Height / size2.Height;
					mag = System.Math.Min(magx, magy) * 100;
				}
				statusMag.Text = string.Format("{0:0.00} %", mag);
			}
		}

		#endregion

		#region フィルタ情報:

		/// <summary>
		/// フィルタ情報
		/// </summary>
		private List<FilterPair> FilterPairs = new List<FilterPair>();

		/// <summary>
		/// フィルタ情報の初期化
		/// </summary>
		void InitializeFilterPairs()
		{
			// [1] フィルタとピンのペア.
			List<FilterPair> filter_pairs = FilterPairs;

			// カテゴリ: GUID.CLSID_VideoInputDeviceCategory
			string category = GUID.CLSID_VideoInputDeviceCategory;

			// フィルタの一覧を取得する.
			List<CxDSFilterInfo> filters = Axi.GetFilterList(category);
			foreach (var filter in filters)
			{
				IBaseFilter capture = null;

				try
				{
					#region フィルタを生成する.
					capture = Axi.CreateFilter(category, filter.Name, filter.Index);
					#endregion

					#region ピンの一覧を取得する.
					List<CxDSPinInfo> pins = new List<CxDSPinInfo>();
					{
						List<CxDSPinInfo> items = Axi.GetPinList(capture);
						// 出力ピンのみ抽出する.
						foreach (var item in items)
						{
							if (item.Direction == PIN_DIRECTION.PINDIR_OUTPUT)
								pins.Add(item);
						}
					}
					#endregion

					#region フォーマットの一覧を取得する.
					try
					{
						// [2] ピンとフォーマットのペア.
						List<PinPair> pin_pairs = new List<PinPair>();

						for (int i = 0; i < pins.Count; i++)
						{
							CxDSPinInfo pin = pins[i];

							// 出力ピンを探す.
							IPin outpin = Axi.FindPin(capture, i, PIN_DIRECTION.PINDIR_OUTPUT);

							// フォーマットの色空間グループ分け.
							Dictionary<string, List<CxDSFormatInfo>> groups = new Dictionary<string, List<CxDSFormatInfo>>();

							// フォーマットの一覧を取得する.
							List<CxDSFormatInfo> formats = Axi.GetFormatList(outpin);
							foreach (var format in formats)
							{
								if (GUID.Compare(format.FormatType, GUID.FORMAT_VideoInfo) == false) continue;

								List<CxDSFormatInfo> groups_value = null;
								if (groups.TryGetValue(format.MediaSubType, out groups_value) == false)
								{
									groups_value = new List<CxDSFormatInfo>();
									groups[format.MediaSubType] = (groups_value);
								}
								groups_value.Add(format);
							}

							// [3] 色空間とフォーマットのペア.
							List<FormatPair> format_pairs = new List<FormatPair>();

							// フォーマットを色空間グループ毎に列挙する.
							foreach (var group in groups)
							{
								// 色空間のニックネーム.
								string nickname = GUID.GetNickname(group.Key);	// Key=MediaSubType
								if (string.IsNullOrEmpty(nickname))
									nickname = "(unknown)";

								// [3] 色空間とフォーマットのペア.
								format_pairs.Add(new FormatPair(nickname, group.Value));
							}

							// [2] ピンとフォーマットのペア.
							pin_pairs.Add(new PinPair(pin, format_pairs));
						}

						// [1] フィルタとピンのペア.
						filter_pairs.Add(new FilterPair(filter, pin_pairs));
					}
					catch (System.Exception ex)
					{
						Debug.WriteLine(ex.StackTrace);
					}
					finally
					{
						Axi.ReleaseInstance(capture);
					}
					#endregion
				}
				catch (System.Exception ex)
				{
					Debug.WriteLine(ex.StackTrace);
				}
				finally
				{
					#region フィルタを解放する.
					Axi.ReleaseInstance(capture);
					#endregion
				}
			}

			int index = -1;

			// コンボボックスへの追加.
			comboFilter.Items.Clear();
			for(int i=0 ; i<filter_pairs.Count ; i++)
			{
				var filter_pair = filter_pairs[i];
				comboFilter.Items.Add(filter_pair.Key.Name);

				if (filter_pair.Key.ContentEquals(this.Backup.FilterInfo))
					index = i;
			}

			if (0 <= index && index < comboFilter.Items.Count)
			    comboFilter.SelectedIndex = index;
		}
		#endregion

		#region コントロールイベント: (コンボボックス)

		/// <summary>
		/// フィルタリストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
			FilterPair filter_pair = FilterPairs[comboFilter.SelectedIndex];

			comboPin.Items.Clear();
			foreach (var pin_pair in filter_pair.Value)
				comboPin.Items.Add(pin_pair.Key.Name);
			int index = 0;
			if (comboPin.Items.Count > index)
				comboPin.SelectedIndex = index;
		}

		/// <summary>
		/// ピンリストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboPin_SelectedIndexChanged(object sender, EventArgs e)
		{
			FilterPair filter_pair = FilterPairs[comboFilter.SelectedIndex];
			PinPair pin_pair = filter_pair.Value[comboPin.SelectedIndex];

			comboFormatColor.Items.Clear();
			foreach (var format_pair in pin_pair.Value)
				comboFormatColor.Items.Add(format_pair.Key);
			int index = 0;
			if (comboFormatColor.Items.Count > index)
				comboFormatColor.SelectedIndex = index;
		}

		/// <summary>
		/// フォーマット(色空間)リストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboFormatColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			FilterPair filter_pair = FilterPairs[comboFilter.SelectedIndex];
			PinPair pin_pair = filter_pair.Value[comboPin.SelectedIndex];
			FormatPair format_pair = pin_pair.Value[comboFormatColor.SelectedIndex];

			comboFormatSize.Items.Clear();
			foreach (var format in format_pair.Value)
				comboFormatSize.Items.Add(string.Format("{0} x {1}", format.VideoSize.Width, format.VideoSize.Height));
			int index = 0;
			if (comboFormatSize.Items.Count > index)
				comboFormatSize.SelectedIndex = index;
		}

		/// <summary>
		/// フォーマット(サイズ)リストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboFormatSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				this.Grabber.Stop();
			}
			catch (Exception)
			{
			}
			this.Grabber.Dispose();
			lock (this.TimeStamps)
			{
				this.TimeStamps.Clear();
			}
			this.statusTimeStamp.Text = "";
			this.statusFps.Text = "";

			FilterPair filter_pair = FilterPairs[comboFilter.SelectedIndex];
			PinPair pin_pair = filter_pair.Value[comboPin.SelectedIndex];
			FormatPair format_pair = pin_pair.Value[comboFormatColor.SelectedIndex];
			CxDSFormatInfo format_info = format_pair.Value[comboFormatSize.SelectedIndex];

			this.Backup.FilterInfo = filter_pair.Key;
			this.Backup.PinInfo = pin_pair.Key;
			this.Backup.FormatInfo = format_info;
			this.propertyParam.Refresh();

			this.Grabber.Setup();
			foreach (var image in this.Buffer)
			{
				image.Resize(this.Grabber.FrameSize.Width, this.Grabber.FrameSize.Height, TxModel.U8(4), 1);
			}
			this.Grabber.Capture(this.Buffer, true);
			this.Grabber.Start();
		}

		#endregion

		#region コントロールイベント: (プロパティ)

		/// <summary>
		/// フィルタプロパティ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonFilter_Click(object sender, EventArgs e)
		{
			this.Grabber.OpenPropertyDialog(this, "Capture", PropertyDialogType.Capture);
		}

		/// <summary>
		/// ピンプロパティ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonPin_Click(object sender, EventArgs e)
		{
			this.Grabber.OpenPropertyDialog(this, "Outpin", PropertyDialogType.Outpin);
		}

		#endregion

		#region 画像取り込み:
		/// <summary>
		/// グラバー
		/// </summary>
		private CxDSCamera Grabber = new CxDSCamera();

		/// <summary>
		/// 取り込み用バッファ
		/// </summary>
		private CxImage[] Buffer = new CxImage[]
		{
			new CxImage(),
			new CxImage(),
			new CxImage(),
		};

		/// <summary>
		/// タイムスタンプのコレクション
		/// </summary>
		private List<DateTime> TimeStamps = new List<DateTime>();
		#endregion
	}
}
