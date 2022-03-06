using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 프로세스 실행기.
	/// </summary>
	public class ProcessExecutor : DisposableObject
	{
		private UniqueIdentifier _uniqueIdentifier;
		protected Dictionary<ulong, Process> _runningProcesses;
		protected List<ulong> _deleteReservedProcessIDList;

		public ProcessExecutor() : base()
		{
			_uniqueIdentifier = new UniqueIdentifier();
			_runningProcesses = new Dictionary<ulong, Process>();
			_deleteReservedProcessIDList = new List<ulong>();
		}

		public ProcessExecutor(UniqueIdentifier uniqueIdentifier) : base()
		{
			_uniqueIdentifier = uniqueIdentifier;
			_runningProcesses = new Dictionary<ulong, Process>();
			_deleteReservedProcessIDList = new List<ulong>();
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

		public virtual void Update(float deltaTime)
		{
			foreach (var process in _runningProcesses)
			{
				if (_deleteReservedProcessIDList.Contains(process.Key))
					continue;
				if (process.Value.IsPaused())
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
			foreach (var process in _runningProcesses)
			{
				if (_deleteReservedProcessIDList.Contains(process.Key))
					continue;

				if (!process.Value.IsFinished())
					continue;

				_deleteReservedProcessIDList.Add(process.Key);
			}

			// 수집된 내용 삭제.
			foreach (var processID in _deleteReservedProcessIDList)
			{
				var process = GetProcess(processID);
				if (process == null)
					continue;

				if (!process.IsFinished())
					process.Finish();

				_runningProcesses.Remove(processID);
				_uniqueIdentifier.Free(processID);
			}

			_deleteReservedProcessIDList.Clear();
		}

		public TProcess Start<TProcess>(params object[] args) where TProcess : Process, new()
		{
			return (TProcess)Start(new TProcess());
		}

		public Process Start(Process process, params object[] args)
		{
			// 할당되지 않은 개체.
			if (process == null)
				return null;

			// 다른 프로세스 실행기에 의해 실행중인 개체.
			var processExecutor = process.GetProcessExecutor();
			if (processExecutor != null && processExecutor != this)
				return null;

			// 현재 프로세스에 의해 실행중인 개체.
			if (IsRunning(process))
				return null;

			// 실행.
			var processID = _uniqueIdentifier.Generate();
			_runningProcesses.Add(processID, process);
			process.Reset();
			process.Execute(this, processID, args);

			return process;
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
			foreach (var process in _runningProcesses)
			{
				if (_deleteReservedProcessIDList.Contains(process.Key))
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

			foreach (var process in _runningProcesses)
			{
				if (_deleteReservedProcessIDList.Contains(process.Key))
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
			if (_runningProcesses.TryGetValue(processID, out Process process))
			{
				if (!process.IsFinished())
					return process;
			}
			return null;
		}

		public List<Process> GetRunningProcesses()
		{
			var result = new List<Process>();
			foreach (var process in _runningProcesses.Values)
			{
				if (!process.IsFinished())
					result.Add(process);
			}

			return result;
		}

		public bool IsRunning(ulong processID)
		{
			if (_runningProcesses.TryGetValue(processID, out Process process))
			{
				if (!process.IsFinished())
					return true;
			}

			return false;
		}

		internal bool IsRunning(Process process)
		{
			if (_runningProcesses.ContainsValue(process))
			{
				if (!process.IsFinished())
					return true;
			}

			return false;
		}
	}
}