namespace uScript
{
	/// <summary>
	/// 메모리 명령.
	/// </summary>
	public enum UMemoryCommand
	{
		/// <summary>
		/// 없음.
		/// </summary>
		None = 0,

		/// <summary>
		/// 데이터 생성.
		/// </summary>
		New,

		/// <summary>
		/// 데이터 삭제.
		/// </summary>
		Delete,

		/// <summary>
		/// 데이터 설정.
		/// </summary>
		Set,

		/// <summary>
		/// 얻기.
		/// </summary>
		Get,
	}


	/// <summary>
	/// 메모리에 대한 간략한 명령.
	/// </summary>
	public class UMemorySimplifiedCommand : USimplifiedCommand
	{
		/// <summary>
		/// 메모리.
		/// </summary>
		private UMemoryCommand m_MemoryCommand;

		/// <summary>
		/// 메모리.
		/// </summary>
		public UMemoryCommand MemoryCommand => m_MemoryCommand;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UMemorySimplifiedCommand(string name, UMemoryCommand memoryCommand, UMemorySimplifiedCommand parent = null) : base(name, parent)
		{
			m_MemoryCommand = memoryCommand;
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public override void Execute(params object[] arguments)
		{
			// TODO.
		}
	}
}