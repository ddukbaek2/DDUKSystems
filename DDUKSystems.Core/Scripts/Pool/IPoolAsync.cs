using System;


namespace DDUKSystems
{
	/// <summary>
	/// 비동기 풀 인터페이스.
	/// 박싱문제로 인해 인터페이스 함수 호출 (object 관련)은 가급적 삼가할 것.
	/// </summary>
	public interface IPoolAsync
	{
		/// <summary>
		/// 현재 잔여량.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// 풀에 넣음.
		/// </summary>
		void Enqueue(object _obj);
		void DequeueAsync(Action<object> _onComplete, bool _autoIncrease, bool _silentEnqueue);

		/// <summary>
		/// 대상의 포함 여부.
		/// </summary>
		bool Contains(object _obj);

		/// <summary>
		/// 풀 비우기.
		/// </summary>
		void Clear();

		/// <summary>
		/// 풀 파괴.
		/// </summary>
		void Dispose();
	}


	/// <summary>
	/// 비동기 풀 인터페이스.
	/// </summary>
	public interface IPoolAsync<T> : IPoolAsync
	{
		/// <summary>
		/// 풀에 넣음.
		/// </summary>
		void Enqueue(T _obj);

		/// <summary>
		/// 풀에서 빼냄.
		/// </summary>
		void DequeueAsync(Action<T> _onComplete, bool _autoIncrease, bool _silentEnqueue);

		/// <summary>
		/// 대상의 포함 여부.
		/// </summary>
		bool Contains(T _obj);
	}
}