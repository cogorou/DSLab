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
		/// 接続されているデバイスを列挙する処理
		/// </summary>
		/// <remarks>
		///		<pre>
		///		           in    filter   out 
		///		+--------+     +--------+     +--------+
		///		|        | --- 0        0 --- |        |
		///		+--------+     |        |     +--------+
		///		               |        |     +--------+
		///		               |        1 --- |        |
		///		               +--------+     +--------+
		///		</pre>
		/// </remarks>
		public static void Sample01()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			try
			{
				#region 映像入力デバイスの一覧: (カメラ等)
				{
					var category = new Guid(GUID.CLSID_VideoInputDeviceCategory);
					var filterInfos = Util.GetFilterList(category);

					Console.WriteLine("Video Input Devices ({0})", filterInfos.Count);
					for (int iii = 0; iii < filterInfos.Count; iii++)
					{
						// フィルタ情報:
						var filterInfo = filterInfos[iii];
						Console.WriteLine("|- {0}", iii);
						Console.WriteLine("|  |- {0}", filterInfo.Name);
						Console.WriteLine("|  |- {0} ({1})", filterInfo.CLSID, filterInfo.Index);

						IBaseFilter filter = null;
						try
						{
							filter = Axi.CreateFilter(category, filterInfo.CLSID, filterInfo.Index);

							#region ピン情報:
							var pinInfos = Util.GetPinList(filter);
							int outpin_num = 0;
							Console.WriteLine("|  |- {0} ({1})", "Pins", pinInfos.Count);
							for (int ppp = 0; ppp < pinInfos.Count; ppp++)
							{
								var pinInfo = pinInfos[ppp];
								Console.WriteLine("|  |  |- {0}: {1,-15} = {2}", ppp, pinInfo.Direction, pinInfo.Name);

								if (pinInfo.Direction == PIN_DIRECTION.PINDIR_OUTPUT)
									outpin_num++;
							}
							#endregion

							#region フォーマット情報: (※出力ピンに限定しています)
							Console.WriteLine("|  |  |- {0}", "Outpin Details");
							for (int outpin_index = 0; outpin_index < outpin_num; outpin_index++)
							{
								IPin pin = null;
								try
								{
									// 出力ピンからフォーマット情報を抽出します
									pin = Axi.FindPin(filter, outpin_index, PIN_DIRECTION.PINDIR_OUTPUT);
									var formatInfos = Util.GetFormatList(pin);

									// 映像の情報のみ抽出します.
									var videoInfos = formatInfos.FindAll(
										delegate(CxFormatInfo item)
										{
											return (GUID.Compare(item.FormatType, GUID.FORMAT_VideoInfo));
										});

									Console.WriteLine("|  |  |  |- {0}[{1}]: {2} {3}", "Outpin", outpin_index, videoInfos.Count, "formats");
									for (int fff = 0; fff < videoInfos.Count; fff++)
									{
										var videoInfo = videoInfos[fff];
										Console.WriteLine("|  |  |  |  |- {0,2}: {1,4},{2,4}", fff, videoInfo.VideoSize.Width, videoInfo.VideoSize.Height);
									}
								}
								catch (System.Exception ex)
								{
									Console.WriteLine("{0}", ex.StackTrace);
								}
								finally
								{
									if (pin != null)
										Marshal.ReleaseComObject(pin);
									pin = null;
								}
							}
							#endregion
						}
						catch (System.Exception ex)
						{
							Console.WriteLine("{0}", ex.StackTrace);
						}
						finally
						{
							if (filter != null)
								Marshal.ReleaseComObject(filter);
							filter = null;
						}
					}
				}
				#endregion

				#region 音声入力デバイスの一覧: (マイク等)
				{
					var category = new Guid(GUID.CLSID_AudioInputDeviceCategory);
					var filterInfos = Util.GetFilterList(category);

					Console.WriteLine("Audio Input Devices ({0})", filterInfos.Count);
					for (int iii = 0; iii < filterInfos.Count; iii++)
					{
						// フィルタ情報:
						var filterInfo = filterInfos[iii];
						Console.WriteLine("|- {0}", iii);
						Console.WriteLine("|  |- {0}", filterInfo.Name);
						Console.WriteLine("|  |- {0} ({1})", filterInfo.CLSID, filterInfo.Index);

						IBaseFilter filter = null;
						try
						{
							filter = Axi.CreateFilter(category, filterInfo.CLSID, filterInfo.Index);

							#region ピン情報:
							var pinInfos = Util.GetPinList(filter);
							Console.WriteLine("|  |- {0} ({1})", "Pins", pinInfos.Count);
							for (int ppp = 0; ppp < pinInfos.Count; ppp++)
							{
								var pinInfo = pinInfos[ppp];
								Console.WriteLine("|  |  |- {0}: {1,-15} = {2}", ppp, pinInfo.Direction, pinInfo.Name);
							}
							#endregion
						}
						catch (System.Exception ex)
						{
							Console.WriteLine("{0}", ex.StackTrace);
						}
						finally
						{
							if (filter != null)
								Marshal.ReleaseComObject(filter);
							filter = null;
						}
					}
				}
				#endregion

				#region 音声出力デバイスの一覧: (スピーカー等)
				{
					var category = new Guid(GUID.CLSID_AudioRendererCategory);
					var filterInfos = Util.GetFilterList(category);

					Console.WriteLine("Audio Output Devices ({0})", filterInfos.Count);
					for (int iii = 0; iii < filterInfos.Count; iii++)
					{
						// フィルタ情報:
						var filterInfo = filterInfos[iii];
						Console.WriteLine("|- {0}", iii);
						Console.WriteLine("|  |- {0}", filterInfo.Name);
						Console.WriteLine("|  |- {0} ({1})", filterInfo.CLSID, filterInfo.Index);

						IBaseFilter filter = null;
						try
						{
							filter = Axi.CreateFilter(category, filterInfo.CLSID, filterInfo.Index);

							#region ピン情報:
							var pinInfos = Util.GetPinList(filter);
							Console.WriteLine("|  |- {0} ({1})", "Pins", pinInfos.Count);
							for (int ppp = 0; ppp < pinInfos.Count; ppp++)
							{
								var pinInfo = pinInfos[ppp];
								Console.WriteLine("|  |  |- {0}: {1,-15} = {2}", ppp, pinInfo.Direction, pinInfo.Name);
							}
							#endregion
						}
						catch (System.Exception ex)
						{
							Console.WriteLine("{0}", ex.StackTrace);
						}
						finally
						{
							if (filter != null)
								Marshal.ReleaseComObject(filter);
							filter = null;
						}
					}
				}
				#endregion
			}
			catch (System.Exception ex)
			{
				Console.WriteLine("{0}", ex.StackTrace);
			}
			finally
			{
			}
		}
	}
}
