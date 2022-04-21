using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.Network
{
	public class ChunkBuffer : DisposableObject
	{
		public const int ChunkMaxLength = 4096;

		public class Chunk
		{
			public int Length;
			public byte[] Data;

			public Chunk()
			{
				Length = 0;
				Data = new byte[ChunkMaxLength];
			}

			public void Clear()
			{
				Length = 0;
				Buffer.SetByte(Data, 0, 0);
			}
		}

		private Queue<Chunk> _availableChunks;
		private Queue<Chunk> _usingChunks;
		private Chunk _current;

		public ChunkBuffer()
		{
			_availableChunks = new Queue<Chunk>();
			_usingChunks = new Queue<Chunk>();
			_current = null;
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			_availableChunks.Clear();
			_availableChunks = null;

			_usingChunks.Clear();
			_usingChunks = null;

			base.OnDispose(explicitedDispose);
		}

		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		public void Enqueue(byte[] data)
		{
			if (_current == null)
			{
				_current = new Chunk();
			}
		}

		private void PushChunk(Chunk chunk)
		{
			if (_availableChunks.Contains(chunk))
				return;

			_availableChunks.Enqueue(chunk);
		}

		private Chunk PopChunk()
		{
			if (_availableChunks.Count > 0)
			{
				return _availableChunks.Dequeue();
			}

			return new Chunk();
		}

	}
}
