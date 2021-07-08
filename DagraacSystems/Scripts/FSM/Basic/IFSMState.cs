namespace DagraacSystems.FSM
{
	/// <summary>
	/// 상태 인터페이스.
	/// </summary>
	public interface IFSMState
	{
		/// <summary>
		/// 상태의 진입.
		/// </summary>
		void Enter();

		/// <summary>
		/// 상태의 매 프레임마다 진행.
		/// </summary>
		void FrameMove(float deltaTime);

		/// <summary>
		/// 상태의 탈출.
		/// </summary>
		void Exit();

		/// <summary>
		/// 다음 상태의 결정.
		/// </summary>
		IFSMState Decide();

		/// <summary>
		/// 상태의 처리기(상태기계) 설정.
		/// </summary>
		void SetOwnerMachine(IFSMMachine machine);

		/// <summary>
		/// 상태의 처리기(상태기계) 반환.
		/// </summary>
		IFSMMachine GetOwnerMachine();
	}
}