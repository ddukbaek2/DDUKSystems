using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.Process
{
	public interface IProcessEvent
	{
		bool CheckEvent();
		void DoEvent();
	}
}