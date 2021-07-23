using System;
using System.Collections.Generic;


namespace DagraacSystems.Notification
{
	/// <summary>
	/// 통지 처리기.
	/// </summary>
	public class Notification
	{
		//private static readonly Lazy<Notification> s_Instance = new Lazy<Notification>(() => new Notification(), true); // thread-safe.
		//public static Notification Instance => s_Instance.Value;

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
				var del = m_Callbacks[key];
				if (RemoveDelegate(true, ref del, d => d.Equals(callback)) > 0)
					m_Callbacks[key] = del;
				else
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
		/// 타입별 등록한 모든 함수에 통지.
		/// Invalid 검사를 생략했기에 이미 파괴된 객체에도 통지될 수 있음.
		/// </summary>
		public void NotifyFast<T>(params object[] args) where T : Delegate
		{
			var key = typeof(T);
			if (m_Callbacks.TryGetValue(key, out var del))
				del?.DynamicInvoke(args);
		}

		/// <summary>
		/// 타입별 등록한 모든 함수에 통지.
		/// </summary>
		public void Notify<T>(params object[] args) where T : Delegate
		{
			var key = typeof(T);
			if (m_Callbacks.TryGetValue(key, out var del))
			{
				if (RemoveDelegate(false, ref del, IsInvaildDelegate) > 0)
				{
					m_Callbacks[key] = del;
					del?.DynamicInvoke(args);
				}
				else
				{
					Unregister<T>();
				}
			}
		}

		/// <summary>
		/// 유효하지 않은 델리게이트 여부.
		/// </summary>
		private static bool IsInvaildDelegate(Delegate d)
		{
			if (d.Equals(null))
				return true;

			if (d.Method.Equals(null))
				return true;

			if (d.Target.Equals(null))
			{
				if (!d.Method.IsStatic)
					return true;
			}

			return false;
		}

		/// <summary>
		/// 조건에 맞는 델리게이트를 삭제.
		/// </summary>
		private static int RemoveDelegate(bool removeOnce, ref Delegate del, Predicate<Delegate> match)
		{
			if (del == null)
				return 0;

			var invocationList = del.GetInvocationList();
			if (match == null)
				return invocationList.Length;

			foreach (var d in invocationList)
			{
				if (match.Invoke(d))
				{
					del = Delegate.Remove(del, d);
					if (removeOnce)
						break;
				}
			}

			invocationList = del.GetInvocationList();
			return invocationList.Length;
		}
	}
}