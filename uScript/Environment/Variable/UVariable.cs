namespace uScript
{
	/// <summary>
	/// 변수.
	/// </summary>
	public class UVariable : UObject
	{
		private string m_VariableName;
		private UVariableType m_VariableType;
		
		public UVariableType VariableType => m_VariableType;
		public string VariableName => m_VariableName;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UVariable(string variableName, UVariableType variableType) : base()
		{
			m_VariableName = variableName;
			m_VariableType = variableType;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}
	}
}