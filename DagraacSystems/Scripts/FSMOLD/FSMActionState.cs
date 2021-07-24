using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.FSM
{
	/// <summary>
	/// 행위가 결정된 상태.
	/// </summary>
	public class FSMActionState : FSMState
	{
		private List<IFSMAction> m_Actions;
		private Queue<IFSMAction> m_ActionQueue;
		private List<IFSMAction> m_RunningActions;

		public FSMActionState() : base()
		{
			m_Actions = new List<IFSMAction>();
			m_ActionQueue = new Queue<IFSMAction>();

			//IFSMTransition
			//IFSMDecide
			//IFSMEvent
		}

		public override void Enter()
		{
			base.Enter();

			m_ActionQueue.Clear();
			foreach (var action in m_Actions)
				m_ActionQueue.Enqueue(action);

			m_RunningActions.Clear();
			ProcessActions();
		}


		private void ProcessActions()
		{
			if (m_ActionQueue.Count > 0)
			{
				var action = m_ActionQueue.Peek();

				if (!m_RunningActions.Contains(action))
				{
					m_RunningActions.Add(action);
					action.IsFinished = false;
					action.BeginAct();
				}
				else if (!action.IsAcquired)
				{
				}
			}
		}

		public void AddAction(IFSMAction action)
		{
			m_Actions.Add(action);
		}

		public void InsertAction(int index, IFSMAction action)
		{
			m_Actions.Insert(index, action);
		}

		public void RemoveAction(IFSMAction action)
		{
			m_Actions.Remove(action);
		}
	}
}