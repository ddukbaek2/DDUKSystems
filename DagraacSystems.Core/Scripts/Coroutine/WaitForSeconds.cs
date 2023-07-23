namespace DagraacSystems
{
	/// <summary>
	/// 일정 시간(초)만큼 머무름.
	/// </summary>
	public class WaitForSeconds : YieldInstruction
	{
		private float m_Tick;
		private float m_Duration;

		public WaitForSeconds(float _duration) : base()
		{
			m_Duration = _duration;
		}

		protected override void OnStart()
		{
			m_Tick = 0f;
		}

		protected override bool OnTick(float _tick)
		{
			m_Tick += _tick;
			if (m_Tick < m_Duration)
				return false; // 대기.

			return true;
		}
	}
}