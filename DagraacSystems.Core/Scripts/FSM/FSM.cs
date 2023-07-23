using System; // Action


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
}