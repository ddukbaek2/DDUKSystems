using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	[Serializable]
	public struct OnObjectCreate : IMessage
	{
	}

	[Serializable]
	public struct OnModuleLoad : IMessage
	{
	}

	[Serializable]
	public struct OnModuleUnload : IMessage
	{
	}

	[Serializable]
	public struct OnModuleUpdate : IMessage
	{
		public float DeltaTime;
	}


	/// <summary>
	/// 프레임워크 클래스.
	/// 모듈들을 관리한다.
	/// </summary>
	public class FrameworkSystem : DisposableObject
	{
		protected MessageSystem m_MessageSystem;
		protected UniqueIdentifier m_UniqueIdentifier;
		protected List<Module> m_Modules;

		public UniqueIdentifier UniqueIdentifier => m_UniqueIdentifier;
		public MessageSystem Messenger => m_MessageSystem;

		public FrameworkSystem() : base()
		{
			m_MessageSystem = new MessageSystem();
			m_UniqueIdentifier = new UniqueIdentifier();
			m_Modules = new List<Module>();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			DisposeModuleAll();

			m_MessageSystem.Dispose();
			m_MessageSystem = null;

			m_UniqueIdentifier.Dispose();
			m_UniqueIdentifier = null;

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
		public virtual TModule CreateModule<TModule>() where TModule : Module, new()
		{
			var module = new TModule();

			LoadModule(module);
			return module as TModule;
		}

		/// <summary>
		/// 기존 모듈의 해제.
		/// </summary>
		public virtual void DisposeModule(Module module)
		{
			if (module == null)
				return;

			UnloadModule(module);
			module.Dispose();
		}

		public void DisposeModuleAll()
		{
			var copiedModules = new List<Module>(m_Modules);
			foreach (var copiedModule in copiedModules)
				DisposeModule(copiedModule);
			m_Modules.Clear();
		}

		/// <summary>
		/// 로드 모듈.
		/// </summary>
		public virtual void LoadModule(Module module)
		{
			if (module == null)
				return;

			if (m_Modules.Contains(module))
				return;

			m_Modules.Add(module);
			m_MessageSystem.Send(module, new OnModuleLoad { });
		}

		/// <summary>
		/// 언로드 모듈.
		/// </summary>
		public virtual void UnloadModule(Module module)
		{
			if (!m_Modules.Contains(module))
				return;

			m_Modules.Remove(module);
			m_MessageSystem.Send(module, new OnModuleUnload { });
		}

		public TModule GetModule<TModule>() where TModule : Module
		{
			return m_Modules.Find(_module => _module is TModule) as TModule;
		}

		public virtual void FrameMove(float deltaTime)
		{
			m_MessageSystem.Notify(new OnModuleUpdate { DeltaTime = deltaTime });
		}
	}
}