using System; // Enum
using System.Collections.Generic; // Dictionary


namespace DagraacSystems
{
    /// <summary>
    /// 상태 인터페이스.
    /// </summary>
    public interface IFSMState<TStateID> where TStateID : Enum
	{
		/// <summary>
		/// 상태 아이디.
		/// </summary>
		TStateID StateID { get; }

		/// <summary>
		/// 상태 진입함.
		/// </summary>
		void Enter(ClassFSM<TStateID> _machine, IFSMState<TStateID> _prevState, IFSMTransitionParameter _transitionParameter, IFSMStateResult _prevStateResult);

		/// <summary>
		/// 상태 탈출함.
		/// </summary>
		IFSMStateResult Exit(ClassFSM<TStateID> _machine, IFSMState<TStateID> _nextState);
	}


	/// <summary>
	/// 상태 전이시 입력값 인터페이스.
	/// </summary>
	public interface IFSMTransitionParameter
	{
	}


	/// <summary>
	/// 상태 전이시 출력값 인터페이스.
	/// </summary>
	public interface IFSMStateResult
	{
	}


	/// <summary>
	/// 클래스 기반 상태 처리기.
	/// 기본 FSM으로도 클래스기반이 가능함 (상태 인스턴스를 외부에서 관리 + 상태 인스턴스 내 함수 호출은 상속 후 개인에게 맡긴다는 전제).
	/// 이 처리기는 내부에서 전용 인터페이스 기반의 상태 인스턴스를 관리하고 전용 인터페이스의 구현으로 ENTER-EXIT구조를 지원함.
	/// </summary>
	public class ClassFSM<TStateID> : DisposableObject where TStateID : Enum
	{
		/// <summary>
		/// 상태 목록.
		/// </summary>
		private Dictionary<TStateID, IFSMState<TStateID>> states;

		/// <summary>
		/// 현재 상태.
		/// </summary>
		public TStateID State { private set; get; }

		/// <summary>
		/// 생성.
		/// </summary>
		public ClassFSM() : base()
		{
			states = new Dictionary<TStateID, IFSMState<TStateID>>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool _explicitedDispose)
		{
			RemoveAllStates();

			base.OnDispose(_explicitedDispose);
		}

		/// <summary>
		/// 상태 추가 및 수정.
		/// </summary>
		public void AddState(TStateID _stateID, IFSMState<TStateID> _state)
		{
			if (states.ContainsKey(_stateID))
			{
				states[_stateID] = _state;
			}
			else
			{
				states.Add(_stateID, _state);
			}
		}

		/// <summary>
		/// 상태 제거.
		/// </summary>
		public void RemoveState(TStateID _stateID)
		{
			states.Remove(_stateID);
		}

		/// <summary>
		/// 모든 상태 제거.
		/// </summary>
		public void RemoveAllStates()
		{
			states.Clear();
		}

		/// <summary>
		/// 상태 전이.
		/// </summary>
		public virtual void DoTransition(TStateID _nextStateID, IFSMTransitionParameter _transitionParameter = default)
		{
			var prevStateID = State;

			var prevState = GetState<IFSMState<TStateID>>(prevStateID);
			var nextState = GetState<IFSMState<TStateID>>(_nextStateID);
			var prevFSMResult = default(IFSMStateResult);

			if (prevState != default)
			{
				prevFSMResult = prevState.Exit(this, nextState);
			}

			State = _nextStateID;

			if (nextState != default)
			{
				nextState.Enter(this, prevState, _transitionParameter, prevFSMResult);
			}
		}

		/// <summary>
		/// 상태가 존재하는지 여부.
		/// </summary>
		public bool ExistsState(TStateID _stateID)
		{
			if (!states.ContainsKey(_stateID))
				return false;

			return true;
		}

		/// <summary>
		/// 상태 반환.
		/// </summary>
		public IFSMState<TStateID> GetState(TStateID _stateID)
		{
			if (!states.TryGetValue(_stateID, out var state))
				return default;

			return state;
		}

		/// <summary>
		/// 상태 반환.
		/// </summary>
		public TState GetState<TState>(TStateID _stateID) where TState : IFSMState<TStateID>
		{
			if (!ExistsState(_stateID))
				return default;

			return (TState)GetState(_stateID);
		}
	}
}