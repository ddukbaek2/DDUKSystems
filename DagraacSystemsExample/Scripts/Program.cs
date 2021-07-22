using System;
using DagraacSystems.Notification;


namespace DagraacSystemsExample
{
	public class Callback
	{
		public delegate void OnCreate();
	}

	public class ExampleObject
	{
		public ExampleObject()
		{
			Notification.Instance.Register<Callback.OnCreate>(OnCreate);
		}

		private void OnCreate()
		{
			Console.WriteLine("OnCreate()");
		}
	}

	public class Program
	{


		public static void Main(string[] args)
		{
			TableManager.Instance.LoadAll();

			var exampleObject = new ExampleObject();

			Console.WriteLine("DagraacSystems Example!");
			Console.WriteLine($"{ExampleTable.Instance.Find(1).Desc}");

			GC.SuppressFinalize(exampleObject);
			exampleObject = null;


			Notification.Instance.Notify<Callback.OnCreate>();

			Console.ReadKey();
		}
	}
}