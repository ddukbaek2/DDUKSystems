using System; // Enum


namespace DagraacSystems
{
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
}