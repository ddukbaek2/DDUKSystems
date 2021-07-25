namespace DagraacSystems.Obsolete
{
	public class FSMEvent : IFSMEvent
	{
		private bool m_IsCondition;

		public FSMEvent()
		{
			m_IsCondition = false;
		}

		public void DoEvent()
		{
			m_IsCondition = true;
		}

		public virtual bool IsCondition()
		{
			return m_IsCondition;
		}

	}
}