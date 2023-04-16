using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	[Serializable]
	public struct OnModuleLoad : IMessage
	{
	}

	[Serializable]
	public struct OnModuleUnload : IMessage
	{
	}

	[Serializable]
	public struct OnModuleTick : IMessage
	{
		public float Tick;
	}


	/// <summary>
	/// 프레임워크 클래스.
	/// 모듈들을 관리한다.
	/// </summary>
	public class FrameworkSystem : DisposableObject
	{
		protected List<FModule> m_Modules;

		public UniqueIdentifier UniqueIdentifier { private set; get; }

		public MessageSystem MessageSystem { private set; get; }

		public FrameworkSystem() : base()
		{
			m_Modules = new List<FModule>();
			UniqueIdentifier = new UniqueIdentifier();
			MessageSystem = new MessageSystem();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			DisposeModuleAll();

			MessageSystem.Dispose();
			MessageSystem = null;

			UniqueIdentifier.Dispose();
			UniqueIdentifier = null;

			base.OnDispose(explicitedDispose);
		}

		public virtual void Initialize()
		{
		}

		public virtual void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		/// <summary>
		/// 새로운 모듈을 생성.
		/// </summary>
		public virtual TModule CreateModule<TModule>() where TModule : FModule, new()
		{
			var module = new TModule();
			LoadModule(module);

			return module as TModule;
		}

		/// <summary>
		/// 기존 모듈의 해제.
		/// </summary>
		public virtual void DisposeModule(FModule module)
		{
			if (module == null)
				return;

			UnloadModule(module);
			module.Dispose();
		}

		public void DisposeModuleAll()
		{
			var copiedModules = new List<FModule>(m_Modules);
			foreach (var copiedModule in copiedModules)
				DisposeModule(copiedModule);
			m_Modules.Clear();
		}

		/// <summary>
		/// 로드 모듈.
		/// </summary>
		public virtual void LoadModule(FModule _module)
		{
			if (_module == null)
				return;

			if (m_Modules.Contains(_module))
				return;

			m_Modules.Add(_module);
			MessageSystem.Send(_module, new OnModuleLoad { });
		}

		/// <summary>
		/// 언로드 모듈.
		/// </summary>
		public virtual void UnloadModule(FModule _module)
		{
			if (!m_Modules.Contains(_module))
				return;

			m_Modules.Remove(_module);
			MessageSystem.Send(_module, new OnModuleUnload { });
		}

		public TModule GetModule<TModule>() where TModule : FModule
		{
			return m_Modules.Find(_module => _module is TModule) as TModule;
		}

		public virtual void Tick(float _tick)
		{
			MessageSystem.Notify(new OnModuleTick { Tick = _tick });
		}
	}
}