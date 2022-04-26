using System;


namespace DagraacSystems
{
	/// <summary>
	/// 참이 될때까지 머무름.
	/// </summary>
	public class WaitUntil : YieldInstruction
	{
		private Func<bool> _condition;

		public WaitUntil(Func<bool> condition) : base()
		{
			_condition = condition;
		}

		protected override bool OnUpdated(float tick)
		{
			if (_condition == null)
				return false; // 무한 대기.

			return _condition.Invoke();
		}
	}
}