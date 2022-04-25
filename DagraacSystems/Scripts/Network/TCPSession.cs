using System;
using System.Net;
using System.Net.Sockets;


namespace DagraacSystems.Network
{
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
		private SocketAsyncEventArgs _connectEvent;
		private SocketAsyncEventArgs _disconnectEvent;
		private SocketAsyncEventArgs _sendEvent;
		private SocketAsyncEventArgs _receiveEvent;
		private TCPConnectionState _connectionState;
		private ReceiveBuffer _receiveBuffer;
		public bool IsConnected => _socket.Connected;

		public TCPSession() : base()
		{
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
			_sendEvent.SetBuffer(new byte[4096], 0, 4096);
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
			_receiveEvent.SetBuffer(new byte[4096], 0, 4096);
			_receiveEvent.Completed += (sender, e) =>
			{
				if (e.SocketError == SocketError.Success && e.LastOperation == SocketAsyncOperation.Receive)
				{
					if (e.BytesTransferred > 0)
					{
						// 무지성 수신.
						_receiveBuffer.Enqueue(e.Buffer, e.BytesTransferred);

						// 완성된 수신데이터가 있다면.
						if (_receiveBuffer.Count > 0)
						{
							// 수신완료 처리.
							OnReceived(_receiveBuffer.Dequeue());
						}

						// 다음 수신을 다시 요청.
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

			_receiveBuffer = DisposableObject.Create<ReceiveBuffer>();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			if (_socket != null)
			{
				_socket.Disconnect(false);
				_socket.Dispose();
				_socket = null;
			}

			if (_connectEvent != null)
			{
				_connectEvent.Dispose();
				_connectEvent = null;
			}

			if (_disconnectEvent != null)
			{
				_disconnectEvent.Dispose();
				_disconnectEvent = null;
			}

			_sendEvent.Dispose();
			_receiveEvent.Dispose();

			if (_receiveBuffer != null)
			{
				_receiveBuffer.Dispose();
				_receiveBuffer = null;
			}

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
			_receiveBuffer.Clear();
		}

		protected virtual void OnDisconnected(bool explicitDisconnected)
		{
			_receiveBuffer.Clear();
		}

		protected virtual void OnSended(byte[] data)
		{
		}

		protected virtual void OnReceived(ReceiveBuffer.ReceiveData receiveData)
		{
		}

		public void Connect(string ip, int port)
		{
			if (IsDisposed)
				return;

			_connectEvent.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

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

			_sendEvent.SetBuffer(data, 0, data.Length);
			_socket.SendAsync(_sendEvent);
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
