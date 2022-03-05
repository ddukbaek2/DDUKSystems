using System;
using System.Collections.Generic;


namespace DagraacSystems.Framework
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
	public class Framework : DisposableObject
	{
		protected Messenger _messenger;
		protected UniqueIdentifier _uniqueIdentifier;
		protected Dictionary<Type, Module> _modules;

		public UniqueIdentifier UniqueIdentifier => _uniqueIdentifier;
		public Messenger Messenger => _messenger;

		public Framework() : base()
		{
			_messenger = new Messenger();
			_uniqueIdentifier = new UniqueIdentifier();
			_modules = new Dictionary<Type, Module>();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			DisposeModuleAll();

			_messenger.Dispose();
			_messenger = null;

			_uniqueIdentifier.Dispose();
			_uniqueIdentifier = null;

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

		public virtual TModule CreateModule<TModule>() where TModule : Module, new()
		{
			var module = Module.Create<TModule>(this);
			LoadModule(module);
			return module as TModule;
		}

		public virtual void DisposeModule(Module module)
		{
			if (module == null)
			{
				return;
			}

			UnloadModule(module);
			module.Dispose();
		}

		public void DisposeModuleAll()
		{
			var copiedModules = new List<Module>(_modules.Values);
			foreach (var copiedModule in copiedModules)
				DisposeModule(copiedModule);

			//foreach (var module in _modules)
			//	module.Value.Dispose();
			_modules.Clear();
		}


		public virtual void LoadModule(Module module)
		{
			var moduleType = module.GetType();
			if (_modules.ContainsKey(moduleType))
			{
				return;
			}

			_modules.Add(moduleType, module);
			_messenger.Send(module, new OnModuleLoad { });
		}

		public virtual void UnloadModule(Module module)
		{
			var moduleType = module.GetType();
			if (!_modules.ContainsKey(moduleType))
			{
				return;
			}

			_modules.Remove(moduleType);
			_messenger.Send(module, new OnModuleUnload { });
		}

		public T GetModule<T>() where T : Module
		{
			return _modules[typeof(T)] as T;
		}

		public virtual void FrameMove(float deltaTime)
		{
			_messenger.Notify(new OnModuleUpdate { DeltaTime = deltaTime });
		}
	}
}