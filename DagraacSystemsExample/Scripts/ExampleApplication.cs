﻿using DagraacSystems;
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
			Debug.AddLogger(new ConsoleLogger());

			//// 테이블 로드.
			//TableManager.Instance.LoadAll();
			//var exampleTableData = ExampleTable.Instance.Find(1);

			//LogManager.Instance.Print($"{exampleTableData.Desc}");

			//m_ProcessExecutor = new ProcessExecutor();
			//m_ProcessExecutor.Start(new ExampleProcess());

			//m_ExampleObject = new MyObject();

			m_Coroutine = new Coroutine();
			m_Coroutine.Start(Process());
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

			m_Coroutine.Update(deltaTime);
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