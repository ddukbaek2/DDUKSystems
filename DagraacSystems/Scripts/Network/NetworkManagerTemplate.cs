using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.Network
{
	public class NetworkManagerTemplate<T> : Singleton<T> where T : NetworkManagerTemplate<T>, new()
	{
		protected override void OnCreate()
		{
			base.OnCreate();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}
	}
}
