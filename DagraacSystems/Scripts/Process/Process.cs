using System;
using System.Collections.Generic;


namespace DagraacSystems.Process
{
	/// <summary>
	/// 
	/// </summary>
	public class Process
	{
		private bool m_IsFinished;
		private ProcessExecutor m_ProcessExecutor;
		private ulong m_ProcessID;

		public Process()
		{
			m_IsFinished = false;
			m_ProcessExecutor = null;
			m_ProcessID = 0;
		}

		internal void Reset()
		{
			m_IsFinished = false;
			m_ProcessExecutor = null;
			m_ProcessID = 0;
			OnReset();
		}

		internal void Execute(ProcessExecutor processExecutor, ulong processID)
		{
			m_ProcessExecutor = processExecutor;
			m_ProcessID = processID;
			OnExecute();
		}

		internal void Update(float deltaTime)
		{
			OnUpdate(deltaTime);
		}

		public void Finish()
		{
			if (m_IsFinished)
				return;

			m_IsFinished = true;
			OnFinish();
		}

		protected virtual void OnReset()
		{
		}

		protected virtual void OnExecute()
		{
		}

		protected virtual void OnUpdate(float deltaTime)
		{
		}

		protected virtual void OnFinish()
		{
		}

		public virtual bool IsFinished() => m_IsFinished;

		public ProcessExecutor GetProcessExecutor() => m_ProcessExecutor;

		public ulong GetProcessID() => m_ProcessID;
	}
}