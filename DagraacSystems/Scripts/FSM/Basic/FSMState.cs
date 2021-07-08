using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DagraacSystems.FSM
{
	/// <summary>
	/// 기본 상태.
	/// 머신에서 상태를 변경하면 따라간다.
	/// </summary>
	public class FSMState : IFSMState
	{
		private IFSMMachine m_OwnerMachine;

		public FSMState()
		{
			m_OwnerMachine = null;
		}

		public virtual void Enter()
		{

		}

		public virtual void FrameMove(float deltaTime)
		{
		}

		public virtual void Exit()
		{

		}

		/// <summary>
		/// 상태 결정.
		/// </summary>
		public virtual IFSMState Decide()
		{
			// 다음 전이될 상태를 결정한다.
			// 일단은 계속 현재 상태를 반복한다.
			// null을 넣으면 현재 상태가 종료된다.
			return this;
		}

		public void SetOwnerMachine(IFSMMachine machine)
		{
			m_OwnerMachine = machine;
		}

		public IFSMMachine GetOwnerMachine()
		{
			return m_OwnerMachine;
		}
	}
}