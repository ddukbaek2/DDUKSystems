using DagraacSystems.Process;
using System.Collections.Generic;
using System.Linq;


namespace DagraacSystems.FSM
{
	public class FSMMachine : Process.Process
	{
		public IFSMTarget m_Target;

		internal List<FSMTrigger> m_Triggers;
		internal List<FSMState> m_States;
		internal List<FSMState> m_RunningStates;

		public FSMMachine(IFSMTarget target)
		{
			m_Target = target;

			m_Triggers = new List<FSMTrigger>();
			m_States = new List<FSMState>();
			m_RunningStates = new List<FSMState>();
		}

		protected override void OnExecute()
		{
			base.OnExecute();
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			foreach (var state in m_RunningStates)
				state.Update(deltaTime);
		}

		protected override void OnFinish()
		{
			base.OnFinish();
		}

		public void Clear()
		{
			m_States.Clear();
			m_Triggers.Clear();
		}

		public void RunState(FSMState state)
		{
			if (state == null)
				return;

			if (FSMManager.Instance.m_ProcessExecutor.IsRunning(state))
				return;

			m_RunningStates.Add(state);
			FSMManager.Instance.m_ProcessExecutor.Start(state);
		}

		public void RunState(ulong instanceID)
		{
			RunState(GetStateFromInstanceID<FSMState>(instanceID));
		}

		public void RunState<TFSMState>() where TFSMState : FSMState
		{
			RunState(GetState<TFSMState>());
		}

		public TFSMState CreateState<TFSMState>() where TFSMState : FSMState, new()
		{
			if (m_States.Count(item => item is TFSMState) > 0)
				return null;

			var state = FSMInstance.CreateInstance<TFSMState>();
			m_States.Add(state);

			return state;
		}

		public void DestroyState<TFSMState>() where TFSMState : FSMState
		{
			var state = m_States.Find(it => it is TFSMState);
			if (state == null)
				return;

			state.Finish();
			FSMInstance.DestroyInstance(state);
			m_States.Remove(state);
		}

		public void AddTrigger(FSMTrigger trigger)
		{
			m_Triggers.Add(trigger);
		}

		public void RemoveTrigger(FSMTrigger trigger)
		{
			m_Triggers.Remove(trigger);
		}

		public bool IsRunningState(FSMState state)
		{
			return m_RunningStates.Contains(state);
		}

		public TFSMState GetState<TFSMState>() where TFSMState : FSMState
		{
			return m_States.Find(it => it is TFSMState) as TFSMState;
		}

		public TFSMState GetStateFromInstanceID<TFSMState>(ulong instanceID) where TFSMState : FSMState
		{
			return m_States.Find(it => it.GetInstanceID() == instanceID) as TFSMState;
		}
	}
}