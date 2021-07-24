using System;


namespace DagraacSystems
{
	public class Singleton<T> where T : Singleton<T>, new()
	{
		private static readonly Lazy<T> m_Instance = new Lazy<T>(CreateInstance, true); // thread-safe.
		public static T Instance => m_Instance.Value;

		private static T CreateInstance()
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