using DagraacSystems;
using System;
using System.Threading;


namespace DagraacSystemsExample
{
	/// <summary>
	/// 콘솔 어플리케이션.
	/// </summary>
	public class ConsoleApplication<T> : DisposableObject where T : ConsoleApplication<T>, new()
	{
		private static readonly Lazy<T> m_Instance = new Lazy<T>(() => new T(), true); // thread-safe.
		public static T Instance => m_Instance.Value;

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
					OnUpdate(deltaTime);
				}
				m_PrevTick = currentTick;

				Thread.Sleep(1);
			}
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

		public void LoadModule()
		{

		}
	}
}