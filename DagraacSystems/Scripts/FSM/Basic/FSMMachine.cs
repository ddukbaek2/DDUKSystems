using System.Collections.Generic;


namespace DagraacSystems.FSM
{
	/// <summary>
	/// 상태 처리기.
	/// </summary>
	public class FSMMachine : IFSMMachine
	{
		private Dictionary<int, IFSMState> m_Current; // key:stateLayer, value:state
		private Dictionary<int, IFSMState> m_States; // key:stateid, value:state
		private IFSMTarget m_OwnerTarget;

		/// <summary>
		/// 상태처리기를 계속 동작시킬지 유무.
		/// </summary>
		public bool Enabled { set; get; } = true;

		public FSMMachine()
		{
			m_Current = new Dictionary<int, IFSMState>();
			m_States = new Dictionary<int, IFSMState>();
			m_OwnerTarget = null;
			Enabled = true;
		}

		public virtual void FrameMove(float deltaTime) { if (Enabled) FSMFunction.FrameMove(this, deltaTime); }
		public void To(IFSMState next, int layer = 0) { if (Enabled) FSMFunction.To(this, next, layer); }
		public void To(int key, int layer = 0) { if (Enabled) FSMFunction.To(this, key, layer); }
		public Dictionary<int, IFSMState> GetCurrent() { return m_Current; }
		public Dictionary<int, IFSMState> GetStates() { return m_States; }
		public void SetOwnerTarget(IFSMTarget actor) { m_OwnerTarget = actor; }
		public IFSMTarget GetOwnerTarget() { return m_OwnerTarget; }

		public void AddStateFromInt32(int key, IFSMState state) { FSMFunction.AddState(this, key, state); }
		public void RemoveStateFromInt32(int key, IFSMState state) { FSMFunction.RemoveState(this, key); }
		public IFSMState GetStateFromInt32(int key) { return FSMFunction.GetStateFromKey(this, key); }
		public bool GetInt32FromState(IFSMState state, out int key) { return FSMFunction.GetKeyFromState(this, state, out key); }
		public void ClearAllStates() { FSMFunction.ClearAllStates(this); }
		public int GetStateCount() { return m_States.Count; }
	}
}