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
		private long m_IncreaseID;
		protected Dictionary<long, Process> m_RunningProcesses;
		protected List<long> m_DeleteReservedProcessIDList;

		public ProcessExecutor()
		{
			m_IsDisposed = false;
			m_IncreaseID = 0;
			m_RunningProcesses = new Dictionary<long, Process>();
			m_DeleteReservedProcessIDList = new List<long>();
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
			foreach (var process in m_RunningProcesses)
			{
				if (m_DeleteReservedProcessIDList.Contains(process.Key))
					continue;
				if (process.Value.IsFinished())
					continue;

				process.Value.Update(deltaTime);
			}

			ApplyAllDeleteReservedProcesses();
		}

		/// <summary>
		/// 삭제 예정된 프로세스를 모두 수집해서 삭제.
		/// </summary>
		private void ApplyAllDeleteReservedProcesses()
		{
			// 삭제예정 수집.
			foreach (var process in m_RunningProcesses)
			{
				if (m_DeleteReservedProcessIDList.Contains(process.Key))
					continue;

				if (!process.Value.IsFinished())
					continue;

				m_DeleteReservedProcessIDList.Add(process.Key);
			}

			// 수집된 내용 삭제.
			foreach (var processID in m_DeleteReservedProcessIDList)
			{
				var process = GetRunningProcess(processID);
				if (process == null)
					continue;

				if (!process.IsFinished())
					process.Finish();

				m_RunningProcesses.Remove(processID);
			}

			m_DeleteReservedProcessIDList.Clear();
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
			if (m_RunningProcesses.ContainsValue(process))
				return;

			m_RunningProcesses.Add(++m_IncreaseID, process);
			process.Reset();
			process.Execute(this);
		}

		public void StopAll(bool immeditate = false)
		{
			foreach (var process in m_RunningProcesses)
			{
				if (m_DeleteReservedProcessIDList.Contains(process.Key))
					continue;

				Stop(process.Key, false);
			}

			if (immeditate)
				ApplyAllDeleteReservedProcesses();
		}

		public void Stop(long processID, bool immeditate = false)
		{
			var process = GetRunningProcess(processID);
			if (process == null)
				return;

			if (!process.IsFinished())
				process.Finish();

			if (immeditate)
				ApplyAllDeleteReservedProcesses();
		}
		
		public Process GetRunningProcess(long processID)
		{
			if (m_RunningProcesses.TryGetValue(processID, out Process process))
				return process;
			return null;
		}

		public bool IsRunning(long processID)
		{
			return m_RunningProcesses.ContainsKey(processID);
		}

		public bool IsRunning(Process process)
		{
			return m_RunningProcesses.ContainsValue(process);
		}
	}
}