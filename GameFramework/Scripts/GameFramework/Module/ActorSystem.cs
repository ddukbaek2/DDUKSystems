using DagraacSystems;
using System.Collections.Generic;


namespace DagraacSystems.Game
{
	public class ActorPool : FPool
	{
	}

	public class ProjectilePool : FPool
	{

	}

	/// <summary>
	/// 액터 모듈.
	/// </summary>
	public class ActorSystem : FModule
	{
		//private Dictionary<ulong, ActorModel> _
		/// <summary>
		/// 로드됨.
		/// </summary>
		protected override void OnLoad(OnModuleLoad message)
		{
			base.OnLoad(message);

			//_pools.AddEventTarget(Pool.CreatePool<ActorPool>(this));
			//_pools.AddEventTarget(Pool.CreatePool<ProjectilePool>(this));
		}

		/// <summary>
		/// 언로드됨.
		/// </summary>
		protected override void OnUnload(OnModuleUnload message)
		{
			//_pools.Remove()
			base.OnUnload(message);
		}

		/// <summary>
		/// 업데이트됨.
		/// </summary>
		protected override void OnTick(OnModuleTick message)
		{
			base.OnTick(message);
		}
	}
}