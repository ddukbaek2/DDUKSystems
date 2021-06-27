using System;
using System.Collections.Generic;


namespace DagraacSystems.Notification
{
	public class NotificationManagerTemplete<TNotificationManager, TEventID, TTargetID> : Manager<TNotificationManager> 
		where TNotificationManager : NotificationManagerTemplete<TNotificationManager, TEventID, TTargetID>, new()
		where TEventID : Enum
	{
		internal class NotificationEvent
		{
			private Dictionary<TTargetID, Delegate> m_Targets;

			public NotificationEvent()
			{
				m_Targets = new Dictionary<TTargetID, Delegate>();
			}

			public void Add(TTargetID targetID, Delegate handler)
			{
				if (handler == null)
					return;

				if (m_Targets.ContainsKey(targetID))
				{
					m_Targets[targetID] = Delegate.Combine(m_Targets[targetID], handler);
				}
				else
				{
					m_Targets.Add(targetID, handler);
				}
			}

			public void Remove(TTargetID key)
			{
				m_Targets.Remove(key);
			}

			public void Remove(TTargetID key, Delegate handler)
			{
				if (handler == null)
					return;

				if (m_Targets[key] != null)
				{
					var del = Delegate.Remove(m_Targets[key], handler);

					if (del != null)
					{
						var list = del.GetInvocationList();
						if (list.Length > 0)
						{
							m_Targets[key] = del;
							return;
						}
					}

					Remove(key);
				}
			}

			public void Clear()
			{
				m_Targets.Clear();
			}

			public void SendMessage(TTargetID targetID, params object[] parameters)
			{
				if (!m_Targets.ContainsKey(targetID))
					return;

				m_Targets[targetID].DynamicInvoke(parameters);
			}

			public void NotifyMessage(params object[] parameters)
			{
				var enumerator = m_Targets.GetEnumerator();
				while (enumerator.MoveNext())
				{
					var target = enumerator.Current;
					m_Targets[target.Key].DynamicInvoke(parameters);
				}

				//foreach (TKey key in handlerList.Keys)
				//	handlerList[key].DynamicInvoke(parameters);
			}
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

		public void Add(TEventID eventID, TTargetID targetID, Delegate handler)
		{
			m_Events[eventID].Add(targetID, handler);
		}

		public void Remove(TEventID eventID, TTargetID targetID)
		{
			m_Events[eventID].Remove(targetID);
		}

		public void Remove(TEventID eventID, TTargetID targetID, System.Delegate handler)
		{
			m_Events[eventID].Remove(targetID, handler);
		}

		public void RemoveTargetAll(TTargetID targetID)
		{
			foreach (TEventID eventID in Enum.GetValues(typeof(TEventID)))
				m_Events[eventID].Remove(targetID);
		}

		public void SendMessage(TEventID eventID, TTargetID targetUniqueID)
		{
			m_Events[eventID].SendMessage(targetUniqueID, null);
		}

		public void SendMessage(TEventID eventID, TTargetID targetUniqueID, params object[] parameters)
		{
			m_Events[eventID].SendMessage(targetUniqueID, parameters);
		}

		public void NotifyMessage(TEventID eventID)
		{
			m_Events[eventID].NotifyMessage(null);
		}

		public void NotifyMessage(TEventID eventID, params object[] parameters)
		{
			m_Events[eventID].NotifyMessage(parameters);
		}
	}
}