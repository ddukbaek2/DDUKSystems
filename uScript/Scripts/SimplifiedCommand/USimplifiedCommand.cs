using System;
using System.Collections.Generic;


namespace uScript
{
	/// <summary>
	/// 간략한 명령.
	/// </summary>
	public abstract class USimplifiedCommand : IDisposable
	{
		/// <summary>
		/// 해제됨 여부.
		/// </summary>
		private bool m_IsDisposed;

		/// <summary>
		/// 이름.
		/// </summary>
		private string m_Name;

		/// <summary>
		/// 부모.
		/// </summary>
		private USimplifiedCommand m_Parent;

		/// <summary>
		/// 자식.
		/// </summary>
		private Dictionary<string, USimplifiedCommand> m_Children;

		/// <summary>
		/// 해제됨 여부.
		/// </summary>
		public bool IsDisposed => m_IsDisposed;

		/// <summary>
		/// 이름.
		/// </summary>
		public string Name => m_Name;

		/// <summary>
		/// 부모.
		/// </summary>
		public USimplifiedCommand Parent => m_Parent;

		/// <summary>
		/// 자식.
		/// </summary>
		public Dictionary<string, USimplifiedCommand> Children => m_Children;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public USimplifiedCommand(string name, USimplifiedCommand parent = null)
		{
			m_IsDisposed = false;
			m_Name = name;
			m_Parent = parent;
			m_Children = new Dictionary<string, USimplifiedCommand>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		~USimplifiedCommand()
		{
			if (m_IsDisposed)
				return;

			m_IsDisposed = true;
			OnDispose(false);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected virtual void OnDispose(bool explicitDispose)
		{
			// TODO.
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public virtual void Dispose()
		{
			if (m_IsDisposed)
				return;

			m_IsDisposed = true;
			GC.SuppressFinalize(this);
			OnDispose(true);
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public abstract void Execute(params object[] arguments);
	}
}