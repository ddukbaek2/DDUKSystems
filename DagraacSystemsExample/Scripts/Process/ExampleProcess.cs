using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DagraacSystems.Process;


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
			Console.WriteLine("OnReset()");
		}

		protected override void OnExecute()
		{
			base.OnExecute();
			Console.WriteLine("OnExecute()");
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			m_AccTime += deltaTime;
			if (m_AccTime >= 1.0f)
			{
				m_AccTime = 0f;
				++m_Count;
				Console.WriteLine(m_Count);
			}

			if (m_Count >= 5)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			Console.WriteLine("OnFinish()");
		}
	}
}
