namespace DagraacSystems
{
	public interface ISession
	{
		bool IsConnected { get; }

		void Connect(string ip, int port);
		void Disconnect();
		void Send(byte[] data);
	}
}