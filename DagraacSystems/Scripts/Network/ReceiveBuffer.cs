using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.Network
{
	public class ReceiveBuffer : DisposableObject
	{
		private List<byte> _buffer;
		private Queue<byte[]> _queue;
		private ushort _completeSize;
		private byte[] _completeSizeBytes;

		public int Count => _queue.Count;

		public ReceiveBuffer()
		{
			_buffer = new List<byte>(sizeof(ushort));
			_queue = new Queue<byte[]>();
			_completeSize = 0;
			_completeSizeBytes = new byte[sizeof(ushort)];
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			_buffer.Clear();
			_queue.Clear();
			_completeSize = 0;
			_completeSizeBytes = null;

			base.OnDispose(explicitedDispose);
		}

		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		public void Enqueue(byte[] chunk, int chunkSize)
		{
			//if (chunk == null || chunk.Length == 0 || chunkSize == 0 || chunk.Length < chunkSize)
			//	return;

			// 데이터 쌓기.
			for (var i = 0; i < chunkSize; ++i)
				_buffer.Add(chunk[i]);

			// 시작.
			if (_completeSize == 0)
			{
				// 데이터의 사이즈를 모를때 2바이트 이상 적재가 되었다면 2바이트를 데이터의 사이즈로 셋팅한 후 제거.
				if (_completeSizeBytes.Length <= _buffer.Count)
				{
					for (var i = 0; i < _completeSizeBytes.Length; ++i)
						_completeSizeBytes[i] = _buffer[i];

					_buffer.RemoveRange(0, _completeSizeBytes.Length); // overhead.
					_completeSize = BitConverter.ToUInt16(_completeSizeBytes, 0);
				}
			}

			// 완료.
			if (_completeSize > 0)
			{
				// 데이터의 사이즈 이상 적재가 되었다면 데이터를 큐에 쌓고 제거.
				if (_completeSize <= _buffer.Count)
				{
					var completeData = new byte[_completeSize];
					for (var i = 0; i < _completeSize; ++i)
						completeData[i] = _buffer[i];

					_buffer.RemoveRange(0, (int)_completeSize); // overhead.
					_queue.Enqueue(completeData);

					_completeSize = 0;
					if (_buffer.Count > 0)
					{
						Enqueue(null, 0);
						return;
					}
				}
			}
		}

		public byte[] Dequeue()
		{
			return _queue.Dequeue();
		}

		public void Clear()
		{
			_queue.Clear();
			_buffer.Clear();
			_completeSize = 0;
		}
	}
}