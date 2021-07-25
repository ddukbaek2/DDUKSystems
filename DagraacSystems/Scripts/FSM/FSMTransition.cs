using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.FSM
{
	/// <summary>
	/// 상태 전이.
	/// </summary>
	public class FSMTransition : FSMInstance
	{
		public FSMTransition()
		{
		}

		public virtual bool IsContidition()
		{
			return false;
		}

		public virtual FSMState GetDestinationState()
		{
			return null;
		}
	}
}