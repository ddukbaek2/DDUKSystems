namespace DagraacSystems
{
	/// <summary>
	/// 컴포넌트 인터페이스.
	/// </summary>
	public interface IComponent
	{
		void Awake();
		void Start();
		void Dispose();
		void Tick(float deltaTime);
		void AddComponent<T>() where T : IComponent
	}
}