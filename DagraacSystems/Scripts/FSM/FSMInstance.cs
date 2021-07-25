using DagraacSystems.Process;

namespace DagraacSystems.FSM
{
	public class FSMInstance : DagraacSystems.Process.Process
	{
		private ulong m_InstanceID;
		private bool m_Async;

		public FSMInstance()
		{
			m_InstanceID = 0;
			m_Async = false;
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

		protected override void OnExecute()
		{
			base.OnExecute();
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

			instance.OnDestroy();
		}
	}
}