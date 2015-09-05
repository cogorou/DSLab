namespace DSLab
{
	partial class CxDSCameraSelectionForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxDSCameraSelectionForm));
			this.Toolbar = new DSLab.Forms.CxToolStripEx();
			this.toolCancel = new System.Windows.Forms.ToolStripButton();
			this.toolOK = new System.Windows.Forms.ToolStripButton();
			this.toolSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStart = new System.Windows.Forms.ToolStripButton();
			this.toolPause = new System.Windows.Forms.ToolStripButton();
			this.toolStop = new System.Windows.Forms.ToolStripButton();
			this.Statusbar = new System.Windows.Forms.StatusStrip();
			this.statusFps = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusTimeStamp = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusSpacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusMag = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitView = new System.Windows.Forms.SplitContainer();
			this.buttonPin = new System.Windows.Forms.Button();
			this.buttonFilter = new System.Windows.Forms.Button();
			this.comboFormatSize = new System.Windows.Forms.ComboBox();
			this.comboFormatColor = new System.Windows.Forms.ComboBox();
			this.comboPin = new System.Windows.Forms.ComboBox();
			this.comboFilter = new System.Windows.Forms.ComboBox();
			this.labelFormat = new System.Windows.Forms.Label();
			this.labelPin = new System.Windows.Forms.Label();
			this.labelFilter = new System.Windows.Forms.Label();
			this.tabView = new System.Windows.Forms.TabControl();
			this.tabPageView = new System.Windows.Forms.TabPage();
			this.pictureView = new System.Windows.Forms.PictureBox();
			this.tabPageProperty = new System.Windows.Forms.TabPage();
			this.propertyParam = new System.Windows.Forms.PropertyGrid();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.Toolbar.SuspendLayout();
			this.Statusbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitView)).BeginInit();
			this.splitView.Panel1.SuspendLayout();
			this.splitView.Panel2.SuspendLayout();
			this.splitView.SuspendLayout();
			this.tabView.SuspendLayout();
			this.tabPageView.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureView)).BeginInit();
			this.tabPageProperty.SuspendLayout();
			this.SuspendLayout();
			// 
			// Toolbar
			// 
			this.Toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolCancel,
            this.toolOK,
            this.toolSave,
            this.toolStripSeparator1,
            this.toolStart,
            this.toolPause,
            this.toolStop});
			this.Toolbar.Location = new System.Drawing.Point(0, 0);
			this.Toolbar.Name = "Toolbar";
			this.Toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.Toolbar.Size = new System.Drawing.Size(484, 35);
			this.Toolbar.TabIndex = 1;
			this.Toolbar.Text = "toolbar";
			// 
			// toolCancel
			// 
			this.toolCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolCancel.AutoSize = false;
			this.toolCancel.Image = ((System.Drawing.Image)(resources.GetObject("toolCancel.Image")));
			this.toolCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolCancel.Name = "toolCancel";
			this.toolCancel.Size = new System.Drawing.Size(96, 32);
			this.toolCancel.Text = "Cancel";
			this.toolCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
			this.toolCancel.Click += new System.EventHandler(this.toolCancel_Click);
			// 
			// toolOK
			// 
			this.toolOK.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolOK.AutoSize = false;
			this.toolOK.Image = ((System.Drawing.Image)(resources.GetObject("toolOK.Image")));
			this.toolOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolOK.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolOK.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOK.Name = "toolOK";
			this.toolOK.Size = new System.Drawing.Size(96, 32);
			this.toolOK.Text = "OK";
			this.toolOK.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
			this.toolOK.Click += new System.EventHandler(this.toolOK_Click);
			// 
			// toolSave
			// 
			this.toolSave.AutoSize = false;
			this.toolSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolSave.Image = ((System.Drawing.Image)(resources.GetObject("toolSave.Image")));
			this.toolSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolSave.Name = "toolSave";
			this.toolSave.Size = new System.Drawing.Size(32, 32);
			this.toolSave.Text = "Save";
			this.toolSave.Click += new System.EventHandler(this.toolSave_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
			// 
			// toolStart
			// 
			this.toolStart.AutoSize = false;
			this.toolStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStart.Image = ((System.Drawing.Image)(resources.GetObject("toolStart.Image")));
			this.toolStart.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStart.Name = "toolStart";
			this.toolStart.Size = new System.Drawing.Size(32, 32);
			this.toolStart.Text = "Start";
			this.toolStart.Click += new System.EventHandler(this.toolStart_Click);
			// 
			// toolPause
			// 
			this.toolPause.AutoSize = false;
			this.toolPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPause.Image = ((System.Drawing.Image)(resources.GetObject("toolPause.Image")));
			this.toolPause.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPause.Name = "toolPause";
			this.toolPause.Size = new System.Drawing.Size(32, 32);
			this.toolPause.Text = "Pause";
			this.toolPause.Click += new System.EventHandler(this.toolPause_Click);
			// 
			// toolStop
			// 
			this.toolStop.AutoSize = false;
			this.toolStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStop.Image")));
			this.toolStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStop.Name = "toolStop";
			this.toolStop.Size = new System.Drawing.Size(32, 32);
			this.toolStop.Text = "Stop";
			this.toolStop.Click += new System.EventHandler(this.toolStop_Click);
			// 
			// Statusbar
			// 
			this.Statusbar.AutoSize = false;
			this.Statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusFps,
            this.statusTimeStamp,
            this.statusSpacer,
            this.statusMag});
			this.Statusbar.Location = new System.Drawing.Point(0, 447);
			this.Statusbar.Name = "Statusbar";
			this.Statusbar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.Statusbar.Size = new System.Drawing.Size(484, 40);
			this.Statusbar.TabIndex = 2;
			this.Statusbar.Text = "statusbar";
			// 
			// statusFps
			// 
			this.statusFps.AutoSize = false;
			this.statusFps.Name = "statusFps";
			this.statusFps.Size = new System.Drawing.Size(128, 35);
			this.statusFps.Text = "xx.xx fps";
			// 
			// statusTimeStamp
			// 
			this.statusTimeStamp.AutoSize = false;
			this.statusTimeStamp.Name = "statusTimeStamp";
			this.statusTimeStamp.Size = new System.Drawing.Size(128, 35);
			this.statusTimeStamp.Text = "xx:xx:xx.xxx";
			// 
			// statusSpacer
			// 
			this.statusSpacer.Name = "statusSpacer";
			this.statusSpacer.Size = new System.Drawing.Size(115, 35);
			this.statusSpacer.Spring = true;
			// 
			// statusMag
			// 
			this.statusMag.AutoSize = false;
			this.statusMag.Name = "statusMag";
			this.statusMag.Size = new System.Drawing.Size(96, 35);
			this.statusMag.Text = "xxx.xx %";
			// 
			// splitView
			// 
			this.splitView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitView.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitView.IsSplitterFixed = true;
			this.splitView.Location = new System.Drawing.Point(0, 35);
			this.splitView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.splitView.Name = "splitView";
			this.splitView.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitView.Panel1
			// 
			this.splitView.Panel1.Controls.Add(this.buttonPin);
			this.splitView.Panel1.Controls.Add(this.buttonFilter);
			this.splitView.Panel1.Controls.Add(this.comboFormatSize);
			this.splitView.Panel1.Controls.Add(this.comboFormatColor);
			this.splitView.Panel1.Controls.Add(this.comboPin);
			this.splitView.Panel1.Controls.Add(this.comboFilter);
			this.splitView.Panel1.Controls.Add(this.labelFormat);
			this.splitView.Panel1.Controls.Add(this.labelPin);
			this.splitView.Panel1.Controls.Add(this.labelFilter);
			// 
			// splitView.Panel2
			// 
			this.splitView.Panel2.Controls.Add(this.tabView);
			this.splitView.Size = new System.Drawing.Size(484, 412);
			this.splitView.SplitterDistance = 140;
			this.splitView.SplitterWidth = 5;
			this.splitView.TabIndex = 3;
			// 
			// buttonPin
			// 
			this.buttonPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonPin.Image = ((System.Drawing.Image)(resources.GetObject("buttonPin.Image")));
			this.buttonPin.Location = new System.Drawing.Point(433, 51);
			this.buttonPin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonPin.Name = "buttonPin";
			this.buttonPin.Size = new System.Drawing.Size(32, 32);
			this.buttonPin.TabIndex = 8;
			this.buttonPin.UseVisualStyleBackColor = true;
			this.buttonPin.Click += new System.EventHandler(this.buttonPin_Click);
			// 
			// buttonFilter
			// 
			this.buttonFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonFilter.Image = ((System.Drawing.Image)(resources.GetObject("buttonFilter.Image")));
			this.buttonFilter.Location = new System.Drawing.Point(433, 13);
			this.buttonFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonFilter.Name = "buttonFilter";
			this.buttonFilter.Size = new System.Drawing.Size(32, 32);
			this.buttonFilter.TabIndex = 8;
			this.buttonFilter.UseVisualStyleBackColor = true;
			this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
			// 
			// comboFormatSize
			// 
			this.comboFormatSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboFormatSize.BackColor = System.Drawing.SystemColors.Control;
			this.comboFormatSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFormatSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboFormatSize.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboFormatSize.FormattingEnabled = true;
			this.comboFormatSize.Location = new System.Drawing.Point(327, 94);
			this.comboFormatSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.comboFormatSize.Name = "comboFormatSize";
			this.comboFormatSize.Size = new System.Drawing.Size(143, 23);
			this.comboFormatSize.TabIndex = 7;
			this.comboFormatSize.SelectedIndexChanged += new System.EventHandler(this.comboFormatSize_SelectedIndexChanged);
			// 
			// comboFormatColor
			// 
			this.comboFormatColor.BackColor = System.Drawing.SystemColors.Control;
			this.comboFormatColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFormatColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboFormatColor.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboFormatColor.FormattingEnabled = true;
			this.comboFormatColor.Location = new System.Drawing.Point(133, 94);
			this.comboFormatColor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.comboFormatColor.Name = "comboFormatColor";
			this.comboFormatColor.Size = new System.Drawing.Size(186, 23);
			this.comboFormatColor.TabIndex = 5;
			this.comboFormatColor.SelectedIndexChanged += new System.EventHandler(this.comboFormatColor_SelectedIndexChanged);
			// 
			// comboPin
			// 
			this.comboPin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPin.BackColor = System.Drawing.SystemColors.Control;
			this.comboPin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboPin.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboPin.FormattingEnabled = true;
			this.comboPin.Location = new System.Drawing.Point(133, 57);
			this.comboPin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.comboPin.Name = "comboPin";
			this.comboPin.Size = new System.Drawing.Size(286, 23);
			this.comboPin.TabIndex = 3;
			this.comboPin.SelectedIndexChanged += new System.EventHandler(this.comboPin_SelectedIndexChanged);
			// 
			// comboFilter
			// 
			this.comboFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboFilter.BackColor = System.Drawing.SystemColors.Control;
			this.comboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboFilter.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboFilter.FormattingEnabled = true;
			this.comboFilter.Location = new System.Drawing.Point(133, 19);
			this.comboFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.comboFilter.Name = "comboFilter";
			this.comboFilter.Size = new System.Drawing.Size(286, 23);
			this.comboFilter.TabIndex = 1;
			this.comboFilter.SelectedIndexChanged += new System.EventHandler(this.comboFilter_SelectedIndexChanged);
			// 
			// labelFormat
			// 
			this.labelFormat.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelFormat.Location = new System.Drawing.Point(14, 96);
			this.labelFormat.Name = "labelFormat";
			this.labelFormat.Size = new System.Drawing.Size(112, 23);
			this.labelFormat.TabIndex = 4;
			this.labelFormat.Text = "Format";
			this.labelFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPin
			// 
			this.labelPin.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelPin.Location = new System.Drawing.Point(14, 58);
			this.labelPin.Name = "labelPin";
			this.labelPin.Size = new System.Drawing.Size(112, 23);
			this.labelPin.TabIndex = 2;
			this.labelPin.Text = "Pin";
			this.labelPin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelFilter
			// 
			this.labelFilter.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelFilter.Location = new System.Drawing.Point(14, 20);
			this.labelFilter.Name = "labelFilter";
			this.labelFilter.Size = new System.Drawing.Size(112, 23);
			this.labelFilter.TabIndex = 0;
			this.labelFilter.Text = "Filter";
			this.labelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabView
			// 
			this.tabView.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabView.Controls.Add(this.tabPageView);
			this.tabView.Controls.Add(this.tabPageProperty);
			this.tabView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabView.ItemSize = new System.Drawing.Size(96, 24);
			this.tabView.Location = new System.Drawing.Point(0, 0);
			this.tabView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabView.Name = "tabView";
			this.tabView.SelectedIndex = 0;
			this.tabView.Size = new System.Drawing.Size(484, 267);
			this.tabView.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabView.TabIndex = 0;
			// 
			// tabPageView
			// 
			this.tabPageView.Controls.Add(this.pictureView);
			this.tabPageView.Location = new System.Drawing.Point(4, 28);
			this.tabPageView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPageView.Name = "tabPageView";
			this.tabPageView.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPageView.Size = new System.Drawing.Size(476, 235);
			this.tabPageView.TabIndex = 0;
			this.tabPageView.Text = "View";
			this.tabPageView.UseVisualStyleBackColor = true;
			// 
			// pictureView
			// 
			this.pictureView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureView.Location = new System.Drawing.Point(3, 4);
			this.pictureView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pictureView.Name = "pictureView";
			this.pictureView.Size = new System.Drawing.Size(470, 227);
			this.pictureView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureView.TabIndex = 0;
			this.pictureView.TabStop = false;
			this.pictureView.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureView_Paint);
			// 
			// tabPageProperty
			// 
			this.tabPageProperty.Controls.Add(this.propertyParam);
			this.tabPageProperty.Location = new System.Drawing.Point(4, 28);
			this.tabPageProperty.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPageProperty.Name = "tabPageProperty";
			this.tabPageProperty.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPageProperty.Size = new System.Drawing.Size(476, 235);
			this.tabPageProperty.TabIndex = 1;
			this.tabPageProperty.Text = "Property";
			this.tabPageProperty.UseVisualStyleBackColor = true;
			// 
			// propertyParam
			// 
			this.propertyParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyParam.Location = new System.Drawing.Point(3, 4);
			this.propertyParam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.propertyParam.Name = "propertyParam";
			this.propertyParam.Size = new System.Drawing.Size(470, 227);
			this.propertyParam.TabIndex = 0;
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Interval = 1;
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// CxDSCameraSelectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 487);
			this.Controls.Add(this.splitView);
			this.Controls.Add(this.Statusbar);
			this.Controls.Add(this.Toolbar);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxDSCameraSelectionForm";
			this.Text = "Camera Selection";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxDSSelectionForm_FormClosing);
			this.Load += new System.EventHandler(this.CxDSSelectionForm_Load);
			this.Toolbar.ResumeLayout(false);
			this.Toolbar.PerformLayout();
			this.Statusbar.ResumeLayout(false);
			this.Statusbar.PerformLayout();
			this.splitView.Panel1.ResumeLayout(false);
			this.splitView.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitView)).EndInit();
			this.splitView.ResumeLayout(false);
			this.tabView.ResumeLayout(false);
			this.tabPageView.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureView)).EndInit();
			this.tabPageProperty.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DSLab.Forms.CxToolStripEx Toolbar;
		private System.Windows.Forms.StatusStrip Statusbar;
		private System.Windows.Forms.SplitContainer splitView;
		private System.Windows.Forms.PictureBox pictureView;
		private System.Windows.Forms.ComboBox comboFilter;
		private System.Windows.Forms.Label labelFilter;
		private System.Windows.Forms.ComboBox comboFormatColor;
		private System.Windows.Forms.ComboBox comboPin;
		private System.Windows.Forms.Label labelFormat;
		private System.Windows.Forms.Label labelPin;
		private System.Windows.Forms.ComboBox comboFormatSize;
		private System.Windows.Forms.ToolStripButton toolCancel;
		private System.Windows.Forms.ToolStripButton toolOK;
		private System.Windows.Forms.ToolStripStatusLabel statusFps;
		private System.Windows.Forms.ToolStripStatusLabel statusSpacer;
		private System.Windows.Forms.ToolStripStatusLabel statusTimeStamp;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ToolStripButton toolSave;
		private System.Windows.Forms.Button buttonPin;
		private System.Windows.Forms.Button buttonFilter;
		private System.Windows.Forms.TabControl tabView;
		private System.Windows.Forms.TabPage tabPageView;
		private System.Windows.Forms.TabPage tabPageProperty;
		private System.Windows.Forms.PropertyGrid propertyParam;
		private System.Windows.Forms.ToolStripStatusLabel statusMag;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStart;
		private System.Windows.Forms.ToolStripButton toolPause;
		private System.Windows.Forms.ToolStripButton toolStop;
	}
}
