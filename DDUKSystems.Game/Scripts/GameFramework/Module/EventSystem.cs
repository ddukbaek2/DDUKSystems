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
	/// �̺�Ʈ ���.
	/// </summary>
	public class EventSystem : FModule
	{
		/// <summary>
		/// �ε��.
		/// </summary>
		protected override void OnLoad(OnModuleLoad message)
		{
			base.OnLoad(message);
		}

		/// <summary>
		/// ��ε��.
		/// </summary>
		protected override void OnUnload(OnModuleUnload message)
		{
			base.OnUnload(message);
		}

		/// <summary>
		/// ������Ʈ��.
		/// </summary>
		protected override void OnTick(OnModuleTick message)
		{
			base.OnTick(message);
		}
	}
}