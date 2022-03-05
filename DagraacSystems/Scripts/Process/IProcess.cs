namespace DagraacSystems
{
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