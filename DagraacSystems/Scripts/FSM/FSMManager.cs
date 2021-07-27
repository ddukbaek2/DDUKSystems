using DagraacSystems.Process;


namespace DagraacSystems.FSM
{
	/// <summary>
	/// FSM 매니저.
	/// 역할은 FSMMachine의 관리 및 프로세스의 실행, 고유식별자 할당.
	/// 딱히 확장이 필요없는 객체.
	/// </summary>
	public class FSMManager : Manager<FSMManager>
	{
		internal ProcessExecutor m_ProcessExecutor;
		internal UniqueIdentifier m_UniqueIdentifier;

		protected override void OnCreate()
		{
			base.OnCreate();

			m_UniqueIdentifier = new UniqueIdentifier(0, 1000000000, 9999999999);
			m_ProcessExecutor = new ProcessExecutor(m_UniqueIdentifier);
		}

		protected override void OnDispose(bool disposing)
		{
			base.OnDispose(disposing);

			m_ProcessExecutor.Dispose();
			m_ProcessExecutor = null;
		}

		public TFSMMachine CreateMachine<TFSMMachine>(IFSMTarget target) where TFSMMachine : FSMMachine, new()
		{
			var machine = new TFSMMachine();
			machine.Target = target;
			m_ProcessExecutor.Start(machine);
			return machine;
		}

		public void DestroyMachine(FSMMachine machine)
		{
			if (machine == null)
				return;

			m_ProcessExecutor.Stop(machine.GetProcessID());
		}
	}
}