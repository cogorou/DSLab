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
			this.toolOpen = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStart = new System.Windows.Forms.ToolStripButton();
			this.toolPause = new System.Windows.Forms.ToolStripButton();
			this.toolStop = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolReset = new System.Windows.Forms.ToolStripButton();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusFrameIndex = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusSeeking = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusSpacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusImageSize = new System.Windows.Forms.ToolStripStatusLabel();
			this.panelView = new System.Windows.Forms.Panel();
			this.pictureView = new System.Windows.Forms.PictureBox();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.trackStartPosition = new System.Windows.Forms.TrackBar();
			this.trackStopPosition = new System.Windows.Forms.TrackBar();
			this.trackCurrentPosition = new System.Windows.Forms.TrackBar();
			this.toolbar.SuspendLayout();
			this.statusbar.SuspendLayout();
			this.panelView.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackStartPosition)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackStopPosition)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackCurrentPosition)).BeginInit();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolOpen,
            this.toolStripSeparator1,
            this.toolStart,
            this.toolPause,
            this.toolStop,
            this.toolStripSeparator2,
            this.toolReset});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(903, 35);
			this.toolbar.TabIndex = 0;
			this.toolbar.Text = "toolbar";
			// 
			// toolOpen
			// 
			this.toolOpen.AutoSize = false;
			this.toolOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolOpen.Image")));
			this.toolOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOpen.Name = "toolOpen";
			this.toolOpen.Size = new System.Drawing.Size(32, 32);
			this.toolOpen.Text = "Open";
			this.toolOpen.Click += new System.EventHandler(this.toolOpen_Click);
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
			this.toolStart.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
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
			this.toolStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStop.Name = "toolStop";
			this.toolStop.Size = new System.Drawing.Size(32, 32);
			this.toolStop.Text = "Stop";
			this.toolStop.Click += new System.EventHandler(this.toolStop_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
			// 
			// toolReset
			// 
			this.toolReset.AutoSize = false;
			this.toolReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolReset.Image = ((System.Drawing.Image)(resources.GetObject("toolReset.Image")));
			this.toolReset.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolReset.Name = "toolReset";
			this.toolReset.Size = new System.Drawing.Size(32, 32);
			this.toolReset.Text = "Reset";
			this.toolReset.Click += new System.EventHandler(this.toolReset_Click);
			// 
			// statusbar
			// 
			this.statusbar.AutoSize = false;
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusImageSize,
            this.statusFrameIndex,
            this.statusSpacer,
            this.statusSeeking});
			this.statusbar.Location = new System.Drawing.Point(0, 496);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(903, 24);
			this.statusbar.TabIndex = 1;
			this.statusbar.Text = "statusbar";
			// 
			// statusFrameIndex
			// 
			this.statusFrameIndex.Name = "statusFrameIndex";
			this.statusFrameIndex.Size = new System.Drawing.Size(15, 19);
			this.statusFrameIndex.Text = "x";
			// 
			// statusSeeking
			// 
			this.statusSeeking.Name = "statusSeeking";
			this.statusSeeking.Size = new System.Drawing.Size(15, 19);
			this.statusSeeking.Text = "x";
			// 
			// statusSpacer
			// 
			this.statusSpacer.Name = "statusSpacer";
			this.statusSpacer.Size = new System.Drawing.Size(843, 19);
			this.statusSpacer.Spring = true;
			// 
			// statusImageSize
			// 
			this.statusImageSize.Name = "statusImageSize";
			this.statusImageSize.Size = new System.Drawing.Size(15, 19);
			this.statusImageSize.Text = "x";
			// 
			// panelView
			// 
			this.panelView.Controls.Add(this.pictureView);
			this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelView.Location = new System.Drawing.Point(0, 35);
			this.panelView.Name = "panelView";
			this.panelView.Size = new System.Drawing.Size(903, 365);
			this.panelView.TabIndex = 2;
			// 
			// pictureView
			// 
			this.pictureView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.pictureView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureView.Location = new System.Drawing.Point(0, 0);
			this.pictureView.Name = "pictureView";
			this.pictureView.Size = new System.Drawing.Size(903, 365);
			this.pictureView.TabIndex = 0;
			this.pictureView.TabStop = false;
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// trackStartPosition
			// 
			this.trackStartPosition.AutoSize = false;
			this.trackStartPosition.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.trackStartPosition.LargeChange = 10;
			this.trackStartPosition.Location = new System.Drawing.Point(0, 432);
			this.trackStartPosition.Maximum = 100;
			this.trackStartPosition.Name = "trackStartPosition";
			this.trackStartPosition.Size = new System.Drawing.Size(903, 32);
			this.trackStartPosition.TabIndex = 0;
			this.trackStartPosition.TickFrequency = 10;
			this.trackStartPosition.Scroll += new System.EventHandler(this.trackStartPosition_Scroll);
			// 
			// trackStopPosition
			// 
			this.trackStopPosition.AutoSize = false;
			this.trackStopPosition.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.trackStopPosition.LargeChange = 10;
			this.trackStopPosition.Location = new System.Drawing.Point(0, 464);
			this.trackStopPosition.Maximum = 100;
			this.trackStopPosition.Name = "trackStopPosition";
			this.trackStopPosition.Size = new System.Drawing.Size(903, 32);
			this.trackStopPosition.TabIndex = 3;
			this.trackStopPosition.TickFrequency = 10;
			this.trackStopPosition.Scroll += new System.EventHandler(this.trackStopPosition_Scroll);
			// 
			// trackCurrentPosition
			// 
			this.trackCurrentPosition.AutoSize = false;
			this.trackCurrentPosition.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.trackCurrentPosition.Enabled = false;
			this.trackCurrentPosition.Location = new System.Drawing.Point(0, 400);
			this.trackCurrentPosition.Name = "trackCurrentPosition";
			this.trackCurrentPosition.Size = new System.Drawing.Size(903, 32);
			this.trackCurrentPosition.TabIndex = 4;
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(903, 520);
			this.Controls.Add(this.panelView);
			this.Controls.Add(this.trackCurrentPosition);
			this.Controls.Add(this.trackStartPosition);
			this.Controls.Add(this.trackStopPosition);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Name = "MainForm";
			this.Text = "demo";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.panelView.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackStartPosition)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackStopPosition)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackCurrentPosition)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolbar;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.Panel panelView;
		private System.Windows.Forms.ToolStripButton toolOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStart;
		private System.Windows.Forms.ToolStripButton toolStop;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ToolStripStatusLabel statusFrameIndex;
		private System.Windows.Forms.ToolStripStatusLabel statusSpacer;
		private System.Windows.Forms.ToolStripStatusLabel statusImageSize;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolPause;
		private System.Windows.Forms.ToolStripStatusLabel statusSeeking;
		private System.Windows.Forms.TrackBar trackStartPosition;
		private System.Windows.Forms.TrackBar trackStopPosition;
		private System.Windows.Forms.PictureBox pictureView;
		private System.Windows.Forms.ToolStripButton toolReset;
		private System.Windows.Forms.TrackBar trackCurrentPosition;
	}
}

