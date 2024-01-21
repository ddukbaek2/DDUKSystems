﻿namespace DDUKSystems
{
	/// <summary>
	/// 처리 단위.
	/// </summary>
	public class Process : ManagedObject, IProcess
	{
		private bool m_IsStarted;
		private bool m_IsFinished;
		private bool m_IsPaused;

		private ProcessSystem m_ProcessSystem;
		private ulong m_ProcessID; // 객체의 프로세스 아이디. execute ~ finish 까지 0이 아님.

		protected override void OnCreate(params object[] _args)
		{
			base.OnCreate(_args);

			m_IsStarted = false;
			m_IsFinished = false;
			m_IsPaused = false;
			m_ProcessSystem = null;
			m_ProcessID = 0;
		}

		protected override void OnDispose(bool _explicitedDispose)
		{
			base.OnDispose(_explicitedDispose);
		}

		public void Reset()
		{
			m_IsStarted = false;
			m_IsFinished = false;
			m_IsPaused = false;
			m_ProcessSystem = null;
			m_ProcessID = 0;

			OnReset();
		}

		public void Execute(ProcessSystem processExecutor, ulong processID, params object[] args)
		{
			m_IsStarted = true;
			m_IsFinished = false;
			m_ProcessSystem = processExecutor;
			m_ProcessID = processID;

			OnExecute(args);
		}

		public void Tick(float deltaTime)
		{
			OnTick(deltaTime);
		}

		public void Finish()
		{
			if (!m_IsStarted || m_IsFinished)
				return;

			m_IsStarted = false;
			m_IsFinished = true;

			OnFinish();
		}

		public void Resume()
		{
			if (m_IsPaused)
			{
				m_IsPaused = false;
				OnResume();
			}
		}

		public void Pause()
		{
			if (!m_IsPaused)
			{
				m_IsPaused = true;
				OnPause();
			}
		}

		protected virtual void OnReset()
		{
		}

		protected virtual void OnExecute(params object[] _args)
		{
		}

		protected virtual void OnTick(float _tick)
		{
		}

		protected virtual void OnPause()
		{
		}

		protected virtual void OnResume()
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

		public bool IsPaused()
		{
			return m_IsPaused;
		}

		public ProcessSystem GetProcessExecutor()
		{
			return m_ProcessSystem;
		}

		public ulong GetProcessID()
		{
			return m_ProcessID;
		}
	}
}