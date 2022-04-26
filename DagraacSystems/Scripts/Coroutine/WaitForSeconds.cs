namespace DagraacSystems
{
	/// <summary>
	/// 일정 시간(초)만큼 머무름.
	/// </summary>
	public class WaitForSeconds : YieldInstruction
	{
		private float _time;
		private float _duration;

		public WaitForSeconds(float duration) : base()
		{
			_duration = duration;
		}

		protected override void OnStarted()
		{
			_time = 0f;
		}

		protected override bool OnUpdated(float tick)
		{
			_time += tick;
			if (_time < _duration)
				return false; // 무한 대기.

			return true;
		}
	}
}