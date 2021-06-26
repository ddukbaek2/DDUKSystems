using System;
using System.Collections.Generic;


namespace DagraacSystems.Notification
{
	public enum ExampleNotificationID : int
	{
		Test,
	}

	public class NotificationManagerTemplete<TNotificationManager, TEventID, TTargetID> : Manager<TNotificationManager> 
		where TNotificationManager : NotificationManagerTemplete<TNotificationManager, TEventID, TTargetID>, new()
		where TEventID : Enum
	{
		internal class NotificationEvent
		{
			private Dictionary<TTargetID, Delegate> m_Handlers;
		}


		private Dictionary<TEventID, NotificationEvent> m_Events;

		public NotificationManagerTemplete()
		{
			m_Events = new Dictionary<TEventID, NotificationEvent>();
			Clear();
		}

		protected override void OnDispose(bool disposing)
		{
		}

		public void Clear()
		{
			m_Events.Clear();
			foreach (TEventID eventID in Enum.GetValues(typeof(TEventID)))
				m_Events.Add(eventID, new NotificationEvent());
		}

		public void NotifyMessage(string group, params object[] args)
		{
		}

		public void SendMessage(string id, params object[] args)
		{
		}
	}
}