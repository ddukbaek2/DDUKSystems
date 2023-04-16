using DagraacSystems;


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
			DagraacSystems.Debug.Log("OnReset()");
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);
			DagraacSystems.Debug.Log("OnExecute()");
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			_accTime += deltaTime;
			if (_accTime >= 1.0f)
			{
				_accTime = 0f;
				++_count;
				DagraacSystems.Debug.Log($"{_count}");
			}

			if (_count >= 5)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			DagraacSystems.Debug.Log("OnFinish()");
		}
	}
}