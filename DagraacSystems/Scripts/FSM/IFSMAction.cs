namespace DagraacSystems.FSM
{
	public interface IFSMAction
	{
		void BeginAct();
		void UpdateAct(float deltaTime);
		void EndAct();

		bool IsAcquired { set; get; } // 점유중인가?.
		bool IsFinished { set; get; } // 종료되었는가?.
	}
}