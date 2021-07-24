using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DagraacSystems.Process;


namespace DagraacSystemsExample
{
	/// <summary>
	/// 10초를 세는 프로세스.
	/// </summary>
	public class ExampleProcess : Process
	{
		private int m_Count;
		private float m_AccTime;

		public override void Reset()
		{
			base.Reset();

			m_Count = 0;
			m_AccTime = 0f;
		}

		public override void Execute(ProcessExecutor processExecutor)
		{
			base.Execute(processExecutor);

			Console.WriteLine("Execute()");
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			m_AccTime += deltaTime;
			if (m_AccTime >= 1.0f)
			{
				m_AccTime = 0f;
				++m_Count;
				Console.WriteLine(m_Count);
			}

			if (m_Count >= 10)
			{
				Finish();
			}
		}

		public override void Finish()
		{
			base.Finish();
			Console.WriteLine("Finish()");
		}
	}
}
