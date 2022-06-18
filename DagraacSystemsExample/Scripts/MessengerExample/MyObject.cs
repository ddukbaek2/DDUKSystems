using System;
using DagraacSystems;


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
		private FSMMachine m_FSM { set; get; }

		public MyObject()
		{
			//Messenger.Instance.Add<NotificationType.OnTest>(OnTest);

			m_FSM = FSMManager.Instance.AddMachine<FSMMachine>("FSM_MACHINE", this);


			var state = default(FSMState);
			state = m_FSM.AddState<FSMState>("FSM_STATE_IDLE");
			state.AddAction<FSMSimpleAction>(new Action(() => { Console.WriteLine("FSMSimpleAction();"); }));

			m_FSM.RunState("FSM_STATE_IDLE");
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