using DagraacSystems;
using DagraacSystems.Log;
using System;
using System.Collections;


namespace DagraacSystemsExample
{
	/// <summary>
	/// 샘플 프로그램.
	/// </summary>
	public class ExampleApplication : ConsoleApplication<ExampleApplication>
	{
		private ProcessExecutor m_ProcessExecutor;
		private MyObject m_ExampleObject;
		private Coroutine _coroutine;

		protected override void OnStart()
		{
			base.OnStart();

			Console.WriteLine("DagraacSystems Example!");

			// 로그.
			LogManager.Instance.AddLogger(new ConsoleLogger());

			//// 테이블 로드.
			//TableManager.Instance.LoadAll();
			//var exampleTableData = ExampleTable.Instance.Find(1);

			//LogManager.Instance.Print($"{exampleTableData.Desc}");

			//m_ProcessExecutor = new ProcessExecutor();
			//m_ProcessExecutor.Start(new ExampleProcess());

			//m_ExampleObject = new MyObject();

			_coroutine = new Coroutine();
			_coroutine.Start(Process());
		}

		IEnumerator Process()
		{
			var count = 0;
			while (true)
			{
				Console.WriteLine($"count={count}");
				yield return new WaitTime(1f);
				++count;
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			//m_ProcessExecutor.Update(deltaTime);
			//DagraacSystems.FSM.FSMManager.Instance.Update(deltaTime);

			_coroutine.Tick(deltaTime);
		}

		protected override void OnFinish()
		{
			base.OnFinish();

			//m_ProcessExecutor.StopAll();
			//GC.SuppressFinalize(m_ExampleObject);
			//m_ExampleObject = null;

			//MessageBroker.Instance.Publish<NotificationType.OnTest>();
			_coroutine.Stop();
		}
	}
}