using DagraacSystems;


namespace DagraacSystems
{
	/// <summary>
	/// FSM 오브젝트.
	/// FSM 매니저쪽에서 고유식별자를 할당받아 사용됨.
	/// ProcessID : 실행중에만 유효한 고유식별자.
	/// InstanceID : 영구적으로 부여받은 별도 고유식별자.
	/// </summary>
	public class FSMObject : Process
	{
		private FSMSystem m_FSMSystem;
		private ulong m_InstanceID; // 인스턴스의 아이디. create ~ destroy 까지 0이 아님.

		public string Name { private set; get; } = string.Empty;
		public bool Async { set; get; } = false; // 끝나기를 기다리지 않는 옵션.

		public FSMObject()
		{
			m_InstanceID = 0;
		}

		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);

			m_InstanceID = m_FSMSystem.m_UniqueIdentifier.New();
		}

		protected virtual void OnDestroy()
		{
			m_FSMSystem.m_UniqueIdentifier.Delete(m_InstanceID);
			m_InstanceID = 0;
		}

		protected override void OnReset()
		{
			base.OnReset();
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);
		}

		protected override void OnTick(float deltaTime)
		{
			base.OnTick(deltaTime);
		}

		protected override void OnFinish()
		{
			base.OnFinish();
		}

		public ulong GetInstanceID()
		{
			return m_InstanceID;
		}

		internal static TFSMInstance CreateInstance<TFSMInstance>(string _name, params object[] _args) where TFSMInstance : FSMObject, new()
		{
			var instance = new TFSMInstance();
			instance.Name = _name;
			instance.OnCreate(_args);
			return instance;
		}

		internal static void DestroyInstance(FSMObject _instance)
		{
			if (_instance == null)
				return;

			if (!_instance.m_FSMSystem.m_UniqueIdentifier.Contains(_instance.GetInstanceID()))
				return;

			var processExecutor = _instance.GetProcessExecutor();
			if (processExecutor != null)
				processExecutor.Stop(_instance.GetProcessID(), true);

			_instance.OnDestroy();
		}
	}
}