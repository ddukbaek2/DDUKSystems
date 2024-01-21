using DDUKSystems;


namespace DDUKSystems.Game
{
	public enum EventTrigger
	{
		TileIn,
		TileOut,
		TileInTime,
	}

	public enum EventCondition
	{
		In,
		Out,
	}

	public enum EventAction
	{

	}

	
	/// <summary>
	/// 이벤트 모듈.
	/// </summary>
	public class EventSystem : FModule
	{
		/// <summary>
		/// 로드됨.
		/// </summary>
		protected override void OnLoad(OnModuleLoad message)
		{
			base.OnLoad(message);
		}

		/// <summary>
		/// 언로드됨.
		/// </summary>
		protected override void OnUnload(OnModuleUnload message)
		{
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