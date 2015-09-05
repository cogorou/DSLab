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

namespace DSLab
{
	/// <summary>
	/// 例外クラス (DirectShow 関連)
	/// </summary>
	public class CxDSException : Exception
	{
		#region プロパティ
		/// <summary>
		/// COM のエラーコード
		/// </summary>
		/// <remarks>
		/// COM 例外の場合、そのエラーコードが取得できます。
		/// </remarks>
		public DSLab.HRESULT ComError
		{
			get { return m_ComError; }
		}
		private DSLab.HRESULT m_ComError = DSLab.HRESULT.E_FAIL;

		#endregion

		#region コンストラクタ.

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ex">元になる例外</param>
		public CxDSException(Exception ex)
			: base(CreateMessage(ex), CreateInnerException(ex))
		{
			if (ex is COMException)
				m_ComError = (DSLab.HRESULT)((ex as COMException).ErrorCode);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="code">エラーコード</param>
		public CxDSException(HRESULT code)
			: base(GetDirectShowMessage(code))
		{
			m_ComError = code;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="param">メッセージ引数</param>
		public CxDSException(string message, params object[] param)
			: base(CreateMessage(message, param))
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ex">元になる例外</param>
		/// <param name="message">メッセージ</param>
		/// <param name="param">メッセージ引数</param>
		public CxDSException(Exception ex, string message, params object[] param)
			: base(CreateMessage(ex, message, param), CreateInnerException(ex))
		{
			if (ex is COMException)
				m_ComError = (DSLab.HRESULT)((ex as COMException).ErrorCode);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ex">元になるCOMException例外</param>
		/// <param name="message">メッセージ</param>
		/// <param name="param">メッセージ引数</param>
		public CxDSException(COMException ex, string message, params object[] param)
			: base(CreateMessage(message, param), ex)
		{
			m_ComError = (DSLab.HRESULT)ex.ErrorCode;
		}

		#endregion

		#region メッセージ生成.

		/// <summary>
		/// メッセージ生成
		/// </summary>
		/// <param name="ex">例外オブジェクト</param>
		/// <returns>
		///		生成されたメッセージを返します。
		///	</returns>
		private static string CreateMessage(Exception ex)
		{
			if (ex is COMException)
			{
				return GetDirectShowMessage(((COMException)ex).ErrorCode) + " @ " + ex.Message;
			}
			return ex.Message;
		}

		/// <summary>
		/// メッセージ生成
		/// </summary>
		/// <param name="format">書式</param>
		/// <param name="param">引数</param>
		/// <returns>
		///		生成されたメッセージを返します。
		///	</returns>
		private static string CreateMessage(string format, object[] param)
		{
			try
			{
				return string.Format(format, param);
			}
			catch (Exception)
			{
				format = format.Replace("{", "{{");
				format = format.Replace("}", "}}");
				return string.Format(format, param);
			}
		}

		/// <summary>
		/// メッセージ生成
		/// </summary>
		/// <param name="ex">例外オブジェクト</param>
		/// <param name="format">書式</param>
		/// <param name="param">引数</param>
		/// <returns>
		///		生成されたメッセージを返します。
		///	</returns>
		private static string CreateMessage(Exception ex, string format, object[] param)
		{
			return CreateMessage(format, param) + " @ " + CreateMessage(ex);
		}

		/// <summary>
		/// 例外メッセージの補足
		/// </summary>
		/// <param name="ex">元の例外</param>
		/// <returns>
		///		COMException の場合は、メッセージに追加情報を付加します。
		///	</returns>
		private static Exception CreateInnerException(Exception ex)
		{
			if (ex is COMException)
			{
				// COMException取得.
				COMException comex = (COMException)ex;

				// メッセージ.
				string msg = DSLab.CxDSException.GetDirectShowMessage(comex.ErrorCode);

				// 対象メソッド.
				msg += " @ " + comex.TargetSite.ToString();

				return new CxDSException(comex, msg);
			}
			return ex;
		}

		#endregion

		#region メッセージ取得.

		/// <summary>
		/// DirectShow関連のエラーメッセージ取得
		/// </summary>
		/// <param name="code">リターンコード</param>
		/// <returns>
		///		リターンコードに対応したメッセージを返します。
		/// </returns>
		public static string GetDirectShowMessage(int code)
		{
			return GetDirectShowMessage((DSLab.HRESULT)code);
		}

		/// <summary>
		/// DirectShow関連のエラーメッセージ取得
		/// </summary>
		/// <param name="code">リターンコード</param>
		/// <returns>
		///		リターンコードに対応したメッセージを返します。
		///	</returns>
		public static string GetDirectShowMessage(DSLab.HRESULT code)
		{
			return String.Format("{0} ({1:x8})", code, (int)code);
		}

		#endregion
	}
}
