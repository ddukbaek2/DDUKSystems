namespace US
{
	/// <summary>
	/// 변수 타입.
	/// </summary>
	public enum UVariableType
	{
		/// <summary>
		/// 
		/// </summary>
		None = 0,

		/// <summary>
		/// 
		/// </summary>
		Boolean,

		/// <summary>
		/// 
		/// </summary>
		Number,

		/// <summary>
		/// 
		/// </summary>
		String,

		/// <summary>
		/// 
		/// </summary>
		Object,

		/// <summary>
		/// 
		/// </summary>
		Array,
	}


	/// <summary>
	/// 변수에 대한 간략한 명령.
	/// </summary>
	public class UVariableSimplifiedCommand : USimplifiedCommand
	{
		/// <summary>
		/// 변수 타입.
		/// </summary>
		private UVariableType m_VariableType;

		/// <summary>
		/// 변수 타입.
		/// </summary>
		public UVariableType VariableType => m_VariableType;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UVariableSimplifiedCommand(string name, UVariableType variableType, UVariableSimplifiedCommand parent = null) : base(name, parent)
		{
			m_VariableType = variableType;
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