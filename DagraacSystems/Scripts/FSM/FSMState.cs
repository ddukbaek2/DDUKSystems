using System.Collections.Generic;


namespace DagraacSystems.FSM
{
	public class FSMState : FSMInstance
	{
		private List<FSMAction> m_Actions;
		private List<FSMTransition> m_Transitions;

		private int m_ActionCursor;

		public FSMState()
		{
			m_Actions = new List<FSMAction>();
			m_Transitions = new List<FSMTransition>();
			m_ActionCursor = 0;
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

			m_ActionCursor = 0;
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);

			if (m_ActionCursor < m_Actions.Count)
			{
				var action = m_Actions[m_ActionCursor];

				var processExecutor = GetProcessExecutor();
				processExecutor.Start(action);
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			if (m_ActionCursor < m_Actions.Count)
			{
				var action = m_Actions[m_ActionCursor];
				if (action.IsFinished() || action.Async)
				{
					++m_ActionCursor;
					if (m_ActionCursor < m_Actions.Count)
					{
						action = m_Actions[m_ActionCursor];
						var processExecutor = GetProcessExecutor();
						processExecutor.Start(action);
					}
				}
			}
			else
			{
				// 비동기 상태라면 액션이 다 끝나기를 기다리지 않는다.
				if (Async)
				{
					Finish();
				}
				else
				{
					// 전체 액션이 전부 종료되었는지 확인한다.
					var processing = m_Actions.Exists(it => !it.IsFinished());
					if (!processing)
						Finish();
				}
			}
		}

		public void AddAction<TFSMAction>() where TFSMAction : FSMAction
		{

		}
	}
}