namespace DagraacSystems
{
	/// <summary>
	/// 인스턴스.
	/// </summary>
	public interface IRepositorable
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		void OnInstantiated(bool explicitCall, params object[] args);
	}
}