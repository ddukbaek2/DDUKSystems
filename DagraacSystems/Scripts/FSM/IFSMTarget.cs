namespace DagraacSystems.FSM
{
	public interface IFSMTarget
	{
		void OnChangeState(FSMMachine machine, FSMTransition transition, FSMState state);
		void OnExecuteAction(FSMAction action);
	}
}