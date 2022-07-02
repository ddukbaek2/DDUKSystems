using DagraacSystems;
using System;


namespace DagraacSystemsExample
{
	public class ConsoleLogger : ILogger
	{
		void ILogger.Log(string text)
		{
			Console.WriteLine(text);
		}

		void ILogger.LogWarning(string text)
		{
			Console.WriteLine(text);
		}

		void ILogger.LogError(string text)
		{
			Console.WriteLine(text);
		}

		void ILogger.LogException(Exception e)
		{
			Console.WriteLine(e.ToString());
		}
	}
}