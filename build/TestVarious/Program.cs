/*
	XIA -Imaging Auxiliary-
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;

namespace demo
{
	partial class Program
	{
		/// <summary>
		/// エントリポイント
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			try
			{
				if (Directory.Exists(Results) == false)
					Directory.CreateDirectory(Results);

				// 接続されているデバイスの一覧:
				Sample01();

				// カメラの接続と画像取り込み:
				Sample11();
				Sample12();
				Sample13();
				Sample14();

				// AVI 動画の再生:
				Sample21();
				Sample22();

				// WMV 動画の再生:
				Sample31();
				Sample32();
			}
			catch (System.Exception ex)
			{
				Console.WriteLine("{0}", ex.StackTrace);
			}
			finally
			{
			}
		}

		/// <summary>
		/// テストファイルディレクトリ
		/// </summary>
		static string TestFiles = "TestFiles";

		/// <summary>
		/// 処理結果ディレクトリ
		/// </summary>
		static string Results = "Results";
	}
}
