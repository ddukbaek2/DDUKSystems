namespace DagraacSystems
{
	/// <summary>
	/// 매니저.
	/// </summary>
	public class Manager<T> : Singleton<T> where T : Manager<T>, new()
	{
		public Manager()
		{

		}
	}
}