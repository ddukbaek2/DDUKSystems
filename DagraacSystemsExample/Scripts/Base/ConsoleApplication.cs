using System;
using System.Threading;


namespace DagraacSystemsExample
{
	public class ConsoleApplication<T> where T : ConsoleApplication<T>, new()
	{
		private static readonly Lazy<T> m_Instance = new Lazy<T>(() => new T(), true); // thread-safe.
		public static T Instance => m_Instance.Value;

		private bool m_IsQuitApplication = false;
		private long m_PrevTick = 0;

		public void Start(string[] args)
		{
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
		}

		private void Update()
		{
			OnStart();

			while (!m_IsQuitApplication)
			{
				var currentTick = DateTime.Now.Ticks;
				if (m_PrevTick > 0)
				{
					var time = TimeSpan.FromTicks(currentTick - m_PrevTick);
					var deltaTime = (float)time.TotalMilliseconds * 0.001f;
					OnUpdate(deltaTime);
				}
				m_PrevTick = currentTick;

				Thread.Sleep(1);
			}

			OnFinish();
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnUpdate(float deltaTime)
		{
		}

		protected virtual void OnFinish()
		{
		}
	}
}