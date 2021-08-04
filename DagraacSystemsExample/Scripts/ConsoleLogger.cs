using System;


namespace DagraacSystemsExample
{
	public class ConsoleLogger : DagraacSystems.Log.ILogger
	{
		public Action<string> OnWrite { set; get; }

		public void Write(string text)
		{
			Console.WriteLine(text);
		}
	}
}