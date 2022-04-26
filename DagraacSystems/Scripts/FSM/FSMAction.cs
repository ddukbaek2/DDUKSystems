namespace DagraacSystems
{
	/// <summary>
	/// FSM 액션.
	/// </summary>
	public class FSMAction : FSMInstance
	{
		public FSMState Target { internal set; get; }

		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);
			Finish();
		}
	}
}