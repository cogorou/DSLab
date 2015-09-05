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
		/// WMV 形式のファイルを読み込む処理
		/// </summary>
		/// <remarks>
		///		RenderStream (NULL, MEDIATYPE_Video, source, videoGrabber, renderner)<br/>
		///		RenderStream (NULL, MEDIATYPE_Audio, source, audioGrabber, renderner)<br/>
		///		<pre>
		///		 source          grabber          mux        renderner
		///		+--------+     +---------+     +-------+     +-------+
		///		|  audio 0 ----0  audio  0 --- 1       0 --- 0       |
		///		|        |     +---------+     |       |     +-------+
		///		|        |                     |       |
		///		|        |     +---------+     |       |
		///		|  video 1 --- 0  video  0 --- 0       |
		///		+--------+     +---------+     |       |
		///		                               2       |
		///		                               +-------+
		///		</pre>
		/// </remarks>
		public static void Sample31()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			IGraphBuilder graph = null;
			ICaptureGraphBuilder2 builder = null;
			IBaseFilter videoSource = null;
			IBaseFilter videoGrabber = null;
			IBaseFilter audioGrabber = null;
			IBaseFilter videoRenderer = null;
			IBaseFilter audioRenderer = null;
			var videoGrabberCB = new CxSampleGrabberCB();
			var audioGrabberCB = new CxSampleGrabberCB();
			string src_filename = Path.Combine(TestFiles, "stopwatch_320x240.wmv");

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
#if true
					graph.AddSourceFilter(src_filename, "VideoSource", ref videoSource);
					if (videoSource == null)
						throw new System.IO.IOException("Failed to create a videoSource.");
#else
					videoSource = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_WMAsfReader);
					if (videoSource == null)
						throw new System.IO.IOException("Failed to create a videoSource.");
					graph.AddFilter(videoSource, "VideoSource");

					// Configure the file source filter.
					var pConfig = (IFileSourceFilter)videoSource;
					{
						HRESULT hr = (HRESULT)pConfig.Load(src_filename, IntPtr.Zero);
						if (hr < HRESULT.S_OK)
							throw new System.IO.IOException("Failed to set the src_filename.");
					}
#endif
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

				#region 音声捕獲用: サンプルグラバーを生成します.
				{
					audioGrabber = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_SampleGrabber);
					if (audioGrabber == null)
						throw new System.IO.IOException("Failed to create a audioGrabber.");
					graph.AddFilter(audioGrabber, "audioGrabber");

					// サンプルグラバフィルタの入力形式設定.
					// SetMediaType で必要なメディア タイプを指定します。
					//   http://msdn.microsoft.com/ja-jp/library/cc369546.aspx
					// ※AM_MEDIA_TYPE 構造体のメンバをすべて設定する必要はない。
					// ※デフォルトでは、サンプル グラバに優先メディア タイプはない。
					// ※サンプル グラバを正しいフィルタに確実に接続するには、フィルタ グラフを作成する前にこのメソッドを呼び出す。
					// majortype: http://msdn.microsoft.com/ja-jp/library/cc370108.aspx
					// subtype  : http://msdn.microsoft.com/ja-jp/library/cc371040.aspx
					{
						var grabber = (ISampleGrabber)audioGrabber;

						var mt = new AM_MEDIA_TYPE();
						mt.majortype = new Guid(GUID.MEDIATYPE_Audio);
						mt.subtype = new Guid(GUID.MEDIASUBTYPE_PCM);
						mt.formattype = new Guid(GUID.FORMAT_WaveFormatEx);
						grabber.SetMediaType(mt);
						grabber.SetBufferSamples(false);			// サンプルコピー 無効.
						grabber.SetOneShot(false);					// One Shot 無効.
						//grabber.SetCallback(audioGrabberCB, 0);	// 0:SampleCB メソッドを呼び出すよう指示する.
						grabber.SetCallback(audioGrabberCB, 1);		// 1:BufferCB メソッドを呼び出すよう指示する.
					}
				}
				#endregion

				#region 映像出力用: レンダラーを生成します.
				{
					videoRenderer = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_NullRenderer);
					if (videoRenderer == null)
						throw new System.IO.IOException("Failed to create a videoRenderer.");
					graph.AddFilter(videoRenderer, "videoRenderer");
				}
				#endregion

				#region 音声出力用: レンダラーを生成します.
				{
					audioRenderer = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_NullRenderer);
					if (audioRenderer == null)
						throw new System.IO.IOException("Failed to create a audioRenderer.");
					graph.AddFilter(audioRenderer, "audioRenderer");
				}
				#endregion

				#region フィルタの接続:
				unsafe
				{
					HRESULT hr;

					// フィルタの接続: (映像入力)
					var mediatype_video = new Guid(GUID.MEDIATYPE_Video);
					hr = (HRESULT)builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype_video), videoSource, videoGrabber, videoRenderer);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);

					// フィルタの接続: (音声入力)
					var mediatype_audio = new Guid(GUID.MEDIATYPE_Audio);
					hr = (HRESULT)builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype_audio), videoSource, audioGrabber, audioRenderer);
					if (hr < HRESULT.S_OK)
						throw new CxDSException(hr);
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

					// 映像サイズの取得.
					var vih = Axi.GetVideoInfo((ISampleGrabber)videoGrabber);
					var images = new List<Bitmap>();

					var watch = new Stopwatch();
					watch.Start();

					// 取り込み処理.
					videoGrabberCB.Notify += delegate(object _sender, CxSampleGrabberEventArgs _e)
					{
						Console.WriteLine("{0}: SampleTime={1:F6}", images.Count, _e.SampleTime);
						images.Add(_e.ToImage(vih));
					};

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

					// 確認用:
					Console.WriteLine("Save ... {0:F3} msec", watch.Elapsed.TotalMilliseconds);
					{
						string subdir = Path.Combine(Results, __FUNCTION__);
						if (Directory.Exists(subdir) == false)
							Directory.CreateDirectory(subdir);

						for (int i = 0; i < images.Count; i++)
						{
							var filename = string.Format("image{0}.png", i);
							images[i].Save(Path.Combine(subdir, filename));
						}
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

				if (videoGrabber != null)
					Marshal.ReleaseComObject(videoGrabber);
				videoGrabber = null;

				if (audioGrabber != null)
					Marshal.ReleaseComObject(audioGrabber);
				audioGrabber = null;

				if (videoRenderer != null)
					Marshal.ReleaseComObject(videoRenderer);
				videoRenderer = null;

				if (audioRenderer != null)
					Marshal.ReleaseComObject(audioRenderer);
				audioRenderer = null;

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
