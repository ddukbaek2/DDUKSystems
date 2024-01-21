using System.Collections.Generic;


namespace DDUKSystems
{
	/// <summary>
	/// FSM 매니저.
	/// 역할은 FSMMachine의 관리 및 프로세스의 실행, 고유식별자 할당.
	/// 딱히 확장이 필요없는 객체.
	/// </summary>
	public class FSMSystem : ManagedObject
	{
		private List<FSMMachine> m_Machines;
		internal ProcessSystem m_ProcessSystem;
		internal UniqueIdentifier m_UniqueIdentifier;

		protected override void OnCreate(params object[] _args)
		{
			base.OnCreate(_args);

			m_Machines = new List<FSMMachine>();
			m_UniqueIdentifier = new UniqueIdentifier(0, 1000000000, 9999999999);
			m_ProcessSystem = new ProcessSystem(m_UniqueIdentifier);
		}

		protected override void OnDispose(bool _explicitedDispose)
		{
			base.OnDispose(_explicitedDispose);

			if (_explicitedDispose)
			{
				m_ProcessSystem.Dispose();
				m_ProcessSystem = null;
			}
		}

		public void Tick(float _tick)
		{
			m_ProcessSystem.Tick(_tick);
		}

		public TFSMMachine AddMachine<TFSMMachine>(string name, IFSMTarget target) where TFSMMachine : FSMMachine, new()
		{
			var machine = FSMObject.CreateInstance<TFSMMachine>(name);
			machine.Target = target;
			m_ProcessSystem.Start(machine);
			return machine;
		}

		public void RemoveMachine(FSMMachine machine)
		{
			if (machine == null)
				return;

			FSMObject.DestroyInstance(machine);
			m_ProcessSystem.Stop(machine.GetProcessID());
		}

		public void RemoveAllMachines()
		{
			while (m_Machines.Count > 0)
				RemoveMachine(m_Machines[0]);
		}
	}
}