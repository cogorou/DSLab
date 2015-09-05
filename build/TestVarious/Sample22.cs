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
		/// AVI 形式のファイルを読み込み、WMV 形式のファイルに保存する処理
		/// </summary>
		/// <remarks>
		///		RenderStream (NULL, NULL, source, null, splitter)<br/>
		///		RenderStream (NULL, MEDIATYPE_Video, splitter, grabber, renderner)<br/>
		///		RenderStream (NULL, MEDIATYPE_Audio, splitter, null, renderner)<br/>
		///		<pre>
		///		 source        splitter        grabber       renderner
		///		+-------+     +--------+     +---------+     +-------+
		///		|       0 --- 0  video 0 --- 0  video  0 --- 1       |
		///		|       |     |        |     +---------+     |       |
		///		|       |     |        |                     |       |
		///		|       |     |        |     +---------+     |       |
		///		|       |     |  audio 1 --- 0  audio  0 --- 0       |
		///		+-------+     +--------+     +---------+     +-------+
		///		</pre>
		///		※ 本例では grabber は省略しています。<br/>
		/// </remarks>
		public static void Sample22()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			IGraphBuilder graph = null;
			ICaptureGraphBuilder2 builder = null;
			IBaseFilter videoSource = null;
			IBaseFilter aviSplitter = null;
			IBaseFilter videoGrabber = null;
			IBaseFilter videoRenderer = null;
			IFileSinkFilter fileSink = null;
			var videoGrabberCB = new CxSampleGrabberCB();
			string src_filename = Path.Combine(TestFiles, "stopwatch_320x240.avi");
			string dst_filename = Path.Combine(Results, __FUNCTION__ + ".wmv");

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
					graph.AddSourceFilter(src_filename, "VideoSource", ref videoSource);
					if (videoSource == null)
						throw new System.IO.IOException("Failed to create a videoSource.");

					aviSplitter = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_AviSplitter);
					if (aviSplitter == null)
						throw new System.IO.IOException("Failed to create a aviSplitter.");
					graph.AddFilter(aviSplitter, "aviSplitter");
				}
				#endregion

				#region 映像捕獲用: サンプルグラバーを生成します.
				{
					videoGrabber = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_SampleGrabber);
					if (videoGrabber == null)
						throw new System.IO.IOException("Failed to create a videoGrabber.");
					graph.AddFilter(videoGrabber, "videoGrabber");

					// サンプルグラバフィルタの入力形式設定.
					// SetMediaType で必要なメディア タイプを指定します。
					//   http://msdn.microsoft.com/ja-jp/library/cc369546.aspx
					// ※AM_MEDIA_TYPE 構造体のメンバをすべて設定する必要はない。
					// ※デフォルトでは、サンプル グラバに優先メディア タイプはない。
					// ※サンプル グラバを正しいフィルタに確実に接続するには、フィルタ グラフを作成する前にこのメソッドを呼び出す。
					// majortype: http://msdn.microsoft.com/ja-jp/library/cc370108.aspx
					// subtype  : http://msdn.microsoft.com/ja-jp/library/cc371040.aspx
					{
						var grabber = (ISampleGrabber)videoGrabber;

						var mt = new AM_MEDIA_TYPE();
						mt.majortype = new Guid(GUID.MEDIATYPE_Video);
						mt.subtype = new Guid(GUID.MEDIASUBTYPE_RGB24);
						mt.formattype = new Guid(GUID.FORMAT_VideoInfo);
						grabber.SetMediaType(mt);
						grabber.SetBufferSamples(false);			// サンプルコピー 無効.
						grabber.SetOneShot(false);					// One Shot 無効.
						//grabber.SetCallback(videoGrabberCB, 0);	// 0:SampleCB メソッドを呼び出すよう指示する.
						grabber.SetCallback(videoGrabberCB, 1);		// 1:BufferCB メソッドを呼び出すよう指示する.
					}
				}
				#endregion

				#region 映像出力用: 保存する動画ファイル名を設定します.
				unsafe
				{
					HRESULT hr;

					// 動画ファイルを保存する設定:
					var filetype = new Guid(GUID.MEDIASUBTYPE_Asf);
					hr = (HRESULT)builder.SetOutputFileName(new IntPtr(&filetype), dst_filename, ref videoRenderer, ref fileSink);
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

					// フィルタの接続: (AVI 分離器)
					hr = (HRESULT)builder.RenderStream(IntPtr.Zero, IntPtr.Zero, videoSource, null, aviSplitter);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					// フィルタの接続: (映像入力)
					var mediatype_video = new Guid(GUID.MEDIATYPE_Video);
					hr = (HRESULT)builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype_video), aviSplitter, videoGrabber, videoRenderer);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					// フィルタの接続: (音声入力)
					var mediatype_audio = new Guid(GUID.MEDIATYPE_Audio);
					hr = (HRESULT)builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype_audio), aviSplitter, null, videoRenderer);
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

					// フレームサイズの取得:
					var vih = Axi.GetVideoInfo((ISampleGrabber)videoGrabber);
					var frameSize = new Size(vih.bmiHeader.biWidth, vih.bmiHeader.biHeight);

					// フレームサイズの変更:
					Axi.SetVideoFrameSize(config, frameSize);
				}
				#endregion

				#region DEBUG: GraphEdit ファイルを保存します.
				/*
				 * 現在のフィルタ構成を指定されたファイル(GRF 拡張子)に保存します。
				 * 保存されたファイルは graphedt.exe (Windws SDK 同梱) で確認できます。
				 */
				try
				{
					Axi.SaveGraphFile(graph, Path.GetFullPath(__FUNCTION__ + ".GRF"));
				}
				catch (System.Exception ex)
				{
					Console.WriteLine(ex.StackTrace);
				}
				#endregion

				// ------------------------------

				#region 取り込み処理:
				{
					var mediaControl = (IMediaControl)graph;
					var mediaEvent = (IMediaEvent)graph;
					var mediaSeeking = (IMediaSeeking)graph;

					var watch = new Stopwatch();
					watch.Start();

					// 再生.
					Console.WriteLine("Run ...");
					{
						HRESULT hr;
						int state;
						hr = (HRESULT)mediaControl.Run();
						hr = (HRESULT)mediaControl.GetState(1000, out state);
					}
					Console.WriteLine("Running ... {0:F3} msec", watch.Elapsed.TotalMilliseconds);

					// 再生が完了するまで待機する.
					{
						HRESULT hr;
						int code;
						hr = (HRESULT)mediaEvent.WaitForCompletion(-1, out code);
						hr = (HRESULT)mediaControl.Stop();
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

				if (aviSplitter != null)
					Marshal.ReleaseComObject(aviSplitter);
				aviSplitter = null;

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
