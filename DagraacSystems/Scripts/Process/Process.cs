using System;
using System.Collections.Generic;


namespace DagraacSystems.Process
{
	/// <summary>
	/// 
	/// </summary>
	public class Process
	{
		private bool m_IsStarted;
		private bool m_IsFinished;
		private ProcessExecutor m_ProcessExecutor;
		private ulong m_ProcessID;

		public Process()
		{
			m_IsStarted = false;
			m_IsFinished = false;
			m_ProcessExecutor = null;
			m_ProcessID = 0;
		}

		internal void Reset()
		{
			m_IsStarted = false;
			m_IsFinished = false;
			m_ProcessExecutor = null;
			m_ProcessID = 0;

			OnReset();
		}

		internal void Execute(ProcessExecutor processExecutor, ulong processID, params object[] args)
		{
			m_IsStarted = true;
			m_IsFinished = false;
			m_ProcessExecutor = processExecutor;
			m_ProcessID = processID;

			OnExecute(args);
		}

		internal void Update(float deltaTime)
		{
			OnUpdate(deltaTime);
		}

		public void Finish()
		{
			if (!m_IsStarted || m_IsFinished)
				return;

			m_IsStarted = false;
			m_IsFinished = true;

			OnFinish();
		}

		protected virtual void OnReset()
		{
		}

		protected virtual void OnExecute(params object[] args)
		{
		}

		protected virtual void OnUpdate(float deltaTime)
		{
		}

		protected virtual void OnFinish()
		{
		}

		public bool IsStarted()
		{
			return m_IsStarted;
		}
		
		public bool IsFinished()
		{
			return m_IsFinished;
		}

		public ProcessExecutor GetProcessExecutor()
		{
			return m_ProcessExecutor;
		}

		public ulong GetProcessID()
		{
			return m_ProcessID;
		}
	}
}