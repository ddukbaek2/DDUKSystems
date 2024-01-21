namespace DDUKSystems
{
	/// <summary>
	/// 모델들을 관리하는 기능 객체의 기본 틀 (=시스템).
	/// </summary>
	public class FModule : FObject
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] _args)
		{
			base.OnCreate(_args);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool _explicitedDispose)
		{
			base.OnDispose(_explicitedDispose);
		}

		/// <summary>
		/// 로드됨.
		/// </summary>
		[Message(typeof(OnModuleLoad))]
		protected virtual void OnLoad(OnModuleLoad _message)
		{
		}

		/// <summary>
		/// 언로드됨.
		/// </summary>
		[Message(typeof(OnModuleUnload))]
		protected virtual void OnUnload(OnModuleUnload _message)
		{
		}

		/// <summary>
		/// 업데이트됨.
		/// </summary>
		[Message(typeof(OnModuleTick))]
		protected virtual void OnTick(OnModuleTick _message)
		{
		}
	}
}