using DagraacSystems.Core.Scripts.Common;
using System;
using System.Threading;


namespace DagraacSystemsExample
{
    /// <summary>
    /// 콘솔 어플리케이션.
    /// </summary>
    public class ConsoleApplication<TApplication> : DisposableObject where TApplication : ConsoleApplication<TApplication>, new()
	{
		//private static readonly Lazy<TApplication> s_Instance = new Lazy<TApplication>(() => new TApplication(), true); // thread-safe.
		//public static TApplication Instance => s_Instance.Variable;

		private bool m_IsQuitApplication = false;
		private long m_PrevTick = 0;

		/// <summary>
		/// 시작.
		/// </summary>
		public void Start(string[] args)
		{
			OnStart();

			var thread = new Thread(Update);
			thread.Start();

			while (true)
			{
				var readKey = Console.ReadKey();
				if (readKey.Key == ConsoleKey.Escape)
					break;

				Thread.Sleep(1);
			}

			m_IsQuitApplication = true;
			thread.Join();

			OnFinish();
		}

		private void Update()
		{
			while (!m_IsQuitApplication)
			{
				var currentTick = DateTime.Now.Ticks;
				if (m_PrevTick > 0)
				{
					var time = TimeSpan.FromTicks(currentTick - m_PrevTick);
					var deltaTime = (float)time.TotalMilliseconds * 0.001f;
					OnTick(deltaTime);
				}
				m_PrevTick = currentTick;

				Thread.Sleep(1);
			}
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnTick(float deltaTime)
		{
		}

		protected virtual void OnFinish()
		{
		}

		public void LoadModule()
		{

		}

		public static void Run(params string[] _args)
		{
			var application = new TApplication();
			application.Start(_args);
		}
	}
}