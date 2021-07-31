using DagraacSystems.Process;
using System.Collections.Generic;
using System.Linq;


namespace DagraacSystems.FSM
{
	/// <summary>
	/// FSM 처리기.
	/// FSM에서 사용되는 STATE는 상태의 고유성을 가져야함. 예를들어 IDLESTATE가 있다면 동일한 클래스 2개를 등록할 수 없음.
	/// </summary>
	public class FSMMachine : FSMInstance
	{
		private IFSMTarget m_Target;
		private List<FSMTrigger> m_Triggers;
		private List<FSMState> m_States;

		public FSMMachine()
		{
			m_Target = null;
			m_Triggers = new List<FSMTrigger>();
			m_States = new List<FSMState>();
		}

		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);
			m_Target = args[0] as IFSMTarget;
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			foreach (var trigger in m_Triggers)
			{
				//if ()
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
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
				FSMManager.Instance.m_ProcessExecutor.Stop(state, true);
				return;
			}

			FSMManager.Instance.m_ProcessExecutor.Start(state);
		}

		public void SuspendState(FSMState state)
		{
			if (state == null)
				return;

			if (!IsRunningState(state))
				return;

			FSMManager.Instance.m_ProcessExecutor.Stop(state);
		}

		public TFSMState AddState<TFSMState>() where TFSMState : FSMState, new()
		{
			var state = FSMInstance.CreateInstance<TFSMState>(this);
			m_States.Add(state);

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

			if (!state.IsFinished())
				state.Finish();

			FSMInstance.DestroyInstance(state);
			m_States.Remove(state);
		}

		public void RemoveAllStates()
		{
			while (m_States.Count > 0)
			{
				RemoveState(m_States[0]);
			}
		}

		public void AddTrigger(FSMTrigger trigger)
		{
			m_Triggers.Add(trigger);
		}

		public void RemoveTrigger(FSMTrigger trigger)
		{
			m_Triggers.Remove(trigger);
		}

		public void RemoveAllTriggers()
		{
			while (m_Triggers.Count > 0)
				RemoveTrigger(m_Triggers[0]);
		}

		public bool IsRunningState(FSMState state)
		{
			return state.IsStarted() && !state.IsFinished();
		}

		public TFSMState GetState<TFSMState>(ulong instanceID) where TFSMState : FSMState
		{
			return m_States.Find(it => it.GetInstanceID() == instanceID) as TFSMState;
		}
	}
}