namespace DagraacSystems.FSM
{
	/// <summary>
	/// FSM 액션.
	/// </summary>
	public class FSMAction : FSMInstance
	{
		private FSMState m_Target;

		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);

			m_Target = args[0] as FSMState;
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);
			Finish();
		}
	}
}