using System.Collections;


namespace DagraacSystems
{
	/// <summary>
	/// 코루틴.
	/// </summary>
	public interface IYield
	{
		void OnBegin();
		bool OnStay(float deltaTime); // 참이 되면 탈출.
		void OnEnd();
	}

	public class WaitForSeconds : IYield
	{
		private float _time;
		private float _duration;

		public WaitForSeconds(float seconds)
		{
			_duration = seconds;
		}

		void IYield.OnBegin()
		{
			_time = 0f;
		}

		void IYield.OnEnd()
		{
		}

		bool IYield.OnStay(float deltaTime)
		{
			_time += deltaTime;
			if (_time < _duration)
				return false;
			return true;
		}
	}
}