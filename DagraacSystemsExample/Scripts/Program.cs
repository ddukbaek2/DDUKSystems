using System;
using System.Threading;
using DagraacSystems.Process;


namespace DagraacSystemsExample
{
	/// <summary>
	/// 샘플 프로그램.
	/// </summary>
	public class ExampleApplication : ConsoleApplication<ExampleApplication>
	{
		private ProcessExecutor m_ProcessExecutor;
		private ExampleObject m_ExampleObject;

		protected override void OnStart()
		{
			base.OnStart();

			Console.WriteLine("DagraacSystems Example!");

			TableManager.Instance.LoadAll();
			var exampleTableData = ExampleTable.Instance.Find(1);

			Console.WriteLine($"{exampleTableData.Desc}");

			m_ProcessExecutor = new ProcessExecutor();
			m_ProcessExecutor.Start(new ExampleProcess());

			m_ExampleObject = new ExampleObject();
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			m_ProcessExecutor.Update(deltaTime);
		}

		protected override void OnFinish()
		{
			base.OnFinish();

			m_ProcessExecutor.StopAll();
			GC.SuppressFinalize(m_ExampleObject);
			m_ExampleObject = null;

			NotificationManager.Instance.Notify<DefinedDelegate.OnCreate>();

			Console.ReadKey();
		}
	}


	/// <summary>
	/// 프로그램 진입점.
	/// </summary>
	public class Program
	{
		public static void Main(string[] args)
		{
			ExampleApplication.Instance.Start(args);
		}
	}
}