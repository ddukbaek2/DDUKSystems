namespace DDUKSystems
{
	/// <summary>
	/// 코루틴 지연 객체 인터페이스.
	/// </summary>
	public interface IYieldInstruction
	{
		void Start();
		bool Update(float deltaTime);
		void Complete();
	}
}