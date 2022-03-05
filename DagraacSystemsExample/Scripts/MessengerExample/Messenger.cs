using System;


namespace DagraacSystemsExample
{
	public class NotificationType
	{
		public delegate void OnTest();
	}

	public class Messenger : DagraacSystems.Messenger
	{
		private static readonly Lazy<Messenger> m_Instance = new Lazy<Messenger>(() => new Messenger(), true); // thread-safe.
		public static Messenger Instance => m_Instance.Value;
	}
}