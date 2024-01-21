using System; // Func


namespace DDUKSystems
{
	/// <summary>
	/// 참이 될때까지 머무름.
	/// 조건이 없으면 참으로 간주한다.
	/// </summary>
	public class WaitUntil : YieldInstruction
	{
		private Func<bool> m_Condition;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public WaitUntil(Func<bool> _condition = null) : base()
		{
			m_Condition = _condition;
		}

		/// <summary>
		/// 틱 처리.
		/// </summary>
		protected override bool OnUpdate(float deltaTime)
		{
			if (m_Condition == null)
				return true;

			return m_Condition.Invoke();
		}
	}
}