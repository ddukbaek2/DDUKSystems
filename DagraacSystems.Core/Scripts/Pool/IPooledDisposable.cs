namespace DagraacSystems
{
	/// <summary>
	/// Pool<T>에서 동적항목을 해제시키기 위한 인터페이스.
	/// 사용하지 않을 경우 기본값이 없어 아무 것도 처리하지 않으니 메모리 관리 주의.
	/// </summary>
	public interface IPooledDisposable<T>
	{
		/// <summary>
		/// 해제(파괴).
		/// </summary>
		void DisposeInstance(IPool _pool, T _value);
	}
}