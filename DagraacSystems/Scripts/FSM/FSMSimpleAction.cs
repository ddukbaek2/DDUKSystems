using System;


namespace DagraacSystems.FSM
{
	/// <summary>
	/// FSM 액션.
	/// </summary>
	public class FSMSimpleAction : FSMAction
	{
		private Action _action;

		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);
			_action = args[0] as Action;
		}

		protected override void OnExecute(params object[] args)
		{
			//base.OnExecute(args);
			_action?.Invoke();
			Finish();
		}
	}
}