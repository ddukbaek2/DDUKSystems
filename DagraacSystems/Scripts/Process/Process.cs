using System;
using System.Collections.Generic;


namespace DagraacSystems.Process
{
	/// <summary>
	/// 처리기계를 상속받은 처리.
	/// 처리가 처리를 실행한다.
	/// </summary>
	public class Process
	{
		private bool m_IsFinished;
		private ProcessExecutor m_Executor;
		private long m_ID;

		public Process()
		{
			m_IsFinished = false;
			m_Executor = null;
			m_ID = 0;
		}

		public virtual void Reset()
		{
			m_IsFinished = false;
			m_Executor = null;
			m_ID = 0;
		}

		public virtual void Execute(ProcessExecutor processExecutor)
		{
			m_Executor = processExecutor;
		}

		public virtual void Update(float deltaTime)
		{
		}

		public virtual void Finish()
		{
			m_IsFinished = true;
		}

		public virtual bool IsFinished() => m_IsFinished;

		public ProcessExecutor GetExecutor() => m_Executor;
	}
}