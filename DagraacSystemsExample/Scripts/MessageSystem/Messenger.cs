using System;


namespace DagraacSystemsExample
{
	public class NotificationType
	{
		public delegate void OnTest();
	}

	public class MessageSystem : DagraacSystems.MessageSystem
	{
		private static readonly Lazy<MessageSystem> _instance = new Lazy<MessageSystem>(() => new MessageSystem(), true); // thread-safe.
		public static MessageSystem Instance => _instance.Value;
	}
}