using System;
using System.Net;
using System.Net.Sockets;


namespace DagraacSystems.Network
{
	public interface ISession
	{
		bool IsConnected { get; }

		void Connect(string ip, ushort port);
		void Disconnect();
	}


	/// <summary>
	/// 세션.
	/// </summary>
	public class TCPSession : DisposableObject
	{
		public enum TCPConnectionState
		{
			Disconnected,
			Disconnecting,
			Connecting,
			Connected,
		}


		private Socket _socket;
		private string _ip;
		private int _port;
		private TCPConnectionState _connectionState;

		private SocketAsyncEventArgs _connectEvent;
		private SocketAsyncEventArgs _disconnectEvent;
		private SocketAsyncEventArgs _sendEvent;
		private SocketAsyncEventArgs _receiveEvent;

		public bool IsConnected => _socket.Connected;

		public TCPSession() : base()
		{
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_ip = string.Empty;
			_port = 0;

			_connectEvent = new SocketAsyncEventArgs();
			_connectEvent.Completed += (sender, e) =>
			{
				if (e.SocketError == SocketError.Success)
				{
					SetConnectionState(TCPConnectionState.Connected);
					Receive();
				}
				else
				{
					SetConnectionState(TCPConnectionState.Disconnected);
				}
			};

			_disconnectEvent = new SocketAsyncEventArgs();
			_disconnectEvent.Completed += (sender, e) =>
			{
				SetConnectionState(TCPConnectionState.Disconnected);
			};

			_sendEvent = new SocketAsyncEventArgs();
			_sendEvent.Completed += (sender, e) =>
			{
				if (e.SocketError == SocketError.Success && e.LastOperation == SocketAsyncOperation.Send)
				{
					OnSended(e.Buffer);
				}
				else
				{
					SetConnectionState(TCPConnectionState.Disconnected);
				}
			};

			_receiveEvent = new SocketAsyncEventArgs();
			_receiveEvent.Completed += (sender, e) =>
			{
				if (e.SocketError == SocketError.Success && e.LastOperation == SocketAsyncOperation.Receive)
				{
					if (e.BytesTransferred > 0)
					{
						OnReceived(e.Buffer);
						Receive();
					}
					else
					{
						SetConnectionState(TCPConnectionState.Disconnected);
					}
				}
				else
				{
					SetConnectionState(TCPConnectionState.Disconnected);
				}
			};
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			if (_socket != null)
			{
				_socket.Disconnect(false);
				_socket.Dispose();
				_socket = null;
			}

			_connectEvent.Dispose();
			_disconnectEvent.Dispose();
			_sendEvent.Dispose();
			_receiveEvent.Dispose();

			if (_connectionState != TCPConnectionState.Disconnected)
			{
				SetConnectionState(TCPConnectionState.Disconnecting);
				SetConnectionState(TCPConnectionState.Disconnected);
			}

			base.OnDispose(explicitedDispose);
		}

		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		protected virtual void OnConnected(bool isSuccssed)
		{
		}

		protected virtual void OnDisconnected(bool explicitDisconnected)
		{
		}

		protected virtual void OnSended(byte[] data)
		{
		}

		protected virtual void OnReceived(byte[] data)
		{
		}

		public void Connect(string ip, int port)
		{
			if (IsDisposed)
				return;

			_ip = ip;
			_port = port;
			_connectEvent.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);

			Reconnect();
		}

		public void Reconnect()
		{
			if (IsDisposed)
				return;

			if (IsConnected)
				return;

			if (_connectionState == TCPConnectionState.Connecting || _connectionState == TCPConnectionState.Connected)
				return;

			SetConnectionState(TCPConnectionState.Connecting);		
			_socket.ConnectAsync(_connectEvent);
		}

		public void Disconnect(bool forced)
		{
			if (IsDisposed)
				return;

			if (!IsConnected)
				return;

			if (_connectionState == TCPConnectionState.Disconnecting || _connectionState == TCPConnectionState.Disconnected)
				return;

			SetConnectionState(TCPConnectionState.Disconnecting);
			SetConnectionState(TCPConnectionState.Disconnected);
			_socket.DisconnectAsync(_disconnectEvent);
		}

		public void Send(byte[] data)
		{
			if (IsDisposed)
				return;

			if (!IsConnected)
				return;

			if (_connectionState == TCPConnectionState.Disconnecting || _connectionState == TCPConnectionState.Disconnected)
				return;

			_socket.SendAsync(_sendEvent);
		}

		/// <summary>
		/// 기존의 Discconnection 감지는 send, receive 이벤트 없이는 동작하지 않으므로 주기적 체크를 기반으로 한 별도 감지체계 추가.
		/// </summary>
		public void CheckDisconnection()
		{
			if (IsDisposed)
				return;

			if (_connectionState != TCPConnectionState.Connected)
				return;

			if (IsConnected)
				return;

			SetConnectionState(TCPConnectionState.Disconnected);
		}

		/// <summary>
		/// 수신.
		/// </summary>
		private void Receive()
		{
			if (IsDisposed)
				return;

			if (!IsConnected)
				return;

			if (_connectionState == TCPConnectionState.Disconnecting || _connectionState == TCPConnectionState.Disconnected)
				return;

			_socket.ReceiveAsync(_receiveEvent);
		}

		/// <summary>
		/// 접속상태 변경.
		/// </summary>
		private void SetConnectionState(TCPConnectionState connectionState)
		{
			if (_connectionState != connectionState)
			{
				TCPConnectionState prevState = _connectionState;
				TCPConnectionState curruntState = connectionState;

				_connectionState = connectionState;

				// 접속시도에 대해 성공.
				if (prevState == TCPConnectionState.Connecting && curruntState == TCPConnectionState.Connected)
				{
					OnConnected(true);
				}

				// 접속시도에 대해 실패.
				if (prevState == TCPConnectionState.Connecting && curruntState == TCPConnectionState.Disconnected)
				{
					OnConnected(false);
				}

				// 접속 중에 강제 해제됨.
				if (prevState == TCPConnectionState.Connected && curruntState == TCPConnectionState.Disconnected)
				{
					OnDisconnected(false);
				}

				// 접속해제시도에 대한 성공.
				if (prevState == TCPConnectionState.Disconnecting && curruntState == TCPConnectionState.Disconnected)
				{
					OnDisconnected(true);
				}
			}
		}
	}
}
