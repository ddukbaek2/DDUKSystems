using System.Collections.Generic;


namespace DagraacSystems.FSM
{
	public class FSMState : FSMInstance
	{
		private List<FSMAction> m_Actions;
		private List<FSMTransition> m_Transitions;

		private int m_Position;

		public FSMState()
		{
			m_Actions = new List<FSMAction>();
			m_Transitions = new List<FSMTransition>();
			m_Position = 0;
		}

		protected override void OnCreate()
		{
			base.OnCreate();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		protected override void OnReset()
		{
			base.OnReset();

			m_Position = 0;
		}

		protected override void OnExecute()
		{
			base.OnExecute();

			if (m_Position < m_Actions.Count)
			{
				var action = m_Actions[m_Position];

				var processExecutor = GetProcessExecutor();
				processExecutor.Start(action);
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			if (m_Position < m_Actions.Count)
			{
				var action = m_Actions[m_Position];
				if (action.IsFinished())
				{
					++m_Position;
					if (m_Position < m_Actions.Count)
					{
						action = m_Actions[m_Position];
						var processExecutor = GetProcessExecutor();
						processExecutor.Start(action);
					}
				}
			}
			else
			{
				var isAllFinished = true;
				foreach (var action in m_Actions)
				{
					if (!action.IsFinished())
					{
						isAllFinished = false;
						break;
					}
				}

				if (isAllFinished)
					Finish();
			}
		}
	}
}