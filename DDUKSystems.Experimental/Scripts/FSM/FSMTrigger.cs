namespace DDUKSystems
{
	public class FSMTrigger : FSMObject
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