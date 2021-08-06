using System;


namespace DagraacSystems
{
	public class Singleton<T> where T : Singleton<T>, new()
	{
		//private static readonly Lazy<T> m_Instance = new Lazy<T>(CreateInstance, true); // thread-safe.
		//public static T Instance => m_Instance.Value;

		protected static T m_Instance = default;
		public static T Instance
		{
			get
			{
				if (m_Instance == null)
					m_Instance = CreateInstance();
				return m_Instance;
			}
		}

		protected static T CreateInstance()
		{
			var instance = new T();
			instance.OnCreate();
			return instance;
		}

		protected virtual void OnCreate()
		{
		}
	}
}