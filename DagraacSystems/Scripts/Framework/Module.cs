using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 모델들을 관리하는 기능 객체의 기본 틀 (=시스템).
	/// </summary>
	public class Module : FrameworkObject
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public Module() : base()
		{
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
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
		}
	}
}