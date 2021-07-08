using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.Network
{
	public class PacketManagerTemplate<T> : Manager<T> where T : PacketManagerTemplate<T>, new()
	{
		public PacketManagerTemplate() : base()
		{
		}

		protected override void OnDispose(bool disposing)
		{
		}
	}
}
