using System;


namespace DDUKSystems
{
	/// <summary>
	/// IPoolAsync<T>에서 동적항목을 생성시키기 위한 인터페이스.
	/// 사용하지 않을 경우 기본값으로 new T()가 호출됨.
	/// </summary>
	public interface IPooledCreatableAsync<T>
	{
		/// <summary>
		/// 비동기 생성.
		/// </summary>
		void CreateInstanceAsync(IPoolAsync _poolAsync, Action<T> _onComplete);
	}
}