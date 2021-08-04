using System;


namespace DagraacSystems.FSM
{
	/// <summary>
	/// FSM 액션.
	/// </summary>
	public class FSMSimpleAction : FSMAction
	{
		private Action m_Action;

		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);
			m_Action = args[0] as Action;
		}

		protected override void OnExecute(params object[] args)
		{
			//base.OnExecute(args);
			m_Action?.Invoke();
			Finish();
		}
	}
}