using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// FSM 처리기.
	/// FSM에서 사용되는 STATE는 상태의 고유성을 가져야함. 예를들어 IDLESTATE가 있다면 동일한 클래스 2개를 등록할 수 없음.
	/// </summary>
	public class FSMMachine : FSMInstance
	{
		private FSMSystem FSMSystem;
		private List<FSMTrigger> _triggers;
		private List<FSMState> _states;

		public IFSMTarget Target { internal set; get; }

		public FSMMachine()
		{
			_triggers = new List<FSMTrigger>();
			_states = new List<FSMState>();
		}

		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			foreach (var trigger in _triggers)
			{
				//if ()
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
		}

		public void RunState(string name)
		{
			var state = GetState<FSMState>(name);
			RunState(state);
		}

		public void RunState(ulong instanceID)
		{
			var state = GetState<FSMState>(instanceID);
			RunState(state);
		}

		public void RunState(FSMState state)
		{
			if (state == null)
				return;

			if (IsRunningState(state))
			{
				FSMSystem.m_ProcessSystem.Stop(state.GetProcessID(), true);
				return;
			}

			FSMSystem.m_ProcessSystem.Start(state);
		}

		public void SuspendState(FSMState state)
		{
			if (state == null)
				return;

			if (!IsRunningState(state))
				return;

			FSMSystem.m_ProcessSystem.Stop(state.GetProcessID());
		}

		public TFSMState AddState<TFSMState>(string name) where TFSMState : FSMState, new()
		{
			var state = FSMInstance.CreateInstance<TFSMState>(name);
			state.Target = this;
			_states.Add(state);
			return state;
		}

		public void RemoveState(ulong instanceID)
		{
			var state = GetState<FSMState>(instanceID);
			RemoveState(state);
		}

		public void RemoveState(FSMState state)
		{
			if (state == null)
				return;

			//if (!state.IsFinished())
			//	state.Finish();

			FSMInstance.DestroyInstance(state);
			_states.Remove(state);
		}

		public void RemoveAllStates()
		{
			while (_states.Count > 0)
			{
				RemoveState(_states[0]);
			}
		}

		public void AddTrigger(string name, FSMTrigger trigger)
		{
			_triggers.Add(trigger);
		}

		public void RemoveTrigger(FSMTrigger trigger)
		{
			_triggers.Remove(trigger);
		}

		public void RemoveAllTriggers()
		{
			while (_triggers.Count > 0)
				RemoveTrigger(_triggers[0]);
		}

		public bool IsRunningState(FSMState state)
		{
			var processExecutor = GetProcessExecutor();
			if (processExecutor != null)
				return processExecutor.IsRunning(state.GetProcessID());

			return false;
		}

		public TFSMState GetState<TFSMState>(ulong instanceID) where TFSMState : FSMState
		{
			return _states.Find(it => it.GetInstanceID() == instanceID) as TFSMState;
		}

		public TFSMState GetState<TFSMState>(string name) where TFSMState : FSMState
		{
			return _states.Find(it => it.Name == name) as TFSMState;
		}
	}
}