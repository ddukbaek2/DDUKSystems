using DagraacSystems.Process;
using System.Collections.Generic;


namespace DagraacSystems.FSM
{
	/// <summary>
	/// FSM 매니저.
	/// 역할은 FSMMachine의 관리 및 프로세스의 실행, 고유식별자 할당.
	/// 딱히 확장이 필요없는 객체.
	/// </summary>
	public class FSMManager : Manager<FSMManager>
	{
		private List<FSMMachine> m_Machines;
		internal ProcessExecutor m_ProcessExecutor;
		internal UniqueIdentifier m_UniqueIdentifier;

		protected override void OnCreate()
		{
			base.OnCreate();

			m_Machines = new List<FSMMachine>();
			m_UniqueIdentifier = new UniqueIdentifier(0, 1000000000, 9999999999);
			m_ProcessExecutor = new ProcessExecutor(m_UniqueIdentifier);
		}

		protected override void OnDispose(bool disposing)
		{
			base.OnDispose(disposing);

			if (disposing)
			{
				m_ProcessExecutor.Dispose();
				m_ProcessExecutor = null;
			}
		}

		public TFSMMachine AddMachine<TFSMMachine>(IFSMTarget target) where TFSMMachine : FSMMachine, new()
		{
			var machine = FSMInstance.CreateInstance<TFSMMachine>(target);
			m_ProcessExecutor.Start(machine);
			return machine;
		}

		public void RemoveMachine(FSMMachine machine)
		{
			if (machine == null)
				return;

			FSMInstance.DestroyInstance(machine);
			m_ProcessExecutor.Stop(machine.GetProcessID());
		}

		public void RemoveAllMachines()
		{
			while (m_Machines.Count > 0)
				RemoveMachine(m_Machines[0]);
		}
	}
}