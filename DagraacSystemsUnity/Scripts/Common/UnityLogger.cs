using DagraacSystems.Log;
using System;

namespace DagraacSystems.Unity
{
	public class UnityLogger : ILogger
	{
		public Action<string> OnWrite { set; get; } = null;

		public void Write(string text)
		{
			UnityEngine.Debug.Log(text);
			OnWrite?.Invoke(text);
		}
	}
}