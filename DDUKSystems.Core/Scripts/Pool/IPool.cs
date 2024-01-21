namespace DDUKSystems
{
	/// <summary>
	/// 풀 인터페이스.
	/// 박싱문제로 인해 인터페이스 함수 호출 (object 관련)은 가급적 삼가할 것.
	/// </summary>
	public interface IPool
	{
		int Count { get; }
		void Enqueue(object obj);
		object Dequeue(bool autoIncrease, bool silentEnqueue);
		bool Contains(object obj);
		void Clear();
		void Dispose();
	}


	/// <summary>
	/// 풀 인터페이스.
	/// </summary>
	public interface IPool<T> : IPool
	{
		void Enqueue(T obj);
		new T Dequeue(bool autoIncrease, bool silentEnqueue);
		bool Contains(T obj);
	}
}