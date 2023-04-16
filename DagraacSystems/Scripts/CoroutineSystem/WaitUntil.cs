using System;


namespace DagraacSystems
{
	/// <summary>
	/// 참이 될때까지 머무름.
	/// 조건이 없으면 참으로 간주한다.
	/// </summary>
	public class WaitUntil : YieldInstruction
	{
		private Func<bool> m_Condition;

		public WaitUntil(Func<bool> _condition = null) : base()
		{
			m_Condition = _condition;
		}

		protected override bool OnUpdated(float _tick)
		{
			return m_Condition?.Invoke() ?? true;
		}
	}
}