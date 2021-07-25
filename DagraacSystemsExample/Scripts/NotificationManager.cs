using System;


namespace DagraacSystemsExample
{
	public class NotificationManager : DagraacSystems.Notification
	{
		private static readonly Lazy<NotificationManager> m_Instance = new Lazy<NotificationManager>(() => new NotificationManager(), true); // thread-safe.
		public static NotificationManager Instance => m_Instance.Value;
	}

	public class DefinedDelegate
	{
		public delegate void OnCreate();
	}

	public class ExampleObject
	{
		public ExampleObject()
		{
			NotificationManager.Instance.Register<DefinedDelegate.OnCreate>(OnCreate);
		}

		private void OnCreate()
		{
			Console.WriteLine("OnCreate()");
		}
	}
}