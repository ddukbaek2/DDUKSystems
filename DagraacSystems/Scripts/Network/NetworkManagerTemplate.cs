using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.Network
{
	public class NetworkManagerTemplate<T> : Manager<T> where T : NetworkManagerTemplate<T>, new()
	{
		public NetworkManagerTemplate() : base()
		{
		}

		protected override void OnDispose(bool disposing)
		{
		}
	}
}
