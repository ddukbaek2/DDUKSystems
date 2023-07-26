using System;
using System.Net;
using System.Net.Sockets;


namespace DagraacSystems
{
    /// <summary>
    /// TCP 세션.
    /// </summary>
    public class TCPSession : DisposableObject, ISession
	{
		public enum TCPConnectionState
		{
			Disconnected,
			Disconnecting,
			Connecting,
			Connected,
		}


		private Socket m_Socket;
		private SocketAsyncEventArgs m_ConnectEvent;
		private SocketAsyncEventArgs m_DisconnectEvent;
		private SocketAsyncEventArgs m_SendEvent;
		private SocketAsyncEventArgs m_ReceiveEvent;
		private TCPConnectionState _connectionState;
		private ReceiveBuffer m_ReceiveBuffer;
		public bool IsConnected => m_Socket.Connected;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public TCPSession() : base()
		{
			m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			m_ConnectEvent = new SocketAsyncEventArgs();
			m_ConnectEvent.Completed += (sender, e) =>
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

			m_DisconnectEvent = new SocketAsyncEventArgs();
			m_DisconnectEvent.Completed += (sender, e) =>
			{
				SetConnectionState(TCPConnectionState.Disconnected);
			};

			m_SendEvent = new SocketAsyncEventArgs();
			m_SendEvent.SetBuffer(new byte[4096], 0, 4096);
			m_SendEvent.Completed += (sender, e) =>
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

			m_ReceiveEvent = new SocketAsyncEventArgs();
			m_ReceiveEvent.SetBuffer(new byte[4096], 0, 4096);
			m_ReceiveEvent.Completed += (sender, e) =>
			{
				if (e.SocketError == SocketError.Success && e.LastOperation == SocketAsyncOperation.Receive)
				{
					if (e.BytesTransferred > 0)
					{
						// 무지성 수신.
						m_ReceiveBuffer.Enqueue(e.Buffer, e.BytesTransferred);

						// 완성된 수신데이터가 있다면.
						if (m_ReceiveBuffer.Count > 0)
						{
							// 수신완료 처리.
							OnReceived(m_ReceiveBuffer.Dequeue());
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

			m_ReceiveBuffer = ManagedObject.Create<ReceiveBuffer>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			if (m_Socket != null)
			{
				m_Socket.Disconnect(false);
				m_Socket.Dispose();
				m_Socket = null;
			}

			if (m_ConnectEvent != null)
			{
				m_ConnectEvent.Dispose();
				m_ConnectEvent = null;
			}

			if (m_DisconnectEvent != null)
			{
				m_DisconnectEvent.Dispose();
				m_DisconnectEvent = null;
			}

			m_SendEvent.Dispose();
			m_ReceiveEvent.Dispose();

			if (m_ReceiveBuffer != null)
			{
				m_ReceiveBuffer.Dispose();
				m_ReceiveBuffer = null;
			}

			if (_connectionState != TCPConnectionState.Disconnected)
			{
				SetConnectionState(TCPConnectionState.Disconnecting);
				SetConnectionState(TCPConnectionState.Disconnected);
			}

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		/// <summary>
		/// 연결됨.
		/// </summary>
		protected virtual void OnConnected(bool isSuccssed)
		{
			m_ReceiveBuffer.Clear();
		}

		/// <summary>
		/// 연결 해제됨.
		/// </summary>
		protected virtual void OnDisconnected(bool explicitDisconnected)
		{
			m_ReceiveBuffer.Clear();
		}

		/// <summary>
		/// 송신됨.
		/// </summary>
		protected virtual void OnSended(byte[] data)
		{
		}

		/// <summary>
		/// 수신됨.
		/// </summary>
		protected virtual void OnReceived(ReceiveBuffer.ReceiveData receiveData)
		{
		}

		/// <summary>
		/// 연결.
		/// </summary>
		public void Connect(string ip, int port)
		{
			if (IsDisposed)
				return;

			m_ConnectEvent.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

			Reconnect();
		}

		/// <summary>
		/// 재연결.
		/// </summary>
		public void Reconnect()
		{
			if (IsDisposed)
				return;

			if (IsConnected)
				return;

			if (_connectionState == TCPConnectionState.Connecting || _connectionState == TCPConnectionState.Connected)
				return;

			SetConnectionState(TCPConnectionState.Connecting);		
			m_Socket.ConnectAsync(m_ConnectEvent);
		}

		/// <summary>
		/// 연결 해제.
		/// </summary>
		public void Disconnect()
		{
			if (IsDisposed)
				return;

			if (!IsConnected)
				return;

			if (_connectionState == TCPConnectionState.Disconnecting || _connectionState == TCPConnectionState.Disconnected)
				return;

			SetConnectionState(TCPConnectionState.Disconnecting);
			SetConnectionState(TCPConnectionState.Disconnected);
			m_Socket.DisconnectAsync(m_DisconnectEvent);
		}

		/// <summary>
		/// 송신.
		/// </summary>
		public void Send(byte[] data)
		{
			if (IsDisposed)
				return;

			if (!IsConnected)
				return;

			if (_connectionState == TCPConnectionState.Disconnecting || _connectionState == TCPConnectionState.Disconnected)
				return;

			m_SendEvent.SetBuffer(data, 0, data.Length);
			m_Socket.SendAsync(m_SendEvent);
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

			m_Socket.ReceiveAsync(m_ReceiveEvent);
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
