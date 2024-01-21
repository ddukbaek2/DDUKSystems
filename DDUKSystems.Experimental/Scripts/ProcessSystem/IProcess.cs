namespace DDUKSystems
{
	/// <summary>
	/// 상태 처리기.
	/// </summary>
	public interface IProcess
	{
		void Execute(ProcessSystem processExecutor, ulong processID, params object[] args);
		void Tick(float tick);
		void Reset();
		void Pause();
		void Resume();
		void Finish();
	}
}