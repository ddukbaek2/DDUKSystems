using System.Collections.Generic;


namespace DagraacSystems.Obsolete
{
	public interface IFSMTransition
	{
		void Add(IFSMDecide decide, IFSMState state);
		void Remove(IFSMDecide decide);
		IEnumerator<IFSMDecide> GetEnumerator();
	}
}