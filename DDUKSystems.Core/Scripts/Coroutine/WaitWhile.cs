using System; // Func


namespace DDUKSystems
{
	/// <summary>
	/// 거짓이 될때까지 머무름.
	/// 조건이 없으면 거짓으로 간주한다.
	/// </summary>
	public class WaitWhile : YieldInstruction
	{
		private Func<bool> m_Condition;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public WaitWhile(Func<bool> _condition = null) : base()
		{
			m_Condition = _condition;
		}

		/// <summary>
		/// 틱 처리.
		/// </summary>
		protected override bool OnUpdate(float deltaTime)
		{
			if (m_Condition == null)
				return false;

			return !m_Condition.Invoke();
		}
	}
}