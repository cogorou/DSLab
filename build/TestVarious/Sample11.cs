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
		/// カメラから画像を取り込む処理
		/// </summary>
		/// <remarks>
		///		この方法が最も単純です。<br/>
		///		<br/>
		///		RenderStream (NULL, MEDIATYPE_Video, source, grabber, renderner)<br/>
		///		<pre>
		///		 source        grabber       renderer
		///		+-------+     +-------+     +-------+
		///		|       0 --- 0       0 --- 0       |
		///		|       1     |       |     |       |
		///		+-------+     +-------+     +-------+
		///		</pre>
		/// </remarks>
		public static void Sample11()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			IGraphBuilder graph = null;
			ICaptureGraphBuilder2 builder = null;
			IBaseFilter videoSource = null;
			IBaseFilter videoGrabber = null;
			IBaseFilter videoRenderer = null;
			var videoGrabberCB = new CxSampleGrabberCB();

			try
			{
				#region グラフビルダーの生成:
				{
					// 必須:
					graph = (IGraphBuilder)Axi.CoCreateInstance(GUID.CLSID_FilterGraph);
					if (graph == null)
						throw new System.IO.IOException("Failed to create a GraphBuilder.");

					// 任意: 当例に於いては必須ではありませんが、後述のフィルタ接続を簡潔に記述できます.
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
					Axi.SetFormatSize(pin, 640, 480);
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

				#region 映像出力用: レンダラーを生成します.
				{
					videoRenderer = (IBaseFilter)Axi.CoCreateInstance(GUID.CLSID_NullRenderer);
					if (videoRenderer == null)
						throw new System.IO.IOException("Failed to create a videoRenderer.");
					graph.AddFilter(videoRenderer, "videoRenderer");
				}
				#endregion

				#region フィルタの接続:
				if (builder != null)
				{
					unsafe
					{
						var mediatype = new Guid(GUID.MEDIATYPE_Video);
						var hr = (HRESULT)builder.RenderStream(IntPtr.Zero, new IntPtr(&mediatype), videoSource, videoGrabber, videoRenderer);
						if (hr < HRESULT.S_OK)
							throw new CxDSException(hr);
					}
				}
				else
				{
					// ピンの取得.
					IPin videoSourceOutput = Axi.FindPin(videoSource, 0, PIN_DIRECTION.PINDIR_OUTPUT);
					IPin videoGrabberInput = Axi.FindPin(videoGrabber, 0, PIN_DIRECTION.PINDIR_INPUT);
					IPin videoGrabberOutput = Axi.FindPin(videoGrabber, 0, PIN_DIRECTION.PINDIR_OUTPUT);
					IPin videoRendererInput = Axi.FindPin(videoRenderer, 0, PIN_DIRECTION.PINDIR_INPUT);

					// ピンの接続.
					graph.Connect(videoSourceOutput, videoGrabberInput);
					graph.Connect(videoGrabberOutput, videoRendererInput);
				}
				#endregion

				// ------------------------------

				#region 取り込み処理:
				{
					var mediaControl = (IMediaControl)graph;

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
					while (watch.ElapsedMilliseconds < 3000)
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

				if (videoRenderer != null)
					Marshal.ReleaseComObject(videoRenderer);
				videoRenderer = null;

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
