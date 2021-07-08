using System.Collections.Generic;


namespace DagraacSystems.Process
{
	/// <summary>
	/// 처리기계를 상속받은 처리.
	/// 처리가 처리를 실행한다.
	/// </summary>
	public class Process : ProcessExecutor
	{
		private bool m_IsFinished;

		public Process()
		{
			m_IsFinished = false;
		}

		public virtual void Reset()
		{
			for (var i = 0; i < m_RunningProcesses.Count; ++i)
			{
				var process = m_RunningProcesses[i];
				process.Reset();
			}
		}

		public virtual void Execute()
		{
			for (var i = 0; i < m_RunningProcesses.Count; ++i)
			{
				var process = m_RunningProcesses[i];
				process.Execute();
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
		}

		public virtual void Finish()
		{
			m_IsFinished = true;
			StopAll();
		}

		public bool IsFinished() => m_IsFinished;
	}
}