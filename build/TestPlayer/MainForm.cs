/*
	DSLab
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DSLab;

namespace demo
{
	/// <summary>
	/// メインフォーム
	/// </summary>
	public partial class MainForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Demo2Form_Load(object sender, EventArgs e)
		{
			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォーム終了時の解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Demo2Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();
		}

		#endregion

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			#region コントロールの表示更新:
			{
				bool enable = Player_Connected;
				bool running = Player_IsRunning;
				bool paused = Player_IsPaused;

				toolStart.Enabled = (enable && !running);
				toolPause.Enabled = (enable && running);
				toolPause.Checked = (enable && paused);
				toolStop.Enabled = (enable && running);
				toolReset.Enabled = (enable);
			}
			#endregion

			#region 画像の表示更新:
			var index = this.BufferIndex;
			var image = this.Buffer[index];
			if (image != null)
			{
				this.Buffer[index] = null;
				pictureView.BackgroundImage = image;
				pictureView.Refresh();

				#region ステータスバーの表示更新:
				{
					var ts = DateTime.Now;
					statusFrameIndex.Text = string.Format(
						"{0:0000}.{1:00}.{2:00} {3:00}:{4:00}:{5:00}.{6:000}",
						ts.Year, ts.Month, ts.Day,
						ts.Hour, ts.Minute, ts.Second, ts.Millisecond
						);

					statusImageSize.Text = string.Format("{0}x{1}", image.Width, image.Height);
				}
				#endregion
			}
			#endregion

			#region シーク用コントロールの表示更新:
			try
			{
				var mediaSeeking = (IMediaSeeking)Graph;
				if (mediaSeeking != null)
				{
					int hr;
					long start_positoin = 0;
					long stop_position = 0;
					hr = mediaSeeking.GetPositions(ref start_positoin, ref stop_position);
					if (hr >= 0)
					{
						trackStartPosition.Value = (int)(start_positoin / UNIT);
						trackStopPosition.Value = (int)(stop_position / UNIT);

						statusSeeking.Text = string.Format("[{0}/{1}]", trackStartPosition.Value, trackStopPosition.Value);
					}
				}
			}
			catch (System.Exception)
			{
			}
			#endregion
		}

		/// <summary>
		/// 同期用: サンプルグラバーの通知イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void VideoGrabberCB_Notify(object sender, CxSampleGrabberEventArgs e)
		{
			var index = (this.BufferIndex + 1) % this.Buffer.Length;
			var image = e.ToImage(VideoInfoHeader);
			this.Buffer[index] = image;
			this.BufferIndex = index;
		}

		#region DirectShow Objects:

		IGraphBuilder Graph = null;
		ICaptureGraphBuilder2 Builder = null;
		IBaseFilter VideoSource = null;
		IBaseFilter Splitter = null;
		IBaseFilter VideoGrabber = null;
		IBaseFilter AudioGrabber = null;
		IBaseFilter VideoRenderer = null;
		IBaseFilter AudioRenderer = null;
		CxSampleGrabberCB VideoGrabberCB = new CxSampleGrabberCB();
		CxSampleGrabberCB AudioGrabberCB = new CxSampleGrabberCB();
		private VIDEOINFOHEADER VideoInfoHeader = new VIDEOINFOHEADER();
		private Bitmap[] Buffer = new Bitmap[5];
		private int BufferIndex = 0;
		const int UNIT = 10 * 1000;	// x 100 nsec

		/// <summary>
		/// プレイヤーの接続
		/// </summary>
		/// <param name="filename"></param>
		private void Player_Connect(string filename)
		{
			#region グラフビルダーの生成:
			{
				Graph = (IGraphBuilder)Axi.CoCreateInstance(GUID.CLSID_FilterGraph);
				if (Graph == null)
					throw new System.IO.IOException("Failed to create a GraphBuilder.");

				Builder = (ICaptureGraphBuilder2)Axi.CoCreateInstance(GUID.CLSID_CaptureGraphBuilder2);
				if (Builder == null)
					throw new System.IO.IOException("Failed to create a GraphBuilder.");
				Builder.SetFiltergraph(Graph);
			}
			#endregion

			#region 映像入力用: ソースフィルタを生成します.
			{
				Graph.AddSourceFilter(filename, "VideoSource", ref VideoSource);
				if (VideoSource == null)
					throw new System.IO.IOException("Failed to create a VideoSource.");
			}
			#endregion

			#region 映像捕獲用: サンプルグラバーを生成します.
			{
				VideoGrabber = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_SampleGrabber);
				if (VideoGrabber == null)
					throw new System.IO.IOException("Failed to create a VideoGrabber.");
				Graph.AddFilter(VideoGrabber, "VideoGrabber");

				// サンプルグラバフィルタの入力形式設定.
				// SetMediaType で必要なメディア タイプを指定します。
				//   http://msdn.microsoft.com/ja-jp/library/cc369546.aspx
				// ※AM_MEDIA_TYPE 構造体のメンバをすべて設定する必要はない。
				// ※デフォルトでは、サンプル グラバに優先メディア タイプはない。
				// ※サンプル グラバを正しいフィルタに確実に接続するには、フィルタ グラフを作成する前にこのメソッドを呼び出す。
				// majortype: http://msdn.microsoft.com/ja-jp/library/cc370108.aspx
				// subtype  : http://msdn.microsoft.com/ja-jp/library/cc371040.aspx
				{
					var grabber = (ISampleGrabber)VideoGrabber;

					var mt = new AM_MEDIA_TYPE();
					mt.majortype = new Guid(GUID.MEDIATYPE_Video);
					mt.subtype = new Guid(GUID.MEDIASUBTYPE_RGB24);
					mt.formattype = new Guid(GUID.FORMAT_VideoInfo);
					grabber.SetMediaType(mt);
					grabber.SetBufferSamples(false);			// サンプルコピー 無効.
					grabber.SetOneShot(false);					// One Shot 無効.
					//grabber.SetCallback(VideoGrabberCB, 0);	// 0:SampleCB メソッドを呼び出すよう指示する.
					grabber.SetCallback(VideoGrabberCB, 1);		// 1:BufferCB メソッドを呼び出すよう指示する.
				}
			}
			#endregion

			#region 音声捕獲用: サンプルグラバーを生成します.
			{
				AudioGrabber = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_SampleGrabber);
				if (AudioGrabber == null)
					throw new System.IO.IOException("Failed to create a AudioGrabber.");
				Graph.AddFilter(AudioGrabber, "AudioGrabber");

				// サンプルグラバフィルタの入力形式設定.
				// SetMediaType で必要なメディア タイプを指定します。
				//   http://msdn.microsoft.com/ja-jp/library/cc369546.aspx
				// ※AM_MEDIA_TYPE 構造体のメンバをすべて設定する必要はない。
				// ※デフォルトでは、サンプル グラバに優先メディア タイプはない。
				// ※サンプル グラバを正しいフィルタに確実に接続するには、フィルタ グラフを作成する前にこのメソッドを呼び出す。
				// majortype: http://msdn.microsoft.com/ja-jp/library/cc370108.aspx
				// subtype  : http://msdn.microsoft.com/ja-jp/library/cc371040.aspx
				{
					var grabber = (ISampleGrabber)AudioGrabber;

					var mt = new AM_MEDIA_TYPE();
					mt.majortype = new Guid(GUID.MEDIATYPE_Audio);
					mt.subtype = new Guid(GUID.MEDIASUBTYPE_PCM);
					mt.formattype = new Guid(GUID.FORMAT_WaveFormatEx);
					grabber.SetMediaType(mt);
					grabber.SetBufferSamples(false);			// サンプルコピー 無効.
					grabber.SetOneShot(false);					// One Shot 無効.
					//grabber.SetCallback(AudioGrabberCB, 0);	// 0:SampleCB メソッドを呼び出すよう指示する.
					grabber.SetCallback(AudioGrabberCB, 1);		// 1:BufferCB メソッドを呼び出すよう指示する.
				}
			}
			#endregion

			#region 映像出力用: レンダラーを生成します.
			{
				VideoRenderer = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_NullRenderer);
				if (VideoRenderer == null)
					throw new System.IO.IOException("Failed to create a VideoRenderer.");
				Graph.AddFilter(VideoRenderer, "VideoRenderer");
			}
			#endregion

			#region 音声出力用: レンダラーを生成します.
			{
				AudioRenderer = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_NullRenderer);
				if (AudioRenderer == null)
					throw new System.IO.IOException("Failed to create a AudioRenderer.");
				Graph.AddFilter(AudioRenderer, "AudioRenderer");
			}
			#endregion

			#region フィルタの接続:
			if (filename.EndsWith(".avi", StringComparison.InvariantCultureIgnoreCase))
			{
				#region AVI 形式ファイル用の初期化:
				unsafe
				{
					HRESULT hr;

					// AVI 分離器の追加:
					Splitter = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_AviSplitter);
					if (Splitter == null)
						throw new System.IO.IOException("Failed to create a Splitter.");
					Graph.AddFilter(Splitter, "Splitter");

					// フィルタの接続: (AVI 分離器)
					hr = (HRESULT)Builder.RenderStream(IntPtr.Zero, IntPtr.Zero, VideoSource, null, Splitter);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					// フィルタの接続: (映像入力)
					var mediatype_video = new Guid(GUID.MEDIATYPE_Video);
					hr = (HRESULT)Builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype_video), Splitter, VideoGrabber, VideoRenderer);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					// フィルタの接続: (音声入力) ※ Audioless も有る.
					try
					{
						var mediatype_audio = new Guid(GUID.MEDIATYPE_Audio);
						hr = (HRESULT)Builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype_audio), Splitter, AudioGrabber, AudioRenderer);
					}
					catch (System.Exception ex)
					{
						Debug.WriteLine(ex.StackTrace);
					}
				}
				#endregion
			}
			else if (
				filename.EndsWith(".asf", StringComparison.InvariantCultureIgnoreCase) ||
				filename.EndsWith(".wmv", StringComparison.InvariantCultureIgnoreCase))
			{
				#region WMV 形式ファイル用の初期化:
				unsafe
				{
					HRESULT hr;

					// フィルタの接続: (映像入力)
					var mediatype_video = new Guid(GUID.MEDIATYPE_Video);
					hr = (HRESULT)Builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype_video), VideoSource, VideoGrabber, VideoRenderer);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					// フィルタの接続: (音声入力)
					var mediatype_audio = new Guid(GUID.MEDIATYPE_Audio);
					hr = (HRESULT)Builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype_audio), VideoSource, AudioGrabber, AudioRenderer);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);
				}
				#endregion
			}
			#endregion

			// 同期用: サンプルグラバーのイベント登録:
			VideoGrabberCB.Enable = true;
			VideoGrabberCB.Notify += VideoGrabberCB_Notify;
			VideoInfoHeader = Axi.GetVideoInfo((ISampleGrabber)VideoGrabber);
		}

		/// <summary>
		/// プレイヤーの切断
		/// </summary>
		private void Player_Disconnect()
		{
			if (Player_IsRunning)
				Player_Stop();

			// 同期用: サンプルグラバーのイベント登録解除:
			VideoGrabberCB.Enable = false;
			VideoGrabberCB.Notify -= VideoGrabberCB_Notify;

			#region 解放:
			if (VideoSource != null)
				Marshal.ReleaseComObject(VideoSource);
			VideoSource = null;

			if (Splitter != null)
				Marshal.ReleaseComObject(Splitter);
			Splitter = null;

			if (VideoGrabber != null)
				Marshal.ReleaseComObject(VideoGrabber);
			VideoGrabber = null;

			if (AudioGrabber != null)
				Marshal.ReleaseComObject(AudioGrabber);
			AudioGrabber = null;

			if (VideoRenderer != null)
				Marshal.ReleaseComObject(VideoRenderer);
			VideoRenderer = null;

			if (AudioRenderer != null)
				Marshal.ReleaseComObject(AudioRenderer);
			AudioRenderer = null;

			if (Builder != null)
				Marshal.ReleaseComObject(Builder);
			Builder = null;

			if (Graph != null)
				Marshal.ReleaseComObject(Graph);
			Graph = null;
			#endregion
		}

		/// <summary>
		/// プレイヤーの接続状態
		/// </summary>
		private bool Player_Connected
		{
			get { return (Graph != null); }
		}

		/// <summary>
		/// プレイヤーの動作状態
		/// </summary>
		private bool Player_IsRunning
		{
			get
			{
				var mediaControl = (IMediaControl)Graph;
				if (mediaControl == null) return false;

				#region 動作状態の確認:
				try
				{
					int state = 0;
					int hr = mediaControl.GetState(0, out state);
					if (hr < 0)
						return false;
					return (
						state == (int)FILTER_STATE.Running ||
						state == (int)FILTER_STATE.Paused);
				}
				catch (System.Exception)
				{
					return false;
				}
				#endregion
			}
		}

		/// <summary>
		/// プレイヤーの一時停止状態
		/// </summary>
		private bool Player_IsPaused
		{
			get
			{
				var mediaControl = (IMediaControl)Graph;
				if (mediaControl == null) return false;

				#region 一時停止状態の確認:
				try
				{
					int state = 0;
					int hr = mediaControl.GetState(0, out state);
					if (hr < 0)
						return false;
					return (state == (int)FILTER_STATE.Paused);
				}
				catch (System.Exception)
				{
					return false;
				}
				#endregion
			}
		}

		/// <summary>
		/// プレイヤーの再生
		/// </summary>
		private void Player_Start()
		{
			var mediaControl = (IMediaControl)Graph;
			if (mediaControl == null) return;

			mediaControl.Run();
			int state = 0;
			mediaControl.GetState(3000, out state);
		}

		/// <summary>
		/// プレイヤーの一時停止
		/// </summary>
		private void Player_Pause()
		{
			var mediaControl = (IMediaControl)Graph;
			if (mediaControl == null) return;

			mediaControl.Pause();
			int state = 0;
			mediaControl.GetState(3000, out state);
		}

		/// <summary>
		/// プレイヤーの停止
		/// </summary>
		private void Player_Stop()
		{
			var mediaControl = (IMediaControl)Graph;
			if (mediaControl == null) return;

			mediaControl.Stop();
			int state = 0;
			mediaControl.GetState(3000, out state);
		}

		/// <summary>
		/// プレイヤーのシーク位置のリセット
		/// </summary>
		private void Player_Reset()
		{
			#region コントロールの初期化:
			try
			{
				var mediaSeeking = (IMediaSeeking)Graph;
				if (mediaSeeking == null)
					throw new NotSupportedException();

				int hr;
				long duration = 0;
				hr = mediaSeeking.GetDuration(ref duration);
				if (hr < 0)
					throw new CxDSException((HRESULT)hr);

				long start_positoin = 0;
				long stop_position = duration;
				hr = mediaSeeking.SetPositions(
					start_positoin, AM_SEEKING_SEEKING_FLAGS.AbsolutePositioning,
					stop_position, AM_SEEKING_SEEKING_FLAGS.AbsolutePositioning
					);
				if (hr < 0)
					throw new CxDSException((HRESULT)hr);

				// 開始位置.
				trackStartPosition.Enabled = true;
				trackStartPosition.Value = trackStartPosition.Minimum;

				// 停止位置.
				trackStopPosition.Enabled = true;
				trackStopPosition.Value = trackStopPosition.Maximum;
			}
			catch (System.Exception)
			{
				trackStartPosition.Enabled = false;
				trackStopPosition.Enabled = false;
			}
			#endregion
		}

		#endregion

		#region Toolbar:

		/// <summary>
		/// 開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOpen_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialog dlg = new OpenFileDialog();
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				dlg.Filter = "Media files|*.avi;*.asf;*.wmv";
				dlg.Filter += "|AVI files|*.avi";
				dlg.Filter += "|ASF files|*.asf";
				dlg.Filter += "|WMV files|*.wmv";
				dlg.Filter += "|All files|*.*";

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					Player_Connect(dlg.FileName);

					#region コントロールの初期化:
					try
					{
						var mediaSeeking = (IMediaSeeking)Graph;
						if (mediaSeeking == null)
							throw new NotSupportedException();

						int hr;
						long duration = 0;
						long start_positoin = 0;
						long stop_position = 0;
						hr = mediaSeeking.GetDuration(ref duration);
						if (hr < 0)
							throw new CxDSException((HRESULT)hr);

						hr = mediaSeeking.GetPositions(ref start_positoin, ref stop_position);
						if (hr < 0)
							throw new CxDSException((HRESULT)hr);

						// 開始位置.
						trackStartPosition.Enabled = true;
						trackStartPosition.Minimum = 0;
						trackStartPosition.Maximum = (int)(duration / UNIT);
						trackStartPosition.Value = (int)(start_positoin / UNIT);

						// 停止位置.
						trackStopPosition.Enabled = true;
						trackStopPosition.Minimum = 0;
						trackStopPosition.Maximum = (int)(duration / UNIT);
						trackStopPosition.Value = (int)(stop_position / UNIT);
					}
					catch (System.Exception)
					{
						trackStartPosition.Enabled = false;
						trackStopPosition.Enabled = false;
					}
					#endregion
				}
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// 再生
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStart_Click(object sender, EventArgs e)
		{
			try
			{
				if (Player_IsRunning)
					Player_Stop();
				else
					Player_Start();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
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
				if (Player_IsPaused)
					Player_Start();
				else
					Player_Pause();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
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
				Player_Stop();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// リセット
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolReset_Click(object sender, EventArgs e)
		{
			try
			{
				Player_Reset();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		#endregion

		#region TrackBar:

		/// <summary>
		/// TrackBar が操作されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackStartPosition_Scroll(object sender, EventArgs e)
		{
			var mediaSeeking = (IMediaSeeking)Graph;
			if (mediaSeeking == null) return;

			try
			{
				int hr;
				long start_positoin = trackStartPosition.Value * UNIT;
				long stop_position = 0;
				hr = mediaSeeking.SetPositions(
					start_positoin, AM_SEEKING_SEEKING_FLAGS.AbsolutePositioning,
					stop_position, AM_SEEKING_SEEKING_FLAGS.NoPositioning
					);
			}
			catch (System.Exception)
			{
			}
		}

		/// <summary>
		/// TrackBar が操作されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackStopPosition_Scroll(object sender, EventArgs e)
		{
			var mediaSeeking = (IMediaSeeking)Graph;
			if (mediaSeeking == null) return;

			try
			{
				int hr;
				long start_positoin = 0;
				long stop_position = trackStopPosition.Value * UNIT;
				hr = mediaSeeking.SetPositions(
					start_positoin, AM_SEEKING_SEEKING_FLAGS.NoPositioning,
					stop_position, AM_SEEKING_SEEKING_FLAGS.AbsolutePositioning
					);
			}
			catch (System.Exception)
			{
			}
		}

		#endregion
	}
}
