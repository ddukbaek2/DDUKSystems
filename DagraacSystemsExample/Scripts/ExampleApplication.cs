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
		private ProcessExecutor _processExecutor;
		private MyObject _exampleObject;
		private Coroutine _coroutine;

		protected override void OnStart()
		{
			base.OnStart();

			Console.WriteLine("DagraacSystems Example!");

			// 로거 등록.
			Debug.AddLogger(new ConsoleLogger());

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
				yield return new WaitForSeconds(1f);
				++count;
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			//m_ProcessExecutor.Update(deltaTime);
			//DagraacSystems.FSMManager.Instance.Update(deltaTime);

			_coroutine.Update(deltaTime);
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