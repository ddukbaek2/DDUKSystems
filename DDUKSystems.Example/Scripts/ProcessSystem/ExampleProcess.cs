using DDUKSystems;


namespace DagraacSystemsExample
{
	/// <summary>
	/// 5초를 세는 프로세스.
	/// </summary>
	public class ExampleProcess : Process
	{
		private int _count;
		private float _accTime;

		protected override void OnReset()
		{
			base.OnReset();

			_count = 0;
			_accTime = 0f;
			DDUKSystems.Debug.Log("OnReset()");
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);
			DDUKSystems.Debug.Log("OnExecute()");
		}

		protected override void OnTick(float deltaTime)
		{
			base.OnTick(deltaTime);

			_accTime += deltaTime;
			if (_accTime >= 1.0f)
			{
				_accTime = 0f;
				++_count;
				DDUKSystems.Debug.Log($"{_count}");
			}

			if (_count >= 5)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			DDUKSystems.Debug.Log("OnComplete()");
		}
	}
}