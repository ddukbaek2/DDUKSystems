using System;


namespace DagraacSystems
{
	/// <summary>
	/// 생성시 쓰레드 안전한 공유 클래스.
	/// </summary>
	public class Singleton<T> where T : Singleton<T>, new()
	{
		private static readonly Lazy<T> _instance = new Lazy<T>(() => new T(), true);
		public static T instance => _instance.Value;
	}
}