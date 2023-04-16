using System; // Enum
using System.Collections.Generic; // Dictionary


namespace DagraacSystems
{
	/// <summary>
	/// 상태 처리기.
	/// TState로 상태 구분을 하고 상태 전이 및 현재 상태 실행만 처리.
	/// </summary>
	public class FSM<TState> : DisposableObject
	{
		/// <summary>
		/// 현재 상태.
		/// </summary>
		public TState State { private set; get; }

		/// <summary>
		/// 상태를 실행했을 때의 콜백.
		/// </summary>
		public event Action<TState> OnState;

		/// <summary>
		/// 전이를 실행했을 때의 콜백. 
		/// </summary>
		public event Action<TState, TState> OnTransition;

		/// <summary>
		/// 생성.
		/// </summary>
		public FSM(TState _initializeState = default) : base()
		{
			State = _initializeState;
			// DoState()는 생성 직후 외부에서 호출.
			// 그러면 셋팅된 _initializeState 에 대해 클래스의 OnState 이벤트가 발생한다.
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool _explicitedDispose)
		{
			OnState = null;
			OnTransition = null;

			base.OnDispose(_explicitedDispose);
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		/// <summary>
		/// 현재 상태 실행.
		/// </summary>
		public virtual void DoState()
		{
			OnState?.Invoke(State);
		}

		/// <summary>
		/// 상태 전이.
		/// </summary>
		public virtual void DoTransition(TState _nextState, bool _executeState = true)
		{
			var prevState = State;
			State = _nextState;
			OnTransition?.Invoke(prevState, _nextState);

			if (_executeState)
				DoState();
		}
	}


	/// <summary>
	/// Enum 기반 상태 처리기.
	/// </summary>
	public class EnumFSM<TState> : FSM<TState> where TState : Enum
	{
		/// <summary>
		/// 생성.
		/// </summary>
		public EnumFSM(TState _initializeState = default) : base(_initializeState)
		{
			// DoState()는 생성 직후 외부에서 호출.
			// 그러면 셋팅된 _initializeState 에 대해 클래스의 OnState 이벤트가 발생한다.
		}
	}


	/// <summary>
	/// 상태 인터페이스.
	/// </summary>
	public interface IState<TStateID> where TStateID : Enum
	{
		/// <summary>
		/// 상태 아이디.
		/// </summary>
		public TStateID StateID { get; }

		/// <summary>
		/// 상태 실행됨.
		/// </summary>
		void OnState(ClassFSM<TStateID> _machine);

		/// <summary>
		/// 상태 진입함.
		/// </summary>
		void OnEnter(ClassFSM<TStateID> _machine, IState<TStateID> _prevState);

		/// <summary>
		/// 상태 탈출함.
		/// </summary>
		void OnExit(ClassFSM<TStateID> _machine, IState<TStateID> _nextState);
	}


	/// <summary>
	/// 클래스 기반 상태 처리기.
	/// 기본 FSM으로도 클래스기반이 가능함 (상태 인스턴스를 외부에서 관리 + 상태 인스턴스 내 함수 호출은 상속 후 개인에게 맡긴다는 전제).
	/// 이 처리기는 내부에서 전용 인터페이스 기반의 상태 인스턴스를 관리하고 전용 인터페이스의 구현으로 ENTER-EXIT구조를 지원함.
	/// </summary>
	public class ClassFSM<TStateID> : EnumFSM<TStateID> where TStateID : Enum
	{
		/// <summary>
		/// 상태 목록.
		/// </summary>
		private Dictionary<TStateID, IState<TStateID>> states;

		/// <summary>
		/// 생성.
		/// </summary>
		public ClassFSM(TStateID _initializeStateID, IState<TStateID> _initializeState) : base(_initializeStateID)
		{
			states = new Dictionary<TStateID, IState<TStateID>>();
			AddState(_initializeStateID, _initializeState);

			// DoState()는 생성 직후 외부에서 호출.
			// 그러면 셋팅된 _initializeState 에 대해 클래스의 OnState 이벤트 + 상태 인스턴스의 OnState 이벤트가 발생한다.
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
		public void AddState(TStateID _stateID, IState<TStateID> _state)
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
		/// 상태 실행.
		/// </summary>
		public override void DoState()
		{
			var currentStateID = State;
			var state = GetState<IState<TStateID>>(currentStateID);
			state?.OnState(this);

			base.DoState();
		}

		/// <summary>
		/// 상태 전이.
		/// </summary>
		public override void DoTransition(TStateID _nextStateID, bool _executeState = true)
		{
			var prevStateID = State;

			var prevState = GetState<IState<TStateID>>(prevStateID);
			var nextState = GetState<IState<TStateID>>(_nextStateID);
			prevState?.OnExit(this, nextState);
			nextState?.OnEnter(this, prevState);

			base.DoTransition(_nextStateID, _executeState);
		}

		/// <summary>
		/// 상태 반환.
		/// </summary>
		public TState GetState<TState>(TStateID _stateID) where TState : IState<TStateID>
		{
			if (!states.TryGetValue(_stateID, out var state))
				return default;

			return (TState)state;
		}
	}
}