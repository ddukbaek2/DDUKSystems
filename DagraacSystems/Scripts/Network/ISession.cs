namespace DagraacSystems.Network
{
	public interface ISession
	{
		bool IsConnected { get; }

		void Connect(string ip, ushort port);
		void Disconnect();
		void Send(byte[] data);
	}
}