using DagraacSystems.FSM;
using System;


namespace DagraacSystemsExample
{
	public class NotificationManager : DagraacSystems.Notification
	{
		private static readonly Lazy<NotificationManager> m_Instance = new Lazy<NotificationManager>(() => new NotificationManager(), true); // thread-safe.
		public static NotificationManager Instance => m_Instance.Value;
	}

	public class DefinedDelegate
	{
		public delegate void OnTest();
	}

	public class ExampleObject : IFSMTarget
	{
		private FSMMachine m_FSMMachine { set; get; }

		public ExampleObject()
		{
			NotificationManager.Instance.Register<DefinedDelegate.OnTest>(OnTest);
			m_FSMMachine = FSMManager.Instance.AddMachine<FSMMachine>(this);
			var idleState = m_FSMMachine.AddState<IdleState>();
			m_FSMMachine.RunState(idleState);
		}

		~ExampleObject()
		{
			NotificationManager.Instance.Unregister<DefinedDelegate.OnTest>(OnTest);
		}

		void IFSMTarget.OnChangeState(FSMMachine machine, FSMState state)
		{
		}

		private void OnTest()
		{
			Console.WriteLine("OnCreate()");
		}
	}
}