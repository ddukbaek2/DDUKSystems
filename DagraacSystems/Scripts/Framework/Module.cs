using System.Collections.Generic;


namespace DagraacSystems.Framework
{
	/// <summary>
	/// 모델들을 관리하는 기능 객체의 기본 틀.
	/// </summary>
	public class Module : Object
	{
		protected List<Pool> _pools;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Module() : base()
		{
			_pools = new List<Pool>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			_pools.Clear();
			_pools = null;

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 로드됨.
		/// </summary>
		[Subscribe(typeof(OnModuleLoad))]
		protected virtual void OnLoad(OnModuleLoad message)
		{
		}

		/// <summary>
		/// 언로드됨.
		/// </summary>
		[Subscribe(typeof(OnModuleUnload))]
		protected virtual void OnUnload(OnModuleUnload message)
		{
		}

		/// <summary>
		/// 업데이트됨.
		/// </summary>
		[Subscribe(typeof(OnModuleUpdate))]
		protected virtual void OnUpdate(OnModuleUpdate message)
		{
			foreach (var pool in _pools)
				pool.FrameMove(message.DeltaTime);
		}
	}
}