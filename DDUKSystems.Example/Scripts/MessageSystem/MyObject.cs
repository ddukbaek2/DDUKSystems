using System;
using DDUKSystems;


namespace DagraacSystemsExample
{
	public class SpawnState : FSMState
	{
		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);


			// TODO.
			AddTransition<FSMTransition>("", this, () => true);
		}
	}

	public class MyObject : IFSMTarget
	{
		private FSMMachine _fsm { set; get; }

		public MyObject(FSMSystem _system)
		{
			//Messenger.Instance.AddEventTarget<NotificationType.OnTest>(OnTest);

			_fsm = _system.AddMachine<FSMMachine>("FSM_MACHINE", this);


			var state = default(FSMState);
			state = _fsm.AddState<FSMState>("FSM_STATE_IDLE");
			state.AddAction<FSMSimpleAction>(new Action(() => { Console.WriteLine("FSMSimpleAction();"); }));

			_fsm.RunState("FSM_STATE_IDLE");
		}

		~MyObject()
		{
			//Messenger.Instance.Unsubscribe<NotificationType.OnTest>(OnTest);
		}

		private void OnTest()
		{
			Debug.Log("OnCreate()");
		}

		void IFSMTarget.OnChangeState(FSMMachine machine, FSMTransition transition, FSMState state)
		{

		}

		void IFSMTarget.OnExecuteAction(FSMAction action)
		{
		}
	}
}