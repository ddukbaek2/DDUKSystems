using System;
using System.Threading;
using DagraacSystems.Process;


namespace DagraacSystemsExample
{
	public class Program
	{
		private static bool s_IsQuitApplication = false;
		private static long s_PrevTick = 0;
		public static void Main(string[] args)
		{
			var thread = new Thread(Logic);
			thread.Start();

			while (true)
			{
				var readKey = Console.ReadKey();
				if (readKey.Key == ConsoleKey.Escape)
					break;
	
				Thread.Sleep(1);
			}

			s_IsQuitApplication = true;
			thread.Join();
		}

		private static void Logic()
		{
			TableManager.Instance.LoadAll();

			Console.WriteLine("DagraacSystems Example!");
			Console.WriteLine($"{ExampleTable.Instance.Find(1).Desc}");

			var processExecutor = new ProcessExecutor();
			processExecutor.Start(new ExampleProcess());

			while (!s_IsQuitApplication)
			{
				var currentTick = DateTime.Now.Ticks;
				if (s_PrevTick > 0)
				{
					var time = TimeSpan.FromTicks(currentTick - s_PrevTick);
					var deltaTime = (float)time.TotalMilliseconds * 0.001f;
					processExecutor.Update(deltaTime);
				}
				s_PrevTick = currentTick;

				Thread.Sleep(1);
			}

			processExecutor.StopAll();
		}
	}
}