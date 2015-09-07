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
using System.Runtime.InteropServices;

namespace DSLab
{
	/// <summary>
	/// デバイス選択フォーム
	/// </summary>
	public partial class CxDeviceSelectionForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDeviceSelectionForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDeviceSelectionForm(Guid category)
		{
			InitializeComponent();
			this.Category = category;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 列挙するデバイスのカテゴリ
		/// </summary>
		public Guid Category { get; set; }

		/// <summary>
		/// 現在のフィルタ
		/// </summary>
		private IBaseFilter Filter = null;

		/// <summary>
		/// デバイスの一覧
		/// </summary>
		public List<CxFilterInfo> FilterInfos { get; set; }

		/// <summary>
		/// ピンの一覧
		/// </summary>
		public List<CxPinInfo> PinInfos { get; set; }

		/// <summary>
		/// フォーマットの一覧
		/// </summary>
		public List<CxFormatInfo> FormatInfos { get; set; }

		/// <summary>
		/// 選択されたフィルタの指標 [0~]
		/// </summary>
		public int FilterIndex { get; set; }

		/// <summary>
		/// 選択されたピンの指標 [0~]
		/// </summary>
		public int PinIndex { get; set; }

		/// <summary>
		/// 選択されたフォーマットの指標 [0~]
		/// </summary>
		public int FormatIndex { get; set; }

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxDeviceSelectionForm_Load(object sender, EventArgs e)
		{
			// デバイスの列挙.
			this.FilterInfos = Axi.GetFilterList(this.Category);
			foreach (var item in this.FilterInfos)
				comboName.Items.Add(item.Name);

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxDeviceSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();

			Filter_Dispose();
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
		}

		#endregion

		#region コントロールイベント: (OK/Cancel)

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOK_Click(object sender, EventArgs e)
		{
			this.FilterIndex = comboName.SelectedIndex;
			this.PinIndex = comboPin.SelectedIndex;
			this.FormatIndex = comboFormat.SelectedIndex;
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

		#region コントロールイベント: (プロパティ)

		/// <summary>
		/// プロパティボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolProperty_Click(object sender, EventArgs e)
		{
			if (this.Filter == null) return;
			Axi.OpenPropertyDialog(this.Handle, this.Filter, "Property");
		}

		#endregion

		#region コントロールイベント: (コンボボックス)

		/// <summary>
		/// 現在のフィルタを解放します.
		/// </summary>
		private void Filter_Dispose()
		{
			if (this.Filter != null)
				Marshal.ReleaseComObject(this.Filter);
			this.Filter = null;
		}

		/// <summary>
		/// デバイス名称リストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboName_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				PinInfos = null;
				FormatInfos = null;
				comboPin.Items.Clear();
				comboFormat.Items.Clear();

				Filter_Dispose();

				// フィルタ生成.
				var index = comboName.SelectedIndex;
				var filterInfo = this.FilterInfos[index];
				this.Filter = Axi.CreateFilter(this.Category, filterInfo.CLSID, filterInfo.Index);

				// ピンの列挙.
				var pinInfos = Axi.GetPinList(this.Filter);
				
				// 出力ピンの情報のみ抽出します.
				this.PinInfos = pinInfos.FindAll(
					delegate(CxPinInfo item)
					{
						return (item.Direction == PIN_DIRECTION.PINDIR_OUTPUT);
					});
				foreach (var item in this.PinInfos)
					comboPin.Items.Add(item.Name);
				if (comboPin.Items.Count > 0)
					comboPin.SelectedIndex = 0;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// ピンリストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboPin_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				FormatInfos = null;
				comboFormat.Items.Clear();

				IPin pin = null;
				try
				{
					var index = comboPin.SelectedIndex;
					pin = Axi.FindPin(this.Filter, index, PIN_DIRECTION.PINDIR_OUTPUT);

					// フォーマットの列挙.
					var formatInfos = Axi.GetFormatList(pin);

					// 映像の情報のみ抽出します.
					this.FormatInfos = formatInfos.FindAll(
						delegate(CxFormatInfo item)
						{
							return (GUID.Compare(item.FormatType, GUID.FORMAT_VideoInfo));
						});
					foreach (var item in this.FormatInfos)
					{
						var caption = string.Format("{0} x {1}",
							item.VideoSize.Width,
							item.VideoSize.Height);
						comboFormat.Items.Add(caption);
					}
					if (comboFormat.Items.Count > 0)
						comboFormat.SelectedIndex = 0;
				}
				finally
				{
					if (pin != null)
						Marshal.ReleaseComObject(pin);
					pin = null;
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// フォーマットリストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		#endregion
	}
}
