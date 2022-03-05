using DagraacSystems;


namespace DagraacSystems.FSM
{
	/// <summary>
	/// FSM 오브젝트.
	/// FSM 매니저쪽에서 고유식별자를 할당받아 사용됨.
	/// ProcessID : 실행중에만 유효한 고유식별자.
	/// InstanceID : 영구적으로 부여받은 별도 고유식별자.
	/// </summary>
	public class FSMInstance : Process
	{
		private ulong _instanceID; // 인스턴스의 아이디. create ~ destroy 까지 0이 아님.

		public string Name { private set; get; } = string.Empty;
		public bool Async { set; get; } = false; // 끝나기를 기다리지 않는 옵션.

		public FSMInstance()
		{
			_instanceID = 0;
		}

		protected virtual void OnCreate(params object[] args)
		{
			_instanceID = FSMManager.Instance.m_UniqueIdentifier.Generate();
		}

		protected virtual void OnDestroy()
		{
			FSMManager.Instance.m_UniqueIdentifier.Free(_instanceID);
			_instanceID = 0;
		}

		protected override void OnReset()
		{
			base.OnReset();
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
		}

		protected override void OnFinish()
		{
			base.OnFinish();
		}

		public ulong GetInstanceID()
		{
			return _instanceID;
		}

		internal static TFSMInstance CreateInstance<TFSMInstance>(string name, params object[] args) where TFSMInstance : FSMInstance, new()
		{
			var instance = new TFSMInstance();
			instance.Name = name;
			instance.OnCreate(args);
			return instance;
		}

		internal static void DestroyInstance(FSMInstance instance)
		{
			if (instance == null)
				return;

			if (!FSMManager.Instance.m_UniqueIdentifier.Contains(instance.GetInstanceID()))
				return;

			var processExecutor = instance.GetProcessExecutor();
			if (processExecutor != null)
				processExecutor.Stop(instance.GetProcessID(), true);

			instance.OnDestroy();
		}
	}
}