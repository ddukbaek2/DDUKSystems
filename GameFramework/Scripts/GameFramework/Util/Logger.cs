using System;


namespace GameFramework
{
	public static class Logger
	{
		public static Action<string> OnLogEvent;
		public static Action<string> OnLogWarningEvent;
		public static Action<string> OnLogErrorEvent;

		/// <summary>
		/// 생성됨.
		/// </summary>
		static Logger()
		{
			OnLogEvent = null;
			OnLogWarningEvent = null;
			OnLogErrorEvent = null;
		}

		public static void Log(string format, params object[] args)
		{
			OnLogEvent?.Invoke(string.Format(format, args));
		}

		public static void LogError(string format, params object[] args)
		{
			OnLogWarningEvent?.Invoke(string.Format(format, args));
		}

		public static void LogWarning(string format, params object[] args)
		{
			OnLogErrorEvent?.Invoke(string.Format(format, args));
		}
	}
}