using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.Network
{
	/// <summary>
	/// 수신 버퍼.
	/// 모든 수신 데이터의 구조는 다음과 같다.
	/// HEADER(4)
	///		BODYSIZE(2)
	///		BODYTYPE(1)
	///		RESULT(1)
	///	BODY(N)
	///		...
	/// </summary>
	public class ReceiveBuffer : DisposableObject
	{
		/// <summary>
		/// 수신 상태.
		/// </summary>
		public enum ReceiveState
		{
			Wait, // 대기중.
			Receiving, // 수신중.
		}


		/// <summary>
		/// 수신 데이터.
		/// </summary>
		public struct ReceiveData
		{	
			public ushort BodySize;
			public byte BodyType;
			public byte BodyResult;
			public byte[] Body;
		}


		/// <summary>
		/// 헤더의 크기 (4byte).
		/// </summary>
		public const int HeaderSize = sizeof(ushort) + sizeof(byte) + sizeof(byte);

		/// <summary>
		/// 바디의 최대 크기 (65536byte).
		/// </summary>
		public const int BodyMaxSize = sizeof(ushort);

		private Queue<byte> _buffer;
		private Queue<ReceiveData> _queue;
		private ReceiveState _state;
		private byte[] _header;
		private ReceiveData _currentReceiveData;

		public int Count => _queue.Count;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public ReceiveBuffer()
		{
			_buffer = new Queue<byte>(ReceiveBuffer.BodyMaxSize);
			_queue = new Queue<ReceiveData>();

			_state = ReceiveState.Wait;
			_header = new byte[ReceiveBuffer.HeaderSize];
			_currentReceiveData = new ReceiveData();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			_buffer.Clear();
			_queue.Clear();

			_header = null;
			_currentReceiveData.Body = null;

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
		/// 바이트를 수신한다.
		/// 바이트를 쪼개져서 올수도 있고 더 붙어서 올 수도 있다는 것을 전제한다.
		/// 수신된 바이트를 조립한 수신데이터를 완성하면 큐에 담는다.
		/// </summary>
		public void Enqueue(byte[] bytes, int bytesTransferred)
		{
			//if (chunk == null || chunk.Length == 0 || chunkSize == 0 || chunk.Length < chunkSize)
			//	return;

			// 데이터 쌓기.
			// 하나의 패킷이 쪼개져서 오는 경우를 대비하여 무지성 쌓기를 반복함.
			for (var i = 0; i < bytesTransferred; ++i)
				_buffer.Enqueue(bytes[i]);


			switch (_state)
			{
				// 대기중.
				case ReceiveState.Wait:
					{
						// 아직 데이터의 사이즈를 모를때 헤더크기 이상 적재가 되었다면 헤더크기를 데이터의 사이즈로 셋팅한 후 제거.
						if (_buffer.Count >= ReceiveBuffer.HeaderSize)
						{
							for (var i = 0; i < ReceiveBuffer.HeaderSize; ++i)
								_header[i] = _buffer.Dequeue();

							// 수신데이터는 valuetype이므로 내부의 바디 배열의 경우만 매번 새로 할당한다.
							_currentReceiveData.BodySize = BitConverter.ToUInt16(_header, 0);
							_currentReceiveData.BodyType = _header[2];
							_currentReceiveData.BodyResult = _header[3];
							_currentReceiveData.Body = new byte[_currentReceiveData.BodySize]; // allocate overhead.
							_state = ReceiveState.Receiving;
						}
						break;
					}

				// 수신중.
				case ReceiveState.Receiving:
					{
						// 데이터의 사이즈 이상 적재가 되었다면 데이터를 큐에 쌓고 제거.
						if (_buffer.Count >= _currentReceiveData.BodySize)
						{
							for (var i = 0; i < _currentReceiveData.BodySize; ++i)
								_currentReceiveData.Body[i] = _buffer.Dequeue();
							_queue.Enqueue(_currentReceiveData);
							_state = ReceiveState.Wait;

							// 작업이 완료된 직 후 아직도 버퍼에 헤더크기 이상 남아있는 것이 있다면... 곧바로 다음 수신결과 처리...
							if (_buffer.Count >= ReceiveBuffer.HeaderSize)
							{
								Enqueue(null, 0);
								return;
							}
						}
						break;
					}
			}
		}

		/// <summary>
		/// 수신데이터 빼오기.
		/// </summary>
		public ReceiveData Dequeue()
		{
			return _queue.Dequeue();
		}

		/// <summary>
		/// 버퍼 비우기.
		/// </summary>
		public void Clear()
		{
			_queue.Clear();
			_buffer.Clear();
			_state = ReceiveState.Wait;
		}
	}
}