using System;
using DagraacSystems.Table;


namespace DagraacSystemsExample
{
	public class Program
	{
		public static void Main(string[] args)
		{
			TableManager.Instance.LoadAll();

			Console.WriteLine("DagraacSystems Example!");
		}
	}
}