using System;


namespace DagraacSystems
{
	/// <summary>
	/// 가벼운 상태 처리기.
	/// Enum으로 상태 구분을 하고 상태 전이 및 현재 상태 실행만 처리.
	/// </summary>
	public class FSM<TState> : DisposableObject where TState : Enum
	{
		/// <summary>
		/// 현재 상태.
		/// </summary>
		public TState State { private set; get; }

		/// <summary>
		/// 상태를 실행했을 때의 콜백.
		/// </summary>
		public Action<TState> OnState { set; get; }

		/// <summary>
		/// 전이를 실행했을 때의 콜백. 
		/// </summary>
		public Action<TState, TState> OnTransition { set; get; }

		/// <summary>
		/// 생성.
		/// </summary>
		public FSM(TState initState = default) : base()
		{
			State = initState;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			OnState = null;
			OnTransition = null;

			base.OnDispose(explicitedDispose);
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
		public virtual void DoTransition(TState nextState, bool executeState = true)
		{
			var prevState = State;
			State = nextState;
			OnTransition?.Invoke(prevState, nextState);

			if (executeState)
				DoState();
		}
	}
}