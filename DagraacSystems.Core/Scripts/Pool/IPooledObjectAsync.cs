namespace DagraacSystems
{
	/// <summary>
	/// Pool<T>에 들어갈 수 있는 객체 인터페이스.
	/// 하지만 사용하지 않아도 상관 없음.
	/// </summary>
	public interface IPooledObjectAsync
	{
		/// <summary>
		/// 풀에 넣음.
		/// </summary>
		void OnEnqueued(IPoolAsync _poolAsync);

		/// <summary>
		/// 풀에서 빼냄.
		/// </summary>
		void OnDequeued(IPoolAsync _poolAsync);
	}


	/// <summary>
	/// Pool<T>에 들어갈 수 있는 객체 인터페이스.
	/// 하지만 사용하지 않아도 상관 없음.
	/// </summary>
	public interface IPooledObjectAsync<T>
	{
		/// <summary>
		/// 풀에 넣음.
		/// </summary>
		void OnEnqueued(IPoolAsync<T> _poolAsync);

		/// <summary>
		/// 풀에서 빼냄.
		/// </summary>
		void OnDequeued(IPoolAsync<T> _poolAsync);
	}
}