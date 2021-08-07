using System;
using System.Collections.Generic;


namespace DagraacSystems.Repository
{


	/// <summary>
	/// 인스턴스 저장소.
	/// 종류마다 하나의 인스턴스만 보관하는 객체이다.
	/// </summary>
	public static class Repository
	{
		/// <summary>
		/// 검색하여 반환.
		/// </summary>
		public delegate IRepositorable OnFindEvent(Type type);

		/// <summary>
		/// 생성하여 반환.
		/// </summary>
		public delegate IRepositorable OnInstantiateEvent(Type type, object resource, params object[] args);

		/// <summary>
		/// 객체 보관.
		/// </summary>
		private static Dictionary<Type, IRepositorable> s_Instances;

		/// <summary>
		/// 임시사용용 객체.
		/// </summary>
		private static List<Type> s_TemporaryTypes;

		/// <summary>
		/// 검색.
		/// </summary>
		public static OnFindEvent OnFind { set; get; }

		/// <summary>
		/// 생성.
		/// </summary>
		public static OnInstantiateEvent OnInstantiate { set; get; }

		/// <summary>
		/// 생성.
		/// </summary>
		static Repository()
		{
			s_Instances = new Dictionary<Type, IRepositorable>();
			s_TemporaryTypes = new List<Type>();
		}

		/// <summary>
		/// 초기화.
		/// </summary>
		public static void Cleanup(bool nullOnly = true)
		{
			if (nullOnly)
			{
				if (s_Instances.Count > 0)
				{
					s_TemporaryTypes.Clear();
					s_TemporaryTypes.AddRange(s_Instances.Keys);

					foreach (var type in s_TemporaryTypes)
					{
						if (Contains(type))
							continue;

						s_Instances.Remove(type);
					}
				}
			}
			else
			{
				s_Instances.Clear();
			}
		}

		/// <summary>
		/// 해당 타입에 대한 인스턴스를 보관중인지 여부.
		/// </summary>
		public static bool Contains(Type type)
		{
			if (s_Instances.TryGetValue(type, out var instance))
			{
				if (instance == null)
					return false;

				return true;
			}

			return false;
		}

		/// <summary>
		/// 명시적 생성.
		/// 객체가 존재하는지 한번 검색부터 함.
		/// </summary>
		public static IRepositorable Instantiate(Type type, object resource = null, params object[] args)
		{
			var instance = Get(type, false, null);

			if (instance == null)
				instance = OnInstantiate?.Invoke(type, resource, args) ?? null;

			if (instance == null)
				return null;

			s_Instances.Add(type, instance);
			instance.OnInstantiated(true, args);
			return instance;
		}

		/// <summary>
		/// 객체 반환.
		/// </summary>
		public static IRepositorable Get(Type type, bool autoInstantiate = false, object resource = null, params object[] args)
		{
			// 실질적인 객체가 있으면 반환.
			if (Contains(type))
				return s_Instances[type];

			// 등록은 되어있지만 널값이라면 기존 찌꺼기 제거시도.			
			if (s_Instances.ContainsKey(type))
				s_Instances.Remove(type);

			// 없으면 검색.
			var instance = OnFind?.Invoke(type) ?? null;
			if (instance != null)
			{
				s_Instances.Add(type, instance);
				return s_Instances[type];
			}

			// 검색해도 없으면 생성.
			if (autoInstantiate)
				return Instantiate(type, resource, args);

			// 찾지도 생성하지도 못함.
			return null;
		}

		/// <summary>
		/// 해당 타입이 보관중인지 여부.
		/// </summary>
		public static bool Contains<T>() where T : IRepositorable
		{
			return Contains(typeof(T));
		}

		/// <summary>
		/// 명시적 생성.
		/// </summary>
		public static T Instantiate<T>(object resource = null, params object[] args) where T : IRepositorable
		{
			var instance = Instantiate(typeof(T), resource);
			if (instance == null)
				return default;

			return (T)instance;
		}

		/// <summary>
		/// 객체 반환.
		/// </summary>
		public static T Get<T>(bool autoInstantiate = false, object resource = null, params object[] args) where T : IRepositorable
		{
			var instance = Get(typeof(T), autoInstantiate, resource, args);
			if (instance == null)
				return default;

			return (T)instance;
		}
	}
}