using System;


namespace DagraacSystemsExample
{
	public class NotificationType
	{
		public delegate void OnTest();
	}

	public class Messenger : DagraacSystems.MessageSystem
	{
		private static readonly Lazy<Messenger> _instance = new Lazy<Messenger>(() => new Messenger(), true); // thread-safe.
		public static Messenger Instance => _instance.Value;
	}
}