namespace DDUKSystems
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

		protected override void OnStart()
		{
			m_Time = 0f;
		}

		protected override bool OnUpdate(float deltaTime)
		{
			m_Time += deltaTime;
			if (m_Time < m_Duration)
				return false; // 대기.

			return true;
		}
	}
}