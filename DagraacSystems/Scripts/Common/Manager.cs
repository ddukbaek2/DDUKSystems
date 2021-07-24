using System;


namespace DagraacSystems
{
	/// <summary>
	/// 매니저.
	/// </summary>
	public abstract class Manager<T> : Singleton<T>, IDisposable where T : Manager<T>, new()
	{
		private bool m_IsDisposed;

		public Manager()
		{
			m_IsDisposed = false;
			//OnCreate();
		}

		//~Manager()
		//{
		//	if (!m_IsDisposed)
		//	{
		//		m_IsDisposed = true;
		//		OnDispose(false);
		//	}
		//}

		//protected abstract void OnCreate();

		protected override void OnCreate()
		{
			base.OnCreate();
		}

		protected virtual void OnDispose(bool disposing)
		{
		}

		public void Dispose()
		{
			if (!m_IsDisposed)
			{
				m_IsDisposed = true;
				OnDispose(true);
			}
			GC.SuppressFinalize(this);
		}
	}
}