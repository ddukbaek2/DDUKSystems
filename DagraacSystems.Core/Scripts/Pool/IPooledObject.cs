namespace DagraacSystems
{
	/// <summary>
	/// Pool<T>에 들어갈 수 있는 객체 인터페이스.
	/// 하지만 사용하지 않아도 상관 없음.
	/// </summary>
	public interface IPooledObject
	{
		/// <summary>
		/// 풀에 넣음.
		/// </summary>
		void OnEnqueued(IPool _pool);

		/// <summary>
		/// 풀에서 빼냄.
		/// </summary>
		void OnDequeued(IPool _pool);
	}


	/// <summary>
	/// Pool<T>에 들어갈 수 있는 객체 인터페이스.
	/// 하지만 사용하지 않아도 상관 없음.
	/// </summary>
	public interface IPooledObject<T>
	{
		/// <summary>
		/// 풀에 넣음.
		/// </summary>
		void OnEnqueued(IPool<T> _pool);

		/// <summary>
		/// 풀에서 빼냄.
		/// </summary>
		void OnDequeued(IPool<T> _pool);
	}
}