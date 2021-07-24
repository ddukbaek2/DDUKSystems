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


		private static int[] Convert(string hexColor)
		{
			var rgb = new int[3];

			var prevHexNumber = 0;
			var index = 0;
			foreach (var hexChar in hexColor)
			{
				if (hexChar == 35) // #
					continue;

				var hexNumber = 0;
				if (hexChar > 47 && hexChar < 58) // 숫자 0~9
					hexNumber = hexChar - 48;
				else if (hexChar > 64 && hexChar < 71) // 대문자 A~F.
					hexNumber = (hexChar - 65) + 10;
				else if (hexChar > 96 && hexChar < 103) // 소문자 a~f.
					hexNumber = (hexChar - 97) + 10;

				if (index % 2 != 0)
					rgb[index / 2] = hexNumber + (prevHexNumber * 16);
				else
					prevHexNumber = hexNumber;
	
				++index;
			}

			return rgb;
		}

		private static void Logic()
		{
			TableManager.Instance.LoadAll();

			Console.WriteLine("DagraacSystems Example!");
			Console.WriteLine($"{ExampleTable.Instance.Find(1).Desc}");

			var processExecutor = new ProcessExecutor();
			processExecutor.Start(new ExampleProcess());

			var result = Convert("#FF9900");

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