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
	partial class Program
	{
		/// <summary>
		/// カメラから画像を取り込み、動画ファイル(wmv)へ保存する処理
		/// </summary>
		/// <remarks>
		///		WMV 形式で保存する場合は映像と音声の両方が必要です。<br/>
		///		<br/>
		///		RenderStream (NULL, MEDIATYPE_Audio, audio, null, renderner)<br/>
		///		RenderStream (NULL, MEDIATYPE_Video, video, grabber, renderner)<br/>
		///		<pre>
		///		 audio                       renderer
		///		+-------+                   +-------+
		///		|       0 ----------------- 0       |
		///		+-------+                   |       |
		///		 video         grabber      |       | 
		///		+-------+     +-------+     |       |
		///		|       0 --- 0       0 --- 1       |
		///		|       1     |       |     +-------+
		///		+-------+     +-------+     
		///		</pre>
		///		※ 本例では grabber は省略しています。<br/>
		/// </remarks>
		public static void Sample13()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			IGraphBuilder graph = null;
			ICaptureGraphBuilder2 builder = null;
			IBaseFilter videoSource = null;
			IBaseFilter audioSource = null;
			IBaseFilter videoRenderer = null;
			IFileSinkFilter fileSink = null;
			string filename = Path.Combine(Results, __FUNCTION__ + ".wmv");
			var frameSize = new Size(640, 480);

			try
			{
				#region グラフビルダーの生成:
				{
					graph = (IGraphBuilder)Axi.CoCreateInstance(GUID.CLSID_FilterGraph);
					if (graph == null)
						throw new System.IO.IOException("Failed to create a GraphBuilder.");

					builder = (ICaptureGraphBuilder2)Axi.CoCreateInstance(GUID.CLSID_CaptureGraphBuilder2);
					if (builder == null)
						throw new System.IO.IOException("Failed to create a GraphBuilder.");
					builder.SetFiltergraph(graph);
				}
				#endregion

				#region 映像入力用: ソースフィルタを生成します.
				{
					videoSource = Axi.CreateFilter(GUID.CLSID_VideoInputDeviceCategory, null, 0);
					if (videoSource == null)
						throw new System.IO.IOException("Failed to create a videoSource.");
					graph.AddFilter(videoSource, "VideoSource");

					// フレームサイズを設定します.
					// ※注) この操作は、ピンを接続する前に行う必要があります.
					IPin pin = Axi.FindPin(videoSource, 0, PIN_DIRECTION.PINDIR_OUTPUT);
					Axi.SetFormatSize(pin, frameSize.Width, frameSize.Height);
				}
				#endregion

				#region 音声入力用: ソースフィルタを生成します.
				{
					audioSource = Axi.CreateFilter(GUID.CLSID_AudioInputDeviceCategory, null, 0);
					if (audioSource == null)
						throw new System.IO.IOException("Failed to create a audioSource.");
					graph.AddFilter(audioSource, "audioSource");
				}
				#endregion

				#region 映像出力用: 保存する動画ファイル名を設定します.
				unsafe
				{
					// 動画ファイルを保存する設定:
					var filetype = new Guid(GUID.MEDIASUBTYPE_Asf);
					HRESULT hr = (HRESULT)builder.SetOutputFileName(new IntPtr(&filetype), filename, ref videoRenderer, ref fileSink);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					/*
					 * Capturing Video to a Windows Media File
					 * https://msdn.microsoft.com/en-us/library/windows/desktop/dd318630(v=vs.85).aspx
					 */
					var config = (IConfigAsfWriter)videoRenderer;

					// WMProfile
					{
						// WMProfile_V80_256Video が取得されます.
						Guid currentProfileGuid;
						hr = (HRESULT)config.GetCurrentProfileGuid(out currentProfileGuid);
						if (hr < HRESULT.S_OK)
							throw new CxDSException(hr);

						// WMProfile_V80_BESTVBRVideo に書き替えます.
						Guid newProfileGuid = new Guid(GUID.WMProfile_V80_BESTVBRVideo);
						hr = (HRESULT)config.ConfigureFilterUsingProfileGuid(newProfileGuid);
						if (hr < HRESULT.S_OK)
							throw new CxDSException(hr);
					}
				}
				#endregion

				#region フィルタの接続:
				unsafe
				{
					HRESULT hr;

					// フィルタの接続: (映像入力)
					var mediatype_video = new Guid(GUID.MEDIATYPE_Video);
					hr = (HRESULT)builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype_video), videoSource, null, videoRenderer);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					// フィルタの接続: (音声入力)
					var mediatype_audio = new Guid(GUID.MEDIATYPE_Audio);
					hr = (HRESULT)builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype_audio), audioSource, null, videoRenderer);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);
				}
				#endregion

				#region 映像出力用: 保存する動画ファイルのフレームサイズを設定します.
				unsafe
				{
					/*
					 * Capturing Video to a Windows Media File
					 * https://msdn.microsoft.com/en-us/library/windows/desktop/dd318630(v=vs.85).aspx
					 */
					var config = (IConfigAsfWriter)videoRenderer;

					// フレームサイズの変更:
					Axi.SetVideoFrameSize(config, frameSize);
				}
				#endregion

				// ------------------------------

				#region 取り込み処理:
				{
					var mediaControl = (IMediaControl)graph;
					var mediaEvent = (IMediaEvent)graph;

					var watch = new Stopwatch();
					watch.Start();

					// 露光開始.
					Console.WriteLine("Run ...");
					{
						HRESULT hr;
						int state;
						hr = (HRESULT)mediaControl.Run();
						hr = (HRESULT)mediaControl.GetState(1000, out state);
					}
					Console.WriteLine("Running ... {0:F3} msec", watch.Elapsed.TotalMilliseconds);

					// 取り込み待機.
					while (watch.ElapsedMilliseconds < 5000)
					{
						System.Threading.Thread.Sleep(1);
					}

					// 露光停止.
					Console.WriteLine("Stop ... {0:F3} msec", watch.Elapsed.TotalMilliseconds);
					{
						HRESULT hr;
						int state;
						hr = (HRESULT)mediaControl.Stop();
						hr = (HRESULT)mediaControl.GetState(1000, out state);
					}

					Console.WriteLine("Completed. {0:F3} msec", watch.Elapsed.TotalMilliseconds);
				}
				#endregion
			}
			catch (System.Exception ex)
			{
				Console.WriteLine("{0}", ex.StackTrace);
			}
			finally
			{
				#region 解放:
				if (videoSource != null)
					Marshal.ReleaseComObject(videoSource);
				videoSource = null;

				if (videoRenderer != null)
					Marshal.ReleaseComObject(videoRenderer);
				videoRenderer = null;

				if (fileSink != null)
					Marshal.ReleaseComObject(fileSink);
				fileSink = null;

				if (builder != null)
					Marshal.ReleaseComObject(builder);
				builder = null;

				if (graph != null)
					Marshal.ReleaseComObject(graph);
				graph = null;
				#endregion
			}
		}
	}
}
