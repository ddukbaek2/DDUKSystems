namespace DagraacSystems.FSM
{
	public interface IFSMTarget
	{
		FSMMachine Machine { set; get; }

		void OnChangeState(FSMState state);
		void ExecuteTrigger(string eventName);
	}
}