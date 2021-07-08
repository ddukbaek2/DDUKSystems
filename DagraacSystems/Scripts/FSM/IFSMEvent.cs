using System;
using System.Collections.Generic;
using System.Text;



namespace DagraacSystems.FSM
{
	/// <summary>
	/// 특정 상태를 시작하게 되는 트리거.
	/// </summary>
	public interface IFSMEvent
	{
		bool IsCondition(); // 조건체크.
		void DoEvent(); // 강제 실행.
		//IFSMState CallState(); // 전이될 상태.
	}
}