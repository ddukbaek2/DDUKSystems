using System;
using System.Collections.Generic;


namespace DagraacSystems.Notification
{
	/// <summary>
	/// 통지 처리기.
	/// </summary>
	public sealed class Notification
	{
		private static readonly Lazy<Notification> s_Instance = new Lazy<Notification>(() => new Notification(), true); // thread-safe.
		public static Notification Instance => s_Instance.Value;

		private Dictionary<Type, Delegate> m_Callbacks;

		public Notification()
		{
			m_Callbacks = new Dictionary<Type, Delegate>();
		}

		/// <summary>
		/// 전체삭제.
		/// </summary>
		public void Clear()
		{
			m_Callbacks.Clear();
		}

		/// <summary>
		/// 타입별 콜백 등록.
		/// </summary>
		public void Register<T>(T callback) where T : Delegate
		{
			if (callback == null)
				return;

			var key = typeof(T);
			if (m_Callbacks.ContainsKey(key))
			{
				m_Callbacks[key] = Delegate.Combine(m_Callbacks[key], callback);
			}
			else
			{
				m_Callbacks.Add(key, callback);
			}
		}

		/// <summary>
		/// 타입별 콜백 등록해제.
		/// </summary>
		public void Unregister<T>(T callback) where T : Delegate
		{
			if (callback == null)
				return;

			var key = typeof(T);
			if (m_Callbacks.ContainsKey(key))
			{
				var removedCallback = Delegate.Remove(m_Callbacks[key], callback);
				if (removedCallback != null)
				{
					var invocationList = removedCallback.GetInvocationList();
					if (invocationList.Length > 0)
					{
						m_Callbacks[key] = removedCallback;
						return;
					}
				}

				Unregister<T>();
			}
		}

		/// <summary>
		/// 타입 전체 등록해제.
		/// </summary>
		public void Unregister<T>() where T : Delegate
		{
			var key = typeof(T);
			m_Callbacks.Remove(key);
		}

		/// <summary>
		/// 타입 통지.
		/// </summary>
		public void Notify<T>(params object[] args) where T : Delegate
		{
			var key = typeof(T);
			if (m_Callbacks.TryGetValue(key, out var callback))
			{
				callback?.DynamicInvoke(args);
			}
		}
	}
}