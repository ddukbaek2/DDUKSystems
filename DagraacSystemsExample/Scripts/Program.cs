using System;


namespace DagraacSystemsExample
{
	public class Program
	{
		public static void Main(string[] args)
		{
			TableManager.Instance.LoadAll();

			Console.WriteLine("DagraacSystems Example!");
			Console.WriteLine($"{ExampleTable.Instance.Find(1).Desc}");
		}
	}
}