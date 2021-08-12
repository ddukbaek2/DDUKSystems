using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 메시지 구독/배포 처리기.
	/// </summary>
	public class MessageBroker
	{
		/// <summary>
		/// 타입별 콜백 목록.
		/// </summary>
		private Dictionary<Type, Delegate> m_Subscribes;

		/// <summary>
		/// 생성.
		/// </summary>
		public MessageBroker()
		{
			m_Subscribes = new Dictionary<Type, Delegate>();
		}

		/// <summary>
		/// 전체 제거.
		/// </summary>
		public void Clear()
		{
			m_Subscribes.Clear();
		}

		/// <summary>
		/// 구독.
		/// </summary>
		public void Subscribe<T>(T callback) where T : Delegate
		{
			if (callback == null)
				return;

			var key = typeof(T);
			if (m_Subscribes.ContainsKey(key))
			{
				m_Subscribes[key] = Delegate.Combine(m_Subscribes[key], callback);
			}
			else
			{
				m_Subscribes.Add(key, callback);
			}
		}

		/// <summary>
		/// 구독해제.
		/// </summary>
		public void Unsubscribe<T>(T callback) where T : Delegate
		{
			if (callback == null)
				return;

			var key = typeof(T);
			if (m_Subscribes.ContainsKey(key))
			{
				var del = m_Subscribes[key];
				if (RemoveDelegate(true, ref del, d => d.Equals(callback)) > 0)
					m_Subscribes[key] = del;
				else
					Unsubscribe<T>();
			}
		}

		/// <summary>
		/// 대리자 종류별로 구독해제.
		/// </summary>
		public void Unsubscribe<T>() where T : Delegate
		{
			var key = typeof(T);
			m_Subscribes.Remove(key);
		}

		/// <summary>
		/// 타입별 등록한 모든 함수에 통지.
		/// </summary>
		public void Publish<T>(params object[] args) where T : Delegate
		{
			var key = typeof(T);
			if (m_Subscribes.TryGetValue(key, out var del))
			{
				// 인스턴스가 사라져 유효하지 않은 대리자를 모두 제거.
				if (RemoveDelegate(false, ref del, IsInvaildDelegate) == 0)
				{
					Unsubscribe<T>();
					return;
				}

				// 통지.
				m_Subscribes[key] = del;
				del?.DynamicInvoke(args);
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