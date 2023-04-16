using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// FSM 매니저.
	/// 역할은 FSMMachine의 관리 및 프로세스의 실행, 고유식별자 할당.
	/// 딱히 확장이 필요없는 객체.
	/// </summary>
	public class FSMManager : SharedClass<FSMManager>
	{
		private List<FSMMachine> _machines;
		internal ProcessSystem _processExecutor;
		internal UniqueIdentifier _uniqueIdentifier;

		protected override void OnCreate()
		{
			base.OnCreate();

			_machines = new List<FSMMachine>();
			_uniqueIdentifier = new UniqueIdentifier(0, 1000000000, 9999999999);
			_processExecutor = new ProcessSystem(_uniqueIdentifier);
		}

		protected override void OnDispose(bool disposing)
		{
			base.OnDispose(disposing);

			if (disposing)
			{
				_processExecutor.Dispose();
				_processExecutor = null;
			}
		}

		public void Update(float deltaTime)
		{
			_processExecutor.Update(deltaTime);
		}

		public TFSMMachine AddMachine<TFSMMachine>(string name, IFSMTarget target) where TFSMMachine : FSMMachine, new()
		{
			var machine = FSMInstance.CreateInstance<TFSMMachine>(name);
			machine.Target = target;
			_processExecutor.Start(machine);
			return machine;
		}

		public void RemoveMachine(FSMMachine machine)
		{
			if (machine == null)
				return;

			FSMInstance.DestroyInstance(machine);
			_processExecutor.Stop(machine.GetProcessID());
		}

		public void RemoveAllMachines()
		{
			while (_machines.Count > 0)
				RemoveMachine(_machines[0]);
		}
	}
}