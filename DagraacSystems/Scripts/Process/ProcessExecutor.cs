using System;
using System.Collections.Generic;


namespace DagraacSystems.Process
{
	/// <summary>
	/// 프로세스 실행기.
	/// </summary>
	public class ProcessExecutor : IDisposable
	{
		private bool m_IsDisposed;
		private UniqueIdentifier m_UniqueIdentifier;
		protected Dictionary<ulong, Process> m_RunningProcesses;
		protected List<ulong> m_DeleteReservedProcessIDList;

		public ProcessExecutor()
		{
			m_IsDisposed = false;
			m_UniqueIdentifier = new UniqueIdentifier();
			m_RunningProcesses = new Dictionary<ulong, Process>();
			m_DeleteReservedProcessIDList = new List<ulong>();
		}

		public ProcessExecutor(UniqueIdentifier uniqueIdentifier)
		{
			m_IsDisposed = false;
			m_UniqueIdentifier = uniqueIdentifier;
			m_RunningProcesses = new Dictionary<ulong, Process>();
			m_DeleteReservedProcessIDList = new List<ulong>();
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
			StopAll(true);
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
				m_UniqueIdentifier.Free(processID);
			}

			m_DeleteReservedProcessIDList.Clear();
		}

		public void Start(Process process, params object[] args)
		{
			if (m_RunningProcesses.ContainsValue(process))
				return;

			var processID = m_UniqueIdentifier.Generate();
			m_RunningProcesses.Add(processID, process);
			process.Reset();
			process.Execute(this, processID, args);
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

		public void Stop(ulong processID, bool immeditate = false)
		{
			if (processID == 0)
				return;

			var process = GetRunningProcess(processID);
			Stop(process, immeditate);
		}

		public void Stop(Process process, bool immeditate = false)
		{
			if (process == null)
				return;

			if (!process.IsFinished())
				process.Finish();

			if (immeditate)
				ApplyAllDeleteReservedProcesses();
		}

		public Process GetRunningProcess(ulong processID)
		{
			if (m_RunningProcesses.TryGetValue(processID, out Process process))
				return process;
			return null;
		}

		public bool IsRunning(ulong processID)
		{
			return m_RunningProcesses.ContainsKey(processID);
		}

		public bool IsRunning(Process process)
		{
			return m_RunningProcesses.ContainsValue(process);
		}
	}
}