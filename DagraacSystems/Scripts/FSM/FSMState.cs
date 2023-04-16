using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 상태가 시작되면 상태에 대한 모든 행동을 순차적으로 실행하고 (옵션에 의해 비동기적 수행 가능) 모든 액션의 수행이 끝나면 상태가 종료된다.
	/// 상태의 시작~종료 까지 트랜지션이 존재하면 기존 상태를 중단하고 언제고 새로운 상태를 수행한다.
	/// </summary>
	public class FSMState : FSMObject
	{
		private List<FSMTransition> _transitions; // 이동 조건 목록.
		private List<FSMAction> _actions; // 실행 목록.

		private int _actionCursor;
		private FSMTransition _selectedTransition;

		public FSMMachine Target { internal set; get; }

		public FSMState()
		{
			_actions = new List<FSMAction>();
			_transitions = new List<FSMTransition>();
			_selectedTransition = null;
			_actionCursor = 0;
		}

		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		protected override void OnReset()
		{
			base.OnReset();

			_selectedTransition = null;
			_actionCursor = 0;
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);

			if (CheckTransition())
			{
				Finish();
				return;
			}

			if (_actionCursor < _actions.Count)
			{
				var action = _actions[_actionCursor];

				var processExecutor = GetProcessExecutor();
				processExecutor.Start(action);
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			// 조건에 맞으면.
			if (CheckTransition())
			{
				// 강제 중단.
				Finish();
				return;
			}

			if (_actionCursor < _actions.Count)
			{
				var action = _actions[_actionCursor];
				if (action.IsFinished() || action.Async)
				{
					++_actionCursor;
					if (_actionCursor < _actions.Count)
					{
						action = _actions[_actionCursor];
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
					var processing = _actions.Exists(it => !it.IsFinished());
					if (!processing)
						Finish();
				}
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();

			if (CheckTransition())
			{
				var processExecutor = GetProcessExecutor();
				if (processExecutor != null)
					processExecutor.Start(_selectedTransition);
			}
		}

		public bool CheckTransition()
		{
			if (_selectedTransition != null)
				return true;

			foreach (var transition in _transitions)
			{
				if (transition.IsContidition())
				{
					_selectedTransition = transition;
					return true;
				}
			}

			return false;
		}

		public TFSMAction AddAction<TFSMAction>(params object[] args) where TFSMAction : FSMAction, new()
		{
			var action = FSMObject.CreateInstance<TFSMAction>(typeof(TFSMAction).Name, args);
			action.Target = this;
			_actions.Add(action);

			return action;
		}

		public void RemoveAction(FSMAction action)
		{
			if (action == null)
				return;

			if (!action.IsFinished())
				action.Finish();

			FSMObject.DestroyInstance(action);
			_actions.Remove(action);
		}

		public void RemoveAllActions()
		{
			while (_actions.Count > 0)
				RemoveAction(_actions[0]);
		}

		public TFSMTransition AddTransition<TFSMTransition>(string name, FSMState destinationState, Func<bool> predicate) where TFSMTransition : FSMTransition, new()
		{
			var transition = FSMObject.CreateInstance<TFSMTransition>(name, this, destinationState, predicate);
			_transitions.Add(transition);

			return transition;
		}

		public void RemoveTransition(FSMTransition transition)
		{
			if (transition == null)
				return;

			//if (!transition.IsFinished())
			//	transition.Finish();

			FSMObject.DestroyInstance(transition);
			_transitions.Remove(transition);
		}

		public void RemoveAllTransitions()
		{
			while (_transitions.Count > 0)
				RemoveTransition(_transitions[0]);
		}
	}
}