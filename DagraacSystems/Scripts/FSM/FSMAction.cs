namespace DagraacSystems.FSM
{
	/// <summary>
	/// FSM 액션.
	/// </summary>
	public class FSMAction : FSMInstance
	{
		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);
			Finish();
		}

		protected override void OnFinish()
		{
			base.OnFinish();
		}
	}
}