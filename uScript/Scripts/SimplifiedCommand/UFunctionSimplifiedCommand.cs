using System.Collections.Generic;


namespace uScript
{
	/// <summary>
	/// 함수에 대한 간략한 명령.
	/// </summary>
	public class UFunctionSimplifiedCommand : USimplifiedCommand
	{
		private List<UVariableSimplifiedCommand> m_Parameters;
		private List<UFunctionSimplifiedCommand> m_SubFunctions;
		private List<UVariableSimplifiedCommand> m_Returns;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UFunctionSimplifiedCommand(string name, UFunctionSimplifiedCommand parent = null) : base(name, parent)
		{
			m_Parameters = new List<UVariableSimplifiedCommand>();
			m_SubFunctions = new List<UFunctionSimplifiedCommand>();
			m_Returns = new List<UVariableSimplifiedCommand>();
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