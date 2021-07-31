namespace DagraacSystems.FSM
{
	public class FSMTrigger : FSMInstance
	{
		public FSMState Target { set; get; }

		public virtual void DoState()
		{
		}

		public virtual bool IsCondition()
		{
			return true;
		}
	}
}