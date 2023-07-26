using System;
using System.Collections.Generic;
using System.Reflection;


namespace DagraacSystems
{
    /// <summary>
    /// 메시지 송신 처리기.
    /// </summary>
    public class MessageSystem : DisposableObject, IMessageTarget
	{
		/// <summary>
		/// 구독자 별 모든 구독 정보.
		/// </summary>
		private Dictionary<IMessageTarget, Dictionary<Type, List<MethodInfo>>> messageTargets;

		/// <summary>
		/// 송신자.
		/// </summary>
		public object Sender { private set; get; }

		/// <summary>
		/// 현재 적용중인 메시지.
		/// </summary>
		public IMessage CurrentMessage { private set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public MessageSystem() : base()
		{
			messageTargets = new Dictionary<IMessageTarget, Dictionary<Type, List<MethodInfo>>>();
			CurrentMessage = null;
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			Clear();

			base.OnDispose(explicitedDispose);
		}

		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		/// <summary>
		/// 수신자 등록.
		/// </summary>
		public void Add(IMessageTarget subscriber)
		{
			if (subscriber == null)
			{
				//Debug.LogError($"[Messenger] Listener is null.");
				return;
			}

			if (messageTargets.ContainsKey(subscriber))
			{
				//Debug.LogError($"[Messenger] listener is exist.");
				return;
			}

			var subscriberType = subscriber.GetType();
			var methods = subscriberType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			var subscriberInfo = new Dictionary<Type, List<MethodInfo>>();

			foreach (var method in methods)
			{
				if (!method.IsDefined(typeof(MessageAttribute)))
					continue;

				foreach (var attribute in method.GetCustomAttributes(typeof(MessageAttribute)))
				{
					var subscribe = attribute as MessageAttribute;

					if (subscribe.Type == null)
					{
						//Debug.LogError($"[Messenger] Listen Attribute Parameter is null.");
						continue;
					}

					if (subscribe.Type.IsSubclassOf(typeof(IMessage)))
					{
						//Debug.LogError($"[Messenger] Not Inherit IMessage Listen={listen.Type.FullName}");
						continue;
					}

					if (!subscriberInfo.TryGetValue(subscribe.Type, out var list))
					{
						list = new List<MethodInfo>();
					}

					list.Add(method);
					subscriberInfo.Add(subscribe.Type, list);
				}
			}

			messageTargets.Add(subscriber, subscriberInfo);
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public void Remove(IMessageTarget subscriber)
		{
			messageTargets.Remove(subscriber);
		}

		/// <summary>
		/// 전체 제거.
		/// </summary>
		public void Clear()
		{
			messageTargets.Clear();
		}

		/// <summary>
		/// 특정 리스너에게 통지.
		/// </summary>
		public void Send(IMessageTarget subscriber, IMessage message)
		{
			Send(null, subscriber, message);
		}

		/// <summary>
		/// 특정 리스너에게 통지.
		/// </summary>
		public void Send(object sender, IMessageTarget subscriber, IMessage message)
		{
			if (subscriber == null)
			{
				//Debug.LogError($"[Messenger] listener is null.");
				return;
			}

			if (message == null)
			{
				//Debug.LogError($"[Messenger] message is null.");
				return;
			}

			var messageType = message.GetType();

			if (!messageTargets.TryGetValue(subscriber, out var subscriberInfo))
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
					CurrentMessage = message;

					var parameters = method.GetParameters();
					switch (parameters.Length)
					{
						case 2:
							method.Invoke(subscriber, new object[] { sender, message }); // 메시지를 인자로 삼는 메서드의 경우.
							break;
						case 1:
							method.Invoke(subscriber, new object[] { message }); // 메시지를 인자로 삼는 메서드의 경우.
							break;
						case 0:
							method.Invoke(subscriber, null); // 메시지와 동일한 이름의 메서드.
							break;
					}
				}

				Sender = null;
				CurrentMessage = null;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		/// <summary>
		/// 전체 리스너에게 통지.
		/// </summary>
		public void Notify(IMessage message)
		{
			Notify(null, message);
		}

		/// <summary>
		/// 전체 리스너에게 통지.
		/// </summary>
		public void Notify(object sender, IMessage message)
		{
			if (message == null)
			{
				//Debug.LogError($"[Messenger] message is null.");
				return;
			}

			Sender = sender;

			foreach (var subscriber in messageTargets.Keys)
			{
				// 수신부 중 하나에서 죽을 경우를 대비한 방어 처리.
				try
				{
					Send(subscriber, message);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}

			Sender = null;
			CurrentMessage = null;
		}
	}
}