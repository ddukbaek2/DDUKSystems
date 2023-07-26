using System;
using System.Collections.Generic;
using System.Reflection;


namespace DagraacSystems
{
    /// <summary>
    /// 이벤트 시스템.
    /// </summary>
    public class EventSystem : Component, IEventTarget
	{
		/// <summary>
		/// 수신자 별 모든 구독 정보.
		/// </summary>
		private Dictionary<IEventTarget, Dictionary<Type, List<MethodInfo>>> m_EventTargets;

		/// <summary>
		/// 송신자.
		/// </summary>
		public object Sender { private set; get; }

		/// <summary>
		/// 현재 적용중인 입력값.
		/// </summary>
		public IEventParameter EventParameter { private set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);

			m_EventTargets = new Dictionary<IEventTarget, Dictionary<Type, List<MethodInfo>>>();
			EventParameter = null;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			Clear();

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 수신자 등록.
		/// </summary>
		public void AddEventTarget(IEventTarget eventTarget)
		{
			if (eventTarget == null)
			{
				//Debug.LogError($"[Messenger] Listener is null.");
				return;
			}

			if (m_EventTargets.ContainsKey(eventTarget))
			{
				//Debug.LogError($"[Messenger] listener is exist.");
				return;
			}

			var subscriberType = eventTarget.GetType();
			var methods = subscriberType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			var eventTargetInfo = new Dictionary<Type, List<MethodInfo>>();

			foreach (var method in methods)
			{
				if (!method.IsDefined(typeof(EventAttribute)))
					continue;

				foreach (var attribute in method.GetCustomAttributes(typeof(EventAttribute)))
				{
					var subscribe = attribute as EventAttribute;

					if (subscribe.Type == null)
					{
						//Debug.LogError($"[Messenger] Listen Attribute Parameter is null.");
						continue;
					}

					if (subscribe.Type.IsSubclassOf(typeof(IEventParameter)))
					{
						//Debug.LogError($"[Messenger] Not Inherit IEventParameter Listen={listen.Type.FullName}");
						continue;
					}

					if (!eventTargetInfo.TryGetValue(subscribe.Type, out var list))
					{
						list = new List<MethodInfo>();
					}

					list.Add(method);
					eventTargetInfo.Add(subscribe.Type, list);
				}
			}

			m_EventTargets.Add(eventTarget, eventTargetInfo);
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public void Remove(IEventTarget eventTarget)
		{
			m_EventTargets.Remove(eventTarget);
		}

		/// <summary>
		/// 전체 제거.
		/// </summary>
		public void Clear()
		{
			m_EventTargets.Clear();
		}

		/// <summary>
		/// 특정 리스너에게 통지.
		/// </summary>
		public void Send(IEventTarget eventTarget, IEventParameter eventParameter)
		{
			Send(null, eventTarget, eventParameter);
		}

		/// <summary>
		/// 특정 리스너에게 통지.
		/// </summary>
		public void Send(object sender, IEventTarget eventTarget, IEventParameter eventParameter)
		{
			if (eventTarget == null)
			{
				//Debug.LogError($"[Messenger] listener is null.");
				return;
			}

			if (eventParameter == null)
			{
				//Debug.LogError($"[Messenger] eventParameter is null.");
				return;
			}

			var messageType = eventParameter.GetType();

			if (!m_EventTargets.TryGetValue(eventTarget, out var subscriberInfo))
			{
				//Debug.LogError($"[Messenger] not found listenerInfo.");
				return;
			}

			if (!subscriberInfo.TryGetValue(messageType, out var methods))
			{
				// 모든 리스너가 모든 이벤트 메소드를 가지고 있는 것은 아니기에 오류를 찍지 않음.
				////Debug.LogError($"[Messenger] not found methods.");
				return;
			}

			try
			{
				foreach (var method in methods)
				{
					Sender = sender;
					EventParameter = eventParameter;

					var parameters = method.GetParameters();
					switch (parameters.Length)
					{
						case 2:
							method.Invoke(eventTarget, new object[] { sender, eventParameter }); // 메시지를 인자로 삼는 메서드의 경우.
							break;
						case 1:
							method.Invoke(eventTarget, new object[] { eventParameter }); // 메시지를 인자로 삼는 메서드의 경우.
							break;
						case 0:
							method.Invoke(eventTarget, null); // 메시지와 동일한 이름의 메서드.
							break;
					}
				}

				Sender = null;
				EventParameter = null;
			}
			catch// (Exception e)
			{
			}
		}

		/// <summary>
		/// 전체 리스너에게 통지.
		/// </summary>
		public void Notify(IEventParameter eventParameter)
		{
			Notify(null, eventParameter);
		}

		/// <summary>
		/// 전체 리스너에게 통지.
		/// </summary>
		public void Notify(object sender, IEventParameter eventParameter)
		{
			if (eventParameter == null)
			{
				//Debug.LogError($"[Messenger] eventParameter is null.");
				return;
			}

			Sender = sender;

			foreach (var eventTarget in m_EventTargets.Keys)
			{
				// 수신부 중 하나에서 죽을 경우를 대비한 방어 처리.
				try
				{
					Send(eventTarget, eventParameter);
				}
				catch// (Exception e)
				{
				}
			}

			Sender = null;
			EventParameter = null;
		}
	}
}