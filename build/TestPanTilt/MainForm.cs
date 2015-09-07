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
		private void MainForm_Load(object sender, EventArgs e)
		{
			#region コントロールの初期化:
			{
				trackFocus.Enabled = false;
				trackExposure.Enabled = false;
				trackPan.Enabled = false;
				trackTilt.Enabled = false;
				buttonReset.Enabled = false;
			}
			#endregion

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォーム終了時の解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();

			#region カメラの切断:
			try
			{
				Camera_Disconnect();
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
			#endregion
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
				bool enable = Camera_Connected;
				bool running = Camera_IsRunning;

				toolStart.Enabled = (enable && !running);
				toolStop.Enabled = (enable && running);
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

			// Pan/Tilt 操作.
			PanTilt_Operation();
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

		private IGraphBuilder Graph = null;
		private ICaptureGraphBuilder2 Builder = null;
		private IBaseFilter VideoSource = null;
		private IBaseFilter VideoGrabber = null;
		private IBaseFilter VideoRenderer = null;
		private IAMCameraControl CameraControl = null;
		private CxSampleGrabberCB VideoGrabberCB = new CxSampleGrabberCB();
		private VIDEOINFOHEADER VideoInfoHeader = new VIDEOINFOHEADER();
		private Bitmap[] Buffer = new Bitmap[5];
		private int BufferIndex = 0;

		/// <summary>
		/// カメラの接続
		/// </summary>
		/// <param name="filterInfo"></param>
		/// <param name="pinno"></param>
		/// <param name="frameSize"></param>
		private void Camera_Connect(CxFilterInfo filterInfo, int pinno, Size frameSize)
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
				VideoSource = Axi.CreateFilter(GUID.CLSID_VideoInputDeviceCategory, filterInfo.CLSID, filterInfo.Index);
				if (VideoSource == null)
					throw new System.IO.IOException("Failed to create a VideoSource.");
				Graph.AddFilter(VideoSource, "VideoSource");

				// フレームサイズを設定します.
				// ※注) この操作は、ピンを接続する前に行う必要があります.
				IPin pin = Axi.FindPin(VideoSource, pinno, PIN_DIRECTION.PINDIR_OUTPUT);
				Axi.SetFormatSize(pin, frameSize.Width, frameSize.Height);
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

			#region 映像出力用: レンダラーを生成します.
			{
				VideoRenderer = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_NullRenderer);
				if (VideoRenderer == null)
					throw new System.IO.IOException("Failed to create a VideoRenderer.");
				Graph.AddFilter(VideoRenderer, "VideoRenderer");
			}
			#endregion

			#region フィルタの接続:
			unsafe
			{
				var mediatype = new Guid(GUID.MEDIATYPE_Video);
				var hr = (HRESULT)Builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype), VideoSource, VideoGrabber, VideoRenderer);
				if (hr < HRESULT.S_OK)
					throw new CxDSException(hr);
			}
			#endregion

			// 同期用: サンプルグラバーのイベント登録:
			VideoGrabberCB.Enable = true;
			VideoGrabberCB.Notify += VideoGrabberCB_Notify;
			VideoInfoHeader = Axi.GetVideoInfo((ISampleGrabber)VideoGrabber);

			// カメラ制御インターフェースの抽出.
			CameraControl = Axi.GetInterface<IAMCameraControl>(this.Graph);
		}

		/// <summary>
		/// カメラの切断
		/// </summary>
		private void Camera_Disconnect()
		{
			if (Camera_IsRunning)
				Camera_Stop();

			// 同期用: サンプルグラバーのイベント登録解除:
			VideoGrabberCB.Enable = false;
			VideoGrabberCB.Notify -= VideoGrabberCB_Notify;

			#region 解放:
			if (CameraControl != null)
				Marshal.ReleaseComObject(CameraControl);
			CameraControl = null;

			if (VideoSource != null)
				Marshal.ReleaseComObject(VideoSource);
			VideoSource = null;

			if (VideoGrabber != null)
				Marshal.ReleaseComObject(VideoGrabber);
			VideoGrabber = null;

			if (VideoRenderer != null)
				Marshal.ReleaseComObject(VideoRenderer);
			VideoRenderer = null;

			if (Builder != null)
				Marshal.ReleaseComObject(Builder);
			Builder = null;

			if (Graph != null)
				Marshal.ReleaseComObject(Graph);
			Graph = null;
			#endregion
		}

		/// <summary>
		/// カメラの接続状態
		/// </summary>
		private bool Camera_Connected
		{
			get { return (Graph != null); }
		}

		/// <summary>
		/// カメラの動作状態
		/// </summary>
		private bool Camera_IsRunning
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
		/// カメラの一時停止状態
		/// </summary>
		private bool Camera_IsPaused
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
		/// カメラの露光開始
		/// </summary>
		private void Camera_Start()
		{
			var mediaControl = (IMediaControl)Graph;
			if (mediaControl == null) return;

			mediaControl.Run();
			int state = 0;
			mediaControl.GetState(3000, out state);
		}

		/// <summary>
		/// カメラの露光停止
		/// </summary>
		private void Camera_Stop()
		{
			var mediaControl = (IMediaControl)Graph;
			if (mediaControl == null) return;

			mediaControl.Stop();
			int state = 0;
			mediaControl.GetState(3000, out state);
		}

		/// <summary>
		/// カメラコントロールのリセット
		/// </summary>
		private void Camera_Reset()
		{
		}

		/// <summary>
		/// サポート状況の確認
		/// </summary>
		/// <param name="prop"></param>
		/// <returns>
		///		サポートしている場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		private bool Camera_IsSupported(CameraControlProperty prop)
		{
			if (CameraControl == null) return false;

			try
			{
				#region レンジの取得を試みる.
				int min = 0;
				int max = 0;
				int step = 0;
				int def = 0;
				int flag = 0;

				var hr = (HRESULT)CameraControl.GetRange(prop, ref min, ref max, ref step, ref def, ref flag);
				if (hr < HRESULT.S_OK)
					return false;

				return true;
				#endregion
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region Toolbar:

		/// <summary>
		/// カメラ選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolCameraOpen_Click(object sender, EventArgs e)
		{
			try
			{
				if (Camera_Connected)
					Camera_Disconnect();

				var dlg = new CxDeviceSelectionForm(new Guid(GUID.CLSID_VideoInputDeviceCategory));
				dlg.StartPosition = FormStartPosition.CenterParent;
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					var filterInfo = dlg.FilterInfos[dlg.FilterIndex];
					var pinno = dlg.PinIndex;
					var frameSize = dlg.FormatInfos[dlg.FormatIndex].VideoSize;

					Camera_Connect(filterInfo, pinno, frameSize);

					#region コントロールの初期化:
					{
						int min = 0;
						int max = 0;
						int step = 0;
						int def = 0;
						int flag = 0;
						int value = 0;

						#region Focus
						try
						{
							HRESULT hr;
							var prop = CameraControlProperty.Focus;
							hr = (HRESULT)CameraControl.GetRange(prop, ref min, ref max, ref step, ref def, ref flag);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);
							hr = (HRESULT)CameraControl.Get(prop, ref value, ref flag);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);

							trackFocus.Enabled = true;
							trackFocus.Minimum = min;
							trackFocus.Maximum = max;
							trackFocus.Value = value;
							textFocus.Text = value.ToString();
						}
						catch (System.Exception)
						{
							trackFocus.Enabled = false;
						}
						#endregion

						#region Exposure
						try
						{
							HRESULT hr;
							var prop = CameraControlProperty.Exposure;
							hr = (HRESULT)CameraControl.GetRange(prop, ref min, ref max, ref step, ref def, ref flag);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);
							hr = (HRESULT)CameraControl.Get(prop, ref value, ref flag);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);

							trackExposure.Enabled = true;
							trackExposure.Minimum = min;
							trackExposure.Maximum = max;
							trackExposure.Value = value;
							textExposure.Text = value.ToString();
						}
						catch (System.Exception)
						{
							trackExposure.Enabled = false;
						}
						#endregion

						#region Pan
						try
						{
							HRESULT hr;
							var prop = CameraControlProperty.Pan;
							hr = (HRESULT)CameraControl.GetRange(prop, ref min, ref max, ref step, ref def, ref flag);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);
							hr = (HRESULT)CameraControl.Get(prop, ref value, ref flag);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);

							trackPan.Enabled = true;
							trackPan.Minimum = min;
							trackPan.Maximum = max;
							trackPan.Value = value;
							statusPan.Text = string.Format("Pan={0}", value);
						}
						catch (System.Exception)
						{
							trackPan.Enabled = false;
						}
						#endregion

						#region Tilt
						try
						{
							HRESULT hr;
							var prop = CameraControlProperty.Tilt;
							hr = (HRESULT)CameraControl.GetRange(prop, ref min, ref max, ref step, ref def, ref flag);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);
							hr = (HRESULT)CameraControl.Get(prop, ref value, ref flag);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);

							trackTilt.Enabled = true;
							trackTilt.Minimum = min;
							trackTilt.Maximum = max;
							trackTilt.Value = value;
							statusTilt.Text = string.Format("Tilt={0}", value);
						}
						catch (System.Exception)
						{
							trackTilt.Enabled = false;
						}
						#endregion

						buttonReset.Enabled = true;
					}
					#endregion

					Camera_Start();
				}
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// 露光開始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStart_Click(object sender, EventArgs e)
		{
			try
			{
				if (Camera_IsRunning)
					Camera_Stop();
				else
					Camera_Start();
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// 露光停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStop_Click(object sender, EventArgs e)
		{
			try
			{
				Camera_Stop();
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		#endregion

		#region Focus/Exposure

		/// <summary>
		/// 焦点調整
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackFocus_Scroll(object sender, EventArgs e)
		{
			try
			{
				#region Focus
				try
				{
					var prop = CameraControlProperty.Focus;
					int flag = (int)CameraControlFlags.Manual;
					int value = trackFocus.Value;
					textFocus.Text = value.ToString();

					HRESULT hr;
					hr = (HRESULT)CameraControl.Set(prop, value, flag);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);
				}
				catch (System.Exception)
				{
					trackFocus.Enabled = false;
				}
				#endregion
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// 露出調整
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackExposure_Scroll(object sender, EventArgs e)
		{
			try
			{
				#region Exposure
				try
				{
					var prop = CameraControlProperty.Exposure;
					int flag = (int)CameraControlFlags.Manual;
					int value = trackExposure.Value;
					textExposure.Text = value.ToString();

					HRESULT hr;
					hr = (HRESULT)CameraControl.Set(prop, value, flag);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);
				}
				catch (System.Exception)
				{
					trackFocus.Enabled = false;
				}
				#endregion
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		#endregion

		#region Pan/Tilt

		/// <summary>
		/// 原点復帰
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>
		///		Pan と Tilt に絶対値で 0 を設定するとメカニカルリセット(原点復帰)されます。
		///		操作の間隔を空ける必要があります。
		///		確認に使用した Logicool Qcam Orbit では 3 sec 以上の間隔が必要でした。
		/// </remarks>
		private void buttonReset_Click(object sender, EventArgs e)
		{
			if (Camera_Connected == false) return;

			try
			{
				PanTilt_Operation_Enable = false;

				// メカニカルrリセット.
				CameraControl.Set(CameraControlProperty.Pan, 0, (int)CameraControlFlags.Manual);
				System.Threading.Thread.Sleep(3000);
				CameraControl.Set(CameraControlProperty.Tilt, 0, (int)CameraControlFlags.Manual);

				// コントロールの表示更新.
				trackPan.Value = 0;
				trackTilt.Value = 0;
				statusPan.Text = string.Format("Pan={0}", trackPan.Value);
				statusTilt.Text = string.Format("Tilt={0}", trackTilt.Value);
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			finally
			{
				PanTilt_Operation_Enable = true;
			}
		}

		/// <summary>
		/// Pan 操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackPan_Scroll(object sender, EventArgs e)
		{
			// see: PanTilt_Operation
		}

		/// <summary>
		/// Tilt 操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackTilt_Scroll(object sender, EventArgs e)
		{
			// see: PanTilt_Operation
		}

		/// <summary>
		/// 操作の有効属性
		/// </summary>
		bool PanTilt_Operation_Enable = true;

		/// <summary>
		/// 前回の操作完了時刻
		/// </summary>
		DateTime PanTilt_Operation_Time = DateTime.Now;

		/// <summary>
		/// 操作の間隔 (sec)
		/// </summary>
		/// <remarks>
		///	機種によると思いますが、確認に使用した Logicool Qcam Orbit では 1 sec 以上の間隔が必要でした。
		/// </remarks>
		const int PanTilt_Operation_Interval = 1;

		/// <summary>
		/// Pan/Tilt 操作
		/// </summary>
		/// <remarks>
		///		本関数は timerUpdateUI_Tick から呼び出されます。
		/// </remarks>
		private void PanTilt_Operation()
		{
			if (Camera_Connected == false) return;

			if (PanTilt_Operation_Enable == false) return;
			const int iterval = PanTilt_Operation_Interval;		// 操作の間隔:
			var span = DateTime.Now - PanTilt_Operation_Time;
			if (span.Seconds < iterval) return;

			do
			{
				#region Pan
				if (trackPan.Enabled)
				{
					try
					{
						// 本体を物理的に稼働させる為には相対値で指定します。
						// 式）移動量 = トラックバーの現在値 - 本体の位置
						// + : 右方向
						// - : 左方向

						HRESULT hr;
						var prop = CameraControlProperty.Pan;
						int old_value = 0;
						int flag = 0;
						hr = (HRESULT)CameraControl.Get(prop, ref old_value, ref flag);
						if (hr < HRESULT.S_OK)
							throw new CxDSException(hr);

						int new_value = trackPan.Value;
						int movement = new_value - old_value;
						if (movement != 0)
						{
							// 相対値.
							hr = (HRESULT)CameraControl.Set(prop, movement, (int)CameraControlFlags.Manual | 0x10);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);

							// 絶対値.
							hr = (HRESULT)CameraControl.Set(prop, new_value, flag);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);

							statusPan.Text = string.Format("Pan={0}", new_value);
							statusPan.ForeColor = Color.FromKnownColor(KnownColor.ControlText);

							break;
						}
					}
					catch (System.Exception)
					{
						statusPan.ForeColor = Color.Red;
						break;
					}
				}
				#endregion

				#region Tilt
				if (trackTilt.Enabled)
				{
					try
					{
						// 本体を物理的に稼働させる為には相対値で指定します。
						// 式）移動量 = トラックバーの現在値 - 本体の位置
						// + : 上方向
						// - : 下方向

						HRESULT hr;
						var prop = CameraControlProperty.Tilt;
						int old_value = 0;
						int flag = 0;
						hr = (HRESULT)CameraControl.Get(prop, ref old_value, ref flag);
						if (hr < HRESULT.S_OK)
							throw new CxDSException(hr);

						int new_value = trackTilt.Value;
						int movement = new_value - old_value;
						if (movement != 0)
						{
							// 相対値.
							hr = (HRESULT)CameraControl.Set(prop, movement, (int)CameraControlFlags.Manual | 0x10);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);

							// 絶対値.
							hr = (HRESULT)CameraControl.Set(prop, new_value, flag);
							if (hr < HRESULT.S_OK)
								throw new CxDSException(hr);

							statusTilt.Text = string.Format("Tilt={0}", new_value);
							statusTilt.ForeColor = Color.FromKnownColor(KnownColor.ControlText);

							break;
						}
					}
					catch (System.Exception)
					{
						statusTilt.ForeColor = Color.Red;
						break;
					}
				}
				#endregion
			}
			while (false);

			PanTilt_Operation_Time = DateTime.Now;
		}

		#endregion
	}
}
