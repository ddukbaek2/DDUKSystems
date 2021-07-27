namespace DagraacSystems.FSM
{
	public interface IFSMTarget
	{
		void OnChangeState(FSMMachine machine, FSMState state);
	}
}