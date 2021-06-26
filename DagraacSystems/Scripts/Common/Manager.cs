namespace DagraacSystems
{
	public class Manager<T> : Singleton<T> where T : Manager<T>, new()
	{
		public Manager()
		{

		}
	}
}