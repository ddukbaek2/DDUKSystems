using System;


namespace DagraacSystemsExample
{
	public class NotificationType
	{
		public delegate void OnTest();
	}

	public class MessageBroker : DagraacSystems.MessageBroker
	{
		private static readonly Lazy<MessageBroker> m_Instance = new Lazy<MessageBroker>(() => new MessageBroker(), true); // thread-safe.
		public static MessageBroker Instance => m_Instance.Value;
	}
}