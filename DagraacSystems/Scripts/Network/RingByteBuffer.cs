namespace DagraacSystems
{
	/// <summary>
	/// 계속 위치 값을 증가시켜가며 쓰는 바이트 버퍼.
	/// 읽어올때는 마지막 위치로부터 긁어온다.
	/// </summary>
	public class RingByteBuffer : DisposableObject
	{
		protected ByteBuffer _buffer;
		protected int _offset;

		public RingByteBuffer(int capacity = 4096)
		{
			_buffer = new ByteBuffer(capacity);
			_offset = 0;
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			_buffer.Dispose();
			_buffer = null;

			base.OnDispose(explicitedDispose);
		}

		public void Clear()
		{
			_buffer.Clear();
			_offset = 0;
		}

		/// <summary>
		/// 바이트 배열을 기록.
		/// </summary>
		public void Write(byte[] value)
		{
			foreach (var val in value)
				Write(val);
		}

		/// <summary>
		/// 바이트를 하나 기록.
		/// </summary>
		public void Write(byte value)
		{
			_buffer.Set(_offset, value);

			// 위치를 다음 요소로 셋팅.
			_offset = WrapIndex(_offset + 1);
		}

		/// <summary>
		/// 현재부터 size만큼 과거의 것을 얻어온다.
		/// </summary>
		public byte[] Read(int size)
		{
			var destIndex = WrapIndex(_offset - 1);
			var startIndex = WrapIndex(destIndex - size);

			var result = new byte[size];

			// 정상.
			if (startIndex < destIndex)
			{
				for (var i = startIndex; i <= destIndex; ++i)
					result[i - startIndex] = _buffer.Get(i);
			}
			else
			{
				var count = 0;
				for (var i = startIndex; i < _buffer.Length; ++i)
				{
					result[i - startIndex] = _buffer.Get(i);
					++count;
				}

				for (var i = 0; i < (size - count); ++i)
					result[count + i] = _buffer.Get(i);
			}

			return result;
		}

		/// <summary>
		/// 버퍼 전체 갯수를 넘어서면 다시 처음으로 돌아옴.
		/// 일단 한바퀴를 넘어설 정도라면 전체 캐퍼시티가 부족한 것이므로 한바퀴 내의 기준으로만 상정한다.
		/// </summary>
		private int WrapIndex(int index)
		{
			return WrapIndex(index, _buffer.Length);
		}

		/// <summary>
		/// 버퍼 전체 갯수를 넘어서면 다시 처음으로 돌아옴.
		/// 일단 한바퀴를 넘어설 정도라면 전체 캐퍼시티가 부족한 것이므로 한바퀴 내의 기준으로만 상정한다.
		/// </summary>
		private int WrapIndex(int index, int length)
		{
			var wrapIndex = index % length;
			if (wrapIndex < 0)
				wrapIndex += length;
			return wrapIndex;
		}

		//public byte this[int i]
		//{
		//	get
		//	{
		//		int m = Count < Capacity ? Count : Capacity;
		//		int p = (_position - 1 - i) % m;
		//		if (p < 0) p += m;
		//		return _buffer[p];
		//	}
		//	set
		//	{
		//		int m = Count < Capacity ? Count : Capacity;
		//		int p = (_position - 1 - i) % m;
		//		if (p < 0) p += m;
		//		_buffer[p] = value;
		//	}
		//}

		//public IEnumerator<byte> GetEnumerator()
		//{
		//	int end = Count;
		//	if (Count >= Capacity)
		//	{
		//		for (int i = _position; i < Count; i++)
		//		{
		//			yield return _buffer[i];
		//		}
		//		end = _position;
		//	}

		//	for (int i = 0; i < end; i++)
		//	{
		//		yield return _buffer[i];
		//	}
		//}

		//public IEnumerator<byte> GetReverseEnumerator()
		//{
		//	int end = 0;
		//	if (Count >= Capacity)
		//	{
		//		for (int i = _position - 1; i >= 0; i--)
		//		{
		//			yield return _buffer[i];
		//		}
		//		end = _position;
		//	}

		//	for (int i = Count - 1; i >= end; i--)
		//	{
		//		yield return _buffer[i];
		//	}
		//}

		//IEnumerator IEnumerable.GetEnumerator()
		//{
		//	return GetEnumerator();
		//}
	}
}