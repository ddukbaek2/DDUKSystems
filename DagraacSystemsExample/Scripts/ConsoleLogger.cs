using System;


namespace DagraacSystemsExample
{
	public class ConsoleLogger : DagraacSystems.ILogger
	{
		public Action<string> OnWrite { set; get; }

		public void Log(string text)
		{
			Console.WriteLine(text);
		}
	}
}