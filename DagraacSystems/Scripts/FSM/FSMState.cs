﻿using System;
using System.Collections.Generic;


namespace DagraacSystems.FSM
{
	/// <summary>
	/// 상태가 시작되면 상태에 대한 모든 행동을 순차적으로 실행하고 (옵션에 의해 비동기적 수행 가능) 모든 액션의 수행이 끝나면 상태가 종료된다.
	/// 상태의 시작~종료 까지 트랜지션이 존재하면 기존 상태를 중단하고 언제고 새로운 상태를 수행한다.
	/// </summary>
	public class FSMState : FSMInstance
	{
		private FSMMachine m_Target;
		private List<FSMTransition> m_Transitions; // 이동 조건 목록.
		private List<FSMAction> m_Actions; // 실행 목록.

		private int m_ActionCursor;

		public FSMMachine Target => m_Target;

		public FSMState()
		{
			m_Target = null;
			m_Actions = new List<FSMAction>();
			m_Transitions = new List<FSMTransition>();
			m_ActionCursor = 0;
		}

		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);

			m_Target = args[0] as FSMMachine;
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

			if (CheckAndExecuteTransition())
			{
				Finish();
				return;
			}

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

			if (CheckAndExecuteTransition())
			{
				Finish();
				return;
			}

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

		protected override void OnFinish()
		{
			base.OnFinish();
			CheckAndExecuteTransition();
		}

		public bool CheckAndExecuteTransition()
		{
			foreach (var transition in m_Transitions)
			{
				if (transition.IsContidition())
				{
					var processExecutor = GetProcessExecutor();
					processExecutor.Stop(this);						
					processExecutor.Start(transition);

					return true;
				}
			}

			return false;
		}

		public TFSMAction AddAction<TFSMAction>() where TFSMAction : FSMAction, new()
		{
			var action = FSMInstance.CreateInstance<TFSMAction>(this);
			m_Actions.Add(action);

			return action;
		}

		public void RemoveAction(FSMAction action)
		{
			if (action == null)
				return;

			if (!action.IsFinished())
				action.Finish();

			FSMInstance.DestroyInstance(action);
			m_Actions.Remove(action);
		}

		public void RemoveAllActions()
		{
			while (m_Actions.Count > 0)
				RemoveAction(m_Actions[0]);
		}

		public TFSMTransition AddTransition<TFSMTransition>(FSMState destinationState, Func<bool> predicate) where TFSMTransition : FSMTransition, new()
		{
			var transition = FSMInstance.CreateInstance<TFSMTransition>(this, destinationState, predicate);
			m_Transitions.Add(transition);

			return transition;
		}

		public void RemoveTransition(FSMTransition transition)
		{
			if (transition == null)
				return;

			if (!transition.IsFinished())
				transition.Finish();

			FSMInstance.DestroyInstance(transition);
			m_Transitions.Remove(transition);
		}

		public void RemoveAllTransitions()
		{
			while (m_Transitions.Count > 0)
				RemoveTransition(m_Transitions[0]);
		}
	}
}