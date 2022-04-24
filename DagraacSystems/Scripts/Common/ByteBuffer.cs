using System;


namespace DagraacSystems
{
	/// <summary>
	/// 바이트 버퍼.
	/// </summary>
	public class ByteBuffer : DisposableObject
	{
		private byte[] _buffer;

		public int Length => _buffer.Length;

		public byte this[int index]
		{
			get => Get(index);
			set => Set(index, value);
		}

		public ByteBuffer(int capacity = 4096)
		{
			_buffer = new byte[capacity];
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			_buffer = null;

			base.OnDispose(explicitedDispose);
		}

		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		public void Clear()
		{
			for (var i = 0; i < _buffer.Length; ++i)
				_buffer[i] = 0x00;
		}

		public void Set(int index, byte value)
		{
			_buffer[index] = value;
		}

		public byte Get(int index)
		{
			return _buffer[index];
		}

		public byte[] Copy(int offset, int size)
		{
			var copy = new byte[size];
			Buffer.BlockCopy(_buffer, offset, copy, 0, size);
			return copy;
		}
	}
}