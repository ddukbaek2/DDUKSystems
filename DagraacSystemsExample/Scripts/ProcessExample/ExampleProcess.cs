﻿using DagraacSystems;


namespace DagraacSystemsExample
{
	/// <summary>
	/// 5초를 세는 프로세스.
	/// </summary>
	public class ExampleProcess : Process
	{
		private int m_Count;
		private float m_AccTime;

		protected override void OnReset()
		{
			base.OnReset();

			m_Count = 0;
			m_AccTime = 0f;
			Debug.Log("OnReset()");
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);
			Debug.Log("OnExecute()");
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			m_AccTime += deltaTime;
			if (m_AccTime >= 1.0f)
			{
				m_AccTime = 0f;
				++m_Count;
				Debug.Log($"{m_Count}");
			}

			if (m_Count >= 5)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			Debug.Log("OnFinish()");
		}
	}
}