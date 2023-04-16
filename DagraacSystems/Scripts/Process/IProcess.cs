namespace DagraacSystems
{
	/// <summary>
	/// 상태 처리기.
	/// </summary>
	public interface IProcess
	{
		void Execute(ProcessExecutor processExecutor, ulong processID, params object[] args);
		void Update(float tick);
		void Reset();
		void Pause();
		void Resume();
		void Finish();
	}
}