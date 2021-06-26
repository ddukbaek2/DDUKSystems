using System;


namespace DagraacSystems
{
	public class Singleton<T> where T : Singleton<T>, new()
	{
		private static readonly Lazy<T> m_Instance = new Lazy<T>(() => new T(), true); // thread-safe.
		public static T Instance => m_Instance.Value;
	}
}