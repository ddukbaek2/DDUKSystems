namespace DagraacSystems
{
	/// <summary>
	/// 일정 시간(초)만큼 머무름.
	/// </summary>
	public class WaitForSeconds : YieldInstruction
	{
		private float m_Time;
		private float m_Duration;

		public WaitForSeconds(float _duration) : base()
		{
			m_Duration = _duration;
		}

		protected override void OnStarted()
		{
			m_Time = 0f;
		}

		protected override bool OnUpdated(float _tick)
		{
			m_Time += _tick;
			if (m_Time < m_Duration)
				return false; // 무한 대기.

			return true;
		}
	}
}