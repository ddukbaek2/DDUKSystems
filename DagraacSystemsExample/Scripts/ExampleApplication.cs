using DagraacSystems;
using System;
using System.Collections;


namespace DagraacSystemsExample
{
	/// <summary>
	/// 샘플 프로그램.
	/// </summary>
	public class ExampleApplication : ConsoleApplication<ExampleApplication>
	{
		private ProcessSystem m_ProcessExecutor;
		private MyObject m_ExampleObject;
		private Coroutine m_Coroutine;

		protected override void OnStart()
		{
			base.OnStart();

			Console.WriteLine("DagraacSystems Example!");

			// 로거 등록.
			Debug.AddLogger(new DebugImpl());

			//// 테이블 로드.
			//TableManager.Instance.LoadAll();
			//var exampleTableData = ExampleTable.Instance.Find(1);

			//LogManager.Instance.Print($"{exampleTableData.Desc}");

			//m_ProcessExecutor = new ProcessExecutor();
			//m_ProcessExecutor.Start(new ExampleProcess());

			//m_ExampleObject = new MyObject();

			IEnumerator Process()
			{
				var count = 0;
				while (true)
				{
					Console.WriteLine($"count={count}");
					yield return new WaitForSeconds(1f);
					++count;
				}
			}

			m_Coroutine = new Coroutine();
			m_Coroutine.Start(Process());
		}

		protected override void OnTick(float _tick)
		{
			base.OnTick(_tick);

			//m_ProcessExecutor.Tick(deltaTime);
			//DagraacSystems.FSMManager.Instance.Tick(deltaTime);

			m_Coroutine.Tick(_tick);
		}

		protected override void OnFinish()
		{
			base.OnFinish();

			//m_ProcessExecutor.StopAll();
			//GC.SuppressFinalize(m_ExampleObject);
			//m_ExampleObject = null;

			//MessageBroker.Instance.Publish<NotificationType.OnTest>();
			m_Coroutine.Stop();
		}
	}
}