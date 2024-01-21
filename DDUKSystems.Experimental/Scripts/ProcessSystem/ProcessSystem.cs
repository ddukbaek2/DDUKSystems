using System;
using System.Collections.Generic;
using DDUKSystems.Core.Scripts.Utility;

namespace DDUKSystems
{
	/// <summary>
	/// 프로세스 시스템.
	/// </summary>
	public class ProcessSystem : DisposableObject
	{
		private UniqueIdentifier m_UniqueIdentifier;
		protected Dictionary<ulong, Process> m_RunningProcesses;
		protected List<ulong> m_DeleteReservedProcessIdentifiers;

		public ProcessSystem() : base()
		{
			m_UniqueIdentifier = new UniqueIdentifier();
			m_RunningProcesses = new Dictionary<ulong, Process>();
			m_DeleteReservedProcessIdentifiers = new List<ulong>();
		}

		public ProcessSystem(UniqueIdentifier uniqueIdentifier) : base()
		{
			m_UniqueIdentifier = uniqueIdentifier;
			m_RunningProcesses = new Dictionary<ulong, Process>();
			m_DeleteReservedProcessIdentifiers = new List<ulong>();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			StopAll(true);

			base.OnDispose(explicitedDispose);
		}

		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		public virtual void Tick(float _tick)
		{
			foreach (var process in m_RunningProcesses)
			{
				if (m_DeleteReservedProcessIdentifiers.Contains(process.Key))
					continue;
				if (process.Value.IsPaused())
					continue;
				if (process.Value.IsFinished())
					continue;

				process.Value.Tick(_tick);
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
				if (m_DeleteReservedProcessIdentifiers.Contains(process.Key))
					continue;

				if (!process.Value.IsFinished())
					continue;

				m_DeleteReservedProcessIdentifiers.Add(process.Key);
			}

			// 수집된 내용 삭제.
			foreach (var processID in m_DeleteReservedProcessIdentifiers)
			{
				var process = GetProcess(processID);
				if (process == null)
					continue;

				if (!process.IsFinished())
					process.Finish();

				m_RunningProcesses.Remove(processID);
				m_UniqueIdentifier.Delete(processID);
			}

			m_DeleteReservedProcessIdentifiers.Clear();
		}

		public TProcess Start<TProcess>(params object[] args) where TProcess : Process, new()
		{
			return (TProcess)Start(new TProcess());
		}

		public Process Start(Process _process, params object[] args)
		{
			// 할당되지 않은 개체.
			if (_process == null)
				return null;

			// 다른 프로세스 실행기에 의해 실행중인 개체.
			var processExecutor = _process.GetProcessExecutor();
			if (processExecutor != null && processExecutor != this)
				return null;

			// 현재 프로세스에 의해 실행중인 개체.
			if (IsRunning(_process))
				return null;

			// 실행.
			var processID = m_UniqueIdentifier.New();
			m_RunningProcesses.Add(processID, _process);
			_process.Reset();
			_process.Execute(this, processID, args);

			return _process;
		}

		public void Pause(ulong processID)
		{
			Pause(GetProcess(processID));
		}

		internal void Pause(Process process)
		{
			// 할당되지 않은 개체.
			if (process == null)
				return;

			// 다른 프로세스 실행기에 의해 실행중인 개체.
			var processExecutor = process.GetProcessExecutor();
			if (processExecutor != null && processExecutor != this)
				return;

			// 현재 프로세스에 의해 실행중이지 않은 개체.
			if (!IsRunning(process))
				return;

			process.Pause();
		}

		public void Resume(ulong processID)
		{
			Resume(GetProcess(processID));
		}

		internal void Resume(Process process)
		{
			// 할당되지 않은 개체.
			if (process == null)
				return;

			// 다른 프로세스 실행기에 의해 실행중인 개체.
			var processExecutor = process.GetProcessExecutor();
			if (processExecutor != null && processExecutor != this)
				return;

			// 현재 프로세스에 의해 실행중이지 않은 개체.
			if (!IsRunning(process))
				return;

			process.Resume();
		}

		public void StopAll(bool immeditate = false)
		{
			foreach (var process in m_RunningProcesses)
			{
				if (m_DeleteReservedProcessIdentifiers.Contains(process.Key))
					continue;

				Stop(process.Key, false);
			}

			if (immeditate)
				ApplyAllDeleteReservedProcesses();
		}

		public void StopAllIf(Predicate<Process> match, bool immeditate = false)
		{
			if (match == null)
				return;

			foreach (var process in m_RunningProcesses)
			{
				if (m_DeleteReservedProcessIdentifiers.Contains(process.Key))
					continue;

				if (!match(process.Value))
					continue;

				Stop(process.Key, false);
			}

			if (immeditate)
				ApplyAllDeleteReservedProcesses();
		}

		public void Stop(ulong processID, bool immeditate = false)
		{
			Stop(GetProcess(processID), immeditate);
		}

		internal void Stop(Process process, bool immeditate = false)
		{
			// 할당되지 않은 개체.
			if (process == null)
				return;

			// 다른 프로세스 실행기에 의해 실행중인 개체.
			var processExecutor = process.GetProcessExecutor();
			if (processExecutor != null && processExecutor != this)
				return;

			// 현재 프로세스에 의해 실행중이지 않은 개체.
			if (!IsRunning(process))
				return;

			if (!process.IsFinished())
				process.Finish();

			if (immeditate)
				ApplyAllDeleteReservedProcesses();
		}

		internal Process GetProcess(ulong processID)
		{
			if (m_RunningProcesses.TryGetValue(processID, out Process process))
			{
				if (!process.IsFinished())
					return process;
			}
			return null;
		}

		public List<Process> GetRunningProcesses()
		{
			var result = new List<Process>();
			foreach (var process in m_RunningProcesses.Values)
			{
				if (!process.IsFinished())
					result.Add(process);
			}

			return result;
		}

		public bool IsRunning(ulong processID)
		{
			if (m_RunningProcesses.TryGetValue(processID, out Process process))
			{
				if (!process.IsFinished())
					return true;
			}

			return false;
		}

		internal bool IsRunning(Process process)
		{
			if (m_RunningProcesses.ContainsValue(process))
			{
				if (!process.IsFinished())
					return true;
			}

			return false;
		}
	}
}