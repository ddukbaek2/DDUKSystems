using System;


namespace DagraacSystemsExample
{
	public class NotificationType
	{
		public delegate void OnTest();
	}

	public class NotificationManager : DagraacSystems.Notification
	{
		private static readonly Lazy<NotificationManager> m_Instance = new Lazy<NotificationManager>(() => new NotificationManager(), true); // thread-safe.
		public static NotificationManager Instance => m_Instance.Value;
	}
}