using DagraacSystems.Process;

namespace DagraacSystems.FSM
{
	/// <summary>
	/// FSM 오브젝트.
	/// FSM 매니저쪽에서 고유식별자를 할당받아 사용됨.
	/// ProcessID : 실행중에만 유효한 고유식별자.
	/// InstanceID : 영구적으로 부여받은 별도 고유식별자.
	/// </summary>
	public class FSMInstance : DagraacSystems.Process.Process
	{
		private ulong m_InstanceID; // 인스턴스의 아이디.

		public bool Async { set; get; } = false; // 끝나기를 기다리지 않는 옵션.

		public FSMInstance()
		{
			m_InstanceID = 0;
		}

		protected virtual void OnCreate()
		{
			m_InstanceID = FSMManager.Instance.m_UniqueIdentifier.Generate();
		}

		protected virtual void OnDestroy()
		{
			FSMManager.Instance.m_UniqueIdentifier.Free(m_InstanceID);
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
			return m_InstanceID;
		}

		public static TFSMInstance CreateInstance<TFSMInstance>() where TFSMInstance : FSMInstance, new()
		{
			var instance = new TFSMInstance();
			instance.OnCreate();
			return instance;
		}

		public static void DestroyInstance(FSMInstance instance)
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