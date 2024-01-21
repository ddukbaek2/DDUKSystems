using System; // Enum


namespace DDUKSystems
{
	public class FSMState<TEnum> : ManagedObject, IFSMState<TEnum> where TEnum : Enum
	{
		public virtual TEnum StateID => default;

		void IFSMState<TEnum>.Enter(ClassFSM<TEnum> _machine, IFSMState<TEnum> _prevState, IFSMTransitionParameter _transitionParameter, IFSMStateResult _prevStateResult)
		{
		}

		IFSMStateResult IFSMState<TEnum>.Exit(ClassFSM<TEnum> _machine, IFSMState<TEnum> _nextState)
		{
			return default;
		}
	}
}