using uScript;


namespace uScript
{
	/// <summary>
	/// 런타임 해석기.
	/// </summary>
	public class UInterpreter : DDUKSystems.DisposableObject
	{
		public static void Load(UPlugin plugin)
		{
		}

		public static void Load(UPackage package)
		{
		}

		public static void Unload(UPackage package)
		{
		}

		public static void Unload(UPlugin plugin)
		{
		}

		public static UVariable Execute<T>(string script)
		{
			return default;
		}
	}
}