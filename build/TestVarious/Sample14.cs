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
		/// カメラ制御とビデオ品質制御
		/// </summary>
		public static void Sample14()
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

				#region カメラ制御:
				Console.WriteLine("");
				Console.WriteLine("CameraControlProperty");
				{
					#region 取得:
					var watch = new Stopwatch();
					watch.Start();
					var ranges = new Dictionary<CameraControlProperty, TxPropertyRange>();
					var control = Util.GetInterface<IAMCameraControl>(graph);
					foreach (CameraControlProperty prop in Enum.GetValues(typeof(CameraControlProperty)))
					{
						HRESULT hr;
						var range = TxPropertyRange.Reset();

						try
						{
							// レンジの取得.
							hr = (HRESULT)control.GetRange(
									prop,
									ref range.Min,
									ref range.Max,
									ref range.SteppingDelta,
									ref range.DefaultValue,
									ref range.CapsFlag
								);
							if (hr < HRESULT.S_OK)
								continue;

							// 現在値の取得.
							hr = (HRESULT)control.Get(
									prop,
									ref range.Value,
									ref range.Flag
								);
							if (hr < HRESULT.S_OK)
								continue;

							range.IsSupported = true;
						}
						catch (System.Exception)
						{
						}
						finally
						{
							ranges[prop] = range;
						}
					}
					Console.WriteLine("Completed. {0:F3} msec", watch.Elapsed.TotalMilliseconds);
					#endregion

					#region 視覚的な確認の為の出力:
					Console.WriteLine("--- {0,-22}: {1,5}, {2,5}, {3,6}, {4,4}, {5,5} ~ {6,5}",
						"Name", "Value", "Def", "Flag", "Step", "Lower", "Upper");

					foreach (var item in ranges)
					{
						var name = item.Key.ToString();
						var range = item.Value;
						if (range.IsSupported)
						{
							Console.WriteLine("[O] {0,-22}: {1,5}, {2,5}, 0x{3:X4}, {4,4}, {5,5} ~ {6,5}",
									name,
									range.Value,
									range.DefaultValue,
									range.CapsFlag,
									range.SteppingDelta,
									range.Min,
									range.Max
								);
						}
						else
						{
							Console.WriteLine("[-] {0,-22}:", name);
						}
					}
					#endregion
				}
				#endregion

				#region ビデオ品質制御:
				Console.WriteLine("");
				Console.WriteLine("VideoProcAmpProperty");
				{
					#region 取得:
					var watch = new Stopwatch();
					watch.Start();
					var ranges = new Dictionary<VideoProcAmpProperty, TxPropertyRange>();
					var control = Util.GetInterface<IAMVideoProcAmp>(graph);
					foreach (VideoProcAmpProperty prop in Enum.GetValues(typeof(VideoProcAmpProperty)))
					{
						HRESULT hr;
						var range = TxPropertyRange.Reset();

						try
						{
							// レンジ.
							hr = (HRESULT)control.GetRange(
									prop,
									ref range.Min,
									ref range.Max,
									ref range.SteppingDelta,
									ref range.DefaultValue,
									ref range.CapsFlag
								);
							if (hr >= HRESULT.S_OK)
							{
								// 現在値.
								hr = (HRESULT)control.Get(
										prop,
										ref range.Value,
										ref range.Flag
									);
								if (hr >= HRESULT.S_OK)
								{
									range.IsSupported = true;
								}
							}
						}
						catch (System.Exception)
						{
						}

						ranges[prop] = range;
					}
					Console.WriteLine("Completed. {0:F3} msec", watch.Elapsed.TotalMilliseconds);
					#endregion

					#region 視覚的な確認の為の出力:
					Console.WriteLine("--- {0,-22}: {1,5}, {2,5}, {3,6}, {4,4}, {5,5} ~ {6,5}",
						"Name", "Value", "Def", "Flag", "Step", "Lower", "Upper");

					foreach (var item in ranges)
					{
						var name = item.Key.ToString();
						var range = item.Value;
						if (range.IsSupported)
						{
							Console.WriteLine("[O] {0,-22}: {1,5}, {2,5}, 0x{3:X4}, {4,4}, {5,5} ~ {6,5}",
									name,
									range.Value,
									range.DefaultValue,
									range.CapsFlag,
									range.SteppingDelta,
									range.Min,
									range.Max
								);
						}
						else
						{
							Console.WriteLine("[-] {0,-22}:", name);
						}
					}
					#endregion
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

	#region プロパティレンジ構造体:

	/// <summary>
	/// プロパティレンジ構造体
	/// </summary>
	struct TxPropertyRange
	{
		public bool IsSupported;
		public int Min;
		public int Max;
		public int SteppingDelta;
		public int DefaultValue;
		public int CapsFlag;
		public int Value;
		public int Flag;

		/// <summary>
		/// 初期値
		/// </summary>
		/// <returns>
		///		初期値を格納して返します。
		/// </returns>
		public static TxPropertyRange Reset()
		{
			var result = new TxPropertyRange();
			result.IsSupported = false;
			result.Min = 0;
			result.Max = 0;
			result.SteppingDelta = 0;
			result.DefaultValue = 0;
			result.CapsFlag = 0;
			result.Value = 0;
			result.Flag = 0;
			return result;
		}
	}

	#endregion
}
