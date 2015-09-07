namespace demo
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.toolbar = new System.Windows.Forms.ToolStrip();
			this.toolCameraOpen = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStart = new System.Windows.Forms.ToolStripButton();
			this.toolStop = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusImageSize = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusFrameIndex = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusSpacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusPan = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusTilt = new System.Windows.Forms.ToolStripStatusLabel();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.trackPan = new System.Windows.Forms.TrackBar();
			this.trackTilt = new System.Windows.Forms.TrackBar();
			this.buttonReset = new System.Windows.Forms.Button();
			this.trackFocus = new System.Windows.Forms.TrackBar();
			this.groupFocus = new System.Windows.Forms.GroupBox();
			this.textFocus = new System.Windows.Forms.TextBox();
			this.groupExposure = new System.Windows.Forms.GroupBox();
			this.trackExposure = new System.Windows.Forms.TrackBar();
			this.textExposure = new System.Windows.Forms.TextBox();
			this.pictureView = new System.Windows.Forms.PictureBox();
			this.panelView = new System.Windows.Forms.Panel();
			this.toolbar.SuspendLayout();
			this.statusbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackPan)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackTilt)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackFocus)).BeginInit();
			this.groupFocus.SuspendLayout();
			this.groupExposure.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackExposure)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureView)).BeginInit();
			this.panelView.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.Dock = System.Windows.Forms.DockStyle.None;
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolCameraOpen,
            this.toolStripSeparator1,
            this.toolStart,
            this.toolStop,
            this.toolStripSeparator2});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(231, 63);
			this.toolbar.TabIndex = 0;
			this.toolbar.Text = "toolbar";
			// 
			// toolCameraOpen
			// 
			this.toolCameraOpen.AutoSize = false;
			this.toolCameraOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolCameraOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolCameraOpen.Image")));
			this.toolCameraOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolCameraOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolCameraOpen.Name = "toolCameraOpen";
			this.toolCameraOpen.Size = new System.Drawing.Size(72, 60);
			this.toolCameraOpen.Text = "Open";
			this.toolCameraOpen.Click += new System.EventHandler(this.toolCameraOpen_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 63);
			// 
			// toolStart
			// 
			this.toolStart.AutoSize = false;
			this.toolStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStart.Image = ((System.Drawing.Image)(resources.GetObject("toolStart.Image")));
			this.toolStart.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStart.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStart.Name = "toolStart";
			this.toolStart.Size = new System.Drawing.Size(72, 60);
			this.toolStart.Text = "Start";
			this.toolStart.Click += new System.EventHandler(this.toolStart_Click);
			// 
			// toolStop
			// 
			this.toolStop.AutoSize = false;
			this.toolStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStop.Image")));
			this.toolStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStop.Name = "toolStop";
			this.toolStop.Size = new System.Drawing.Size(72, 60);
			this.toolStop.Text = "Stop";
			this.toolStop.Click += new System.EventHandler(this.toolStop_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 63);
			// 
			// statusbar
			// 
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusImageSize,
            this.statusFrameIndex,
            this.statusSpacer,
            this.statusPan,
            this.statusTilt});
			this.statusbar.Location = new System.Drawing.Point(0, 659);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(928, 23);
			this.statusbar.TabIndex = 1;
			this.statusbar.Text = "statusStrip1";
			// 
			// statusImageSize
			// 
			this.statusImageSize.Name = "statusImageSize";
			this.statusImageSize.Size = new System.Drawing.Size(15, 18);
			this.statusImageSize.Text = "x";
			// 
			// statusFrameIndex
			// 
			this.statusFrameIndex.Name = "statusFrameIndex";
			this.statusFrameIndex.Size = new System.Drawing.Size(15, 18);
			this.statusFrameIndex.Text = "x";
			// 
			// statusSpacer
			// 
			this.statusSpacer.Name = "statusSpacer";
			this.statusSpacer.Size = new System.Drawing.Size(852, 18);
			this.statusSpacer.Spring = true;
			// 
			// statusPan
			// 
			this.statusPan.Name = "statusPan";
			this.statusPan.Size = new System.Drawing.Size(15, 18);
			this.statusPan.Text = "P";
			// 
			// statusTilt
			// 
			this.statusTilt.Name = "statusTilt";
			this.statusTilt.Size = new System.Drawing.Size(16, 18);
			this.statusTilt.Text = "T";
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// trackPan
			// 
			this.trackPan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackPan.AutoSize = false;
			this.trackPan.LargeChange = 10;
			this.trackPan.Location = new System.Drawing.Point(0, 624);
			this.trackPan.Maximum = 100;
			this.trackPan.Minimum = -100;
			this.trackPan.Name = "trackPan";
			this.trackPan.Size = new System.Drawing.Size(893, 32);
			this.trackPan.TabIndex = 3;
			this.trackPan.TickFrequency = 10;
			this.trackPan.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackPan.Scroll += new System.EventHandler(this.trackPan_Scroll);
			// 
			// trackTilt
			// 
			this.trackTilt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackTilt.AutoSize = false;
			this.trackTilt.LargeChange = 10;
			this.trackTilt.Location = new System.Drawing.Point(893, 63);
			this.trackTilt.Maximum = 100;
			this.trackTilt.Minimum = -100;
			this.trackTilt.Name = "trackTilt";
			this.trackTilt.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.trackTilt.Size = new System.Drawing.Size(32, 560);
			this.trackTilt.TabIndex = 4;
			this.trackTilt.TickFrequency = 10;
			this.trackTilt.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackTilt.Scroll += new System.EventHandler(this.trackTilt_Scroll);
			// 
			// buttonReset
			// 
			this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonReset.Location = new System.Drawing.Point(894, 626);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(32, 32);
			this.buttonReset.TabIndex = 5;
			this.buttonReset.Text = "R";
			this.buttonReset.UseVisualStyleBackColor = true;
			this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
			// 
			// trackFocus
			// 
			this.trackFocus.AutoSize = false;
			this.trackFocus.LargeChange = 10;
			this.trackFocus.Location = new System.Drawing.Point(62, 18);
			this.trackFocus.Maximum = 50;
			this.trackFocus.Name = "trackFocus";
			this.trackFocus.Size = new System.Drawing.Size(285, 32);
			this.trackFocus.TabIndex = 6;
			this.trackFocus.TickFrequency = 10;
			this.trackFocus.Scroll += new System.EventHandler(this.trackFocus_Scroll);
			// 
			// groupFocus
			// 
			this.groupFocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupFocus.Controls.Add(this.trackFocus);
			this.groupFocus.Controls.Add(this.textFocus);
			this.groupFocus.Location = new System.Drawing.Point(378, 0);
			this.groupFocus.Name = "groupFocus";
			this.groupFocus.Size = new System.Drawing.Size(353, 57);
			this.groupFocus.TabIndex = 6;
			this.groupFocus.TabStop = false;
			this.groupFocus.Text = "Focus";
			// 
			// textFocus
			// 
			this.textFocus.Location = new System.Drawing.Point(7, 19);
			this.textFocus.Name = "textFocus";
			this.textFocus.ReadOnly = true;
			this.textFocus.Size = new System.Drawing.Size(49, 19);
			this.textFocus.TabIndex = 0;
			this.textFocus.Text = "0";
			this.textFocus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// groupExposure
			// 
			this.groupExposure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupExposure.Controls.Add(this.trackExposure);
			this.groupExposure.Controls.Add(this.textExposure);
			this.groupExposure.Location = new System.Drawing.Point(737, 0);
			this.groupExposure.Name = "groupExposure";
			this.groupExposure.Size = new System.Drawing.Size(188, 57);
			this.groupExposure.TabIndex = 6;
			this.groupExposure.TabStop = false;
			this.groupExposure.Text = "Exposure";
			// 
			// trackExposure
			// 
			this.trackExposure.AutoSize = false;
			this.trackExposure.LargeChange = 1;
			this.trackExposure.Location = new System.Drawing.Point(61, 18);
			this.trackExposure.Name = "trackExposure";
			this.trackExposure.Size = new System.Drawing.Size(121, 32);
			this.trackExposure.TabIndex = 6;
			this.trackExposure.Scroll += new System.EventHandler(this.trackExposure_Scroll);
			// 
			// textExposure
			// 
			this.textExposure.Location = new System.Drawing.Point(6, 18);
			this.textExposure.Name = "textExposure";
			this.textExposure.ReadOnly = true;
			this.textExposure.Size = new System.Drawing.Size(49, 19);
			this.textExposure.TabIndex = 0;
			this.textExposure.Text = "0";
			this.textExposure.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// pictureView
			// 
			this.pictureView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.pictureView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureView.Location = new System.Drawing.Point(0, 0);
			this.pictureView.Name = "pictureView";
			this.pictureView.Size = new System.Drawing.Size(893, 560);
			this.pictureView.TabIndex = 0;
			this.pictureView.TabStop = false;
			// 
			// panelView
			// 
			this.panelView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelView.Controls.Add(this.pictureView);
			this.panelView.Location = new System.Drawing.Point(0, 63);
			this.panelView.Name = "panelView";
			this.panelView.Size = new System.Drawing.Size(893, 560);
			this.panelView.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(928, 682);
			this.Controls.Add(this.groupExposure);
			this.Controls.Add(this.groupFocus);
			this.Controls.Add(this.buttonReset);
			this.Controls.Add(this.trackTilt);
			this.Controls.Add(this.trackPan);
			this.Controls.Add(this.panelView);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Name = "MainForm";
			this.Text = "demo";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackPan)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackTilt)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackFocus)).EndInit();
			this.groupFocus.ResumeLayout(false);
			this.groupFocus.PerformLayout();
			this.groupExposure.ResumeLayout(false);
			this.groupExposure.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackExposure)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureView)).EndInit();
			this.panelView.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolbar;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.ToolStripButton toolCameraOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStart;
		private System.Windows.Forms.ToolStripButton toolStop;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ToolStripStatusLabel statusPan;
		private System.Windows.Forms.ToolStripStatusLabel statusFrameIndex;
		private System.Windows.Forms.ToolStripStatusLabel statusSpacer;
		private System.Windows.Forms.ToolStripStatusLabel statusImageSize;
		private System.Windows.Forms.TrackBar trackPan;
		private System.Windows.Forms.TrackBar trackTilt;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.ToolStripStatusLabel statusTilt;
		private System.Windows.Forms.TrackBar trackFocus;
		private System.Windows.Forms.GroupBox groupFocus;
		private System.Windows.Forms.TextBox textFocus;
		private System.Windows.Forms.GroupBox groupExposure;
		private System.Windows.Forms.TrackBar trackExposure;
		private System.Windows.Forms.TextBox textExposure;
		private System.Windows.Forms.PictureBox pictureView;
		private System.Windows.Forms.Panel panelView;
	}
}

