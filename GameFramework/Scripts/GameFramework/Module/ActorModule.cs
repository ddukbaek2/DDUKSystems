using DagraacSystems.Framework;
using System.Collections.Generic;


namespace GameFramework
{
	public class ActorPool : Pool
	{
	}

	public class ProjectilePool : Pool
	{

	}

	/// <summary>
	/// 액터 모듈.
	/// </summary>
	public class ActorModule : Module
	{
		//private Dictionary<ulong, ActorModel> _
		/// <summary>
		/// 로드됨.
		/// </summary>
		protected override void OnLoad(OnModuleLoad message)
		{
			base.OnLoad(message);

			_pools.Add(Pool.CreatePool<ActorPool>(this));
			_pools.Add(Pool.CreatePool<ProjectilePool>(this));
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
		protected override void OnUpdate(OnModuleUpdate message)
		{
			base.OnUpdate(message);
		}
	}
}