namespace DagraacSystems
{
	/// <summary>
	/// Pool<T>에서 동적항목을 생성시키기 위한 인터페이스.
	/// 사용하지 않을 경우 기본값으로 new T()가 호출됨.
	/// </summary>
	public interface IPooledCreatable<T>
	{
		/// <summary>
		/// 생성.
		/// </summary>
		T CreateInstance(IPool _pool);
	}
}