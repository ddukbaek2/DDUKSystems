using System.Collections.Generic;


namespace DagraacSystems.FSM
{
	public interface IFSMTransition
	{
		void Add(IFSMDecide decide, IFSMState state);
		void Remove(IFSMDecide decide);
		IEnumerator<IFSMDecide> GetEnumerator();
	}
}