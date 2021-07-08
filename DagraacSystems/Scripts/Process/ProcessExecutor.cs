using System;
using System.Collections.Generic;


namespace DagraacSystems.Process
{
	/// <summary>
	/// 처리기계.
	/// 처리기계는 처리를 실행한다.
	/// </summary>
	public class ProcessExecutor : IDisposable
	{
		private bool m_IsDisposed;
		protected List<Process> m_RunningProcesses;

		public ProcessExecutor()
		{
			m_IsDisposed = false;
			m_RunningProcesses = new List<Process>();
		}

		~ProcessExecutor()
		{
			if (!m_IsDisposed)
			{
				m_IsDisposed = true;
				OnDispose(false);
			}
		}

		protected virtual void OnDispose(bool disposing)
		{
		}

		public virtual void Update(float deltaTime)
		{
			for (var i = 0; i < m_RunningProcesses.Count; ++i)
			{
				var process = m_RunningProcesses[i];
				if (process.IsFinished())
				{
					Stop(process);
					--i;
				}
				else
				{
					process.Update(deltaTime);
				}
			}
		}

		public void Dispose()
		{
			if (!m_IsDisposed)
			{
				m_IsDisposed = true;
				OnDispose(true);
			}

			// 소멸자 호출 안함.
			GC.SuppressFinalize(this);
		}

		public void Start(Process process)
		{
			if (m_RunningProcesses.Contains(process))
				return;

			m_RunningProcesses.Add(process);
			process.Reset();
			process.Execute();
		}

		public void StopAll()
		{
			for (var i = 0; i < m_RunningProcesses.Count; ++i)
			{
				var process = m_RunningProcesses[i];
				Stop(process);
				--i;
			}
		}

		public void Stop(Process process)
		{
			if (!m_RunningProcesses.Contains(process))
				return;

			if (!process.IsFinished())
				process.Finish();

			m_RunningProcesses.Remove(process);
		}

		public bool IsRunning(Process process)
		{
			return m_RunningProcesses.Contains(process);
		}
	}
}