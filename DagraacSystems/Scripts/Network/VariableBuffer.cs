using System;
using System.Collections.Generic;
using System.Text;


namespace DagraacSystems
{
	/// <summary>
	/// 변수 버퍼.
	/// 호출 순서대로 바이트 쌓기.
	/// </summary>
	public class VariableBuffer
	{
		public static StringBuilder _builder = new StringBuilder();

		private List<byte> _bytes;
		private byte[] _byteArray;
		private int _readOffset;

		/// <summary>
		/// 바이트 배열.
		/// </summary>
		public byte[] ByteArray
		{
			get
			{
				if (_byteArray == null || _byteArray.Length != _bytes.Count)
					_byteArray = _bytes.ToArray();

				return _byteArray;
			}
		}

		/// <summary>
		/// 읽는 시작 위치.
		/// </summary>
		public int ReadOffset
		{
			set
			{
				_readOffset = Math.Min(Math.Max(value, 0), _bytes.Count - 1);
			}
			get
			{
				return _readOffset;
			}
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public VariableBuffer()
		{
			_bytes = new List<byte>();
			Clear();
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public VariableBuffer(byte[] bytes) : this()
		{
			_bytes.AddRange(bytes);
			_byteArray = bytes;
			_readOffset = 0;
		}

		public void Clear()
		{
			_bytes.Clear();
			_byteArray = null;
			_readOffset = 0;
		}

		public bool Read(out bool value)
		{
			value = BitConverter.ToBoolean(ByteArray, ReadOffset);
			ReadOffset += sizeof(bool);
			return true;
		}

		public bool Read(out byte value)
		{
			value = _bytes[ReadOffset];
			ReadOffset += sizeof(byte);
			return true;
		}

		public bool Read(out ushort value)
		{
			value = BitConverter.ToUInt16(ByteArray, ReadOffset);
			ReadOffset += sizeof(ushort);
			return true;
		}

		public bool Read(out uint value)
		{
			value = BitConverter.ToUInt32(ByteArray, ReadOffset);
			ReadOffset += sizeof(uint);
			return true;
		}

		public bool Read(out ulong value)
		{
			value = BitConverter.ToUInt64(ByteArray, ReadOffset);
			ReadOffset += sizeof(ulong);
			return true;
		}

		public bool Read(out sbyte value)
		{
			value = (sbyte)_bytes[ReadOffset];
			ReadOffset += sizeof(sbyte);
			return true;
		}

		public bool Read(out char value)
		{
			value = BitConverter.ToChar(ByteArray, ReadOffset);
			ReadOffset += sizeof(char);
			return true;
		}

		public bool Read(out short value)
		{
			value = BitConverter.ToInt16(ByteArray, ReadOffset);
			ReadOffset += sizeof(short);
			return true;
		}

		public bool Read(out int value)
		{
			value = BitConverter.ToInt32(ByteArray, ReadOffset);
			ReadOffset += sizeof(int);
			return true;
		}

		public bool Read(out long value)
		{
			value = BitConverter.ToInt64(ByteArray, ReadOffset);
			ReadOffset += sizeof(long);
			return true;
		}

		public bool Read(out float value)
		{
			value = BitConverter.ToSingle(ByteArray, ReadOffset);
			ReadOffset += sizeof(float);
			return true;
		}

		public bool Read(out double value)
		{
			value = BitConverter.ToDouble(ByteArray, ReadOffset);
			ReadOffset += sizeof(double);
			return true;
		}

		public bool Read(out string value)
		{
			_builder.Clear();
			value = string.Empty;

			if (!Read(out int count))
				return false;

			for (int i = 0; i < count; ++i)
			{
				if (!Read(out char ch))
					return false;

				_builder.Append(ch);
			}

			value = _builder.ToString();
			return true;
		}

		public bool Read(out DateTime value)
		{
			value = DateTime.MinValue;

			if (!Read(out long ticks))
				return false;

			value = new DateTime(ticks);
			return true;
		}

		public void Write(bool value)
		{
			var bytes = BitConverter.GetBytes(value);
			_bytes.AddRange(bytes);
		}

		public void Write(byte value)
		{
			_bytes.Add(value);
		}

		public void Write(ushort value)
		{
			var bytes = BitConverter.GetBytes(value);
			_bytes.AddRange(bytes);
		}

		public void Write(uint value)
		{
			var bytes = BitConverter.GetBytes(value);
			_bytes.AddRange(bytes);
		}

		public void Write(ulong value)
		{
			var bytes = BitConverter.GetBytes(value);
			_bytes.AddRange(bytes);
		}

		public void Write(sbyte value)
		{
			_bytes.Add((byte)value);
		}

		public void Write(char value)
		{
			var bytes = BitConverter.GetBytes(value);
			_bytes.AddRange(bytes);
		}

		public void Write(short value)
		{
			var bytes = BitConverter.GetBytes(value);
			_bytes.AddRange(bytes);
		}

		public void Write(int value)
		{
			var bytes = BitConverter.GetBytes(value);
			_bytes.AddRange(bytes);
		}

		public void Write(long value)
		{
			var bytes = BitConverter.GetBytes(value);
			_bytes.AddRange(bytes);
		}

		public void Write(float value)
		{
			var bytes = BitConverter.GetBytes(value);
			_bytes.AddRange(bytes);
		}

		public void Write(double value)
		{
			var bytes = BitConverter.GetBytes(value);
			_bytes.AddRange(bytes);
		}

		public void Write(string value)
		{
			Write(value.Length);
			for (int i = 0; i < value.Length; ++i)
				Write(value[i]);
		}

		public void Write(DateTime value)
		{
			Write(value.Ticks);
		}

		//public void Write(decimal[] value)
		//{
		//	Write(value.Length);
		//	for (int i = 0; i < value.Length; ++i)
		//		Write(value[i]);
		//}

		//public void Write(int[] value)
		//{
		//	Write(value.Length);
		//	for (int i = 0; i < value.Length; ++i)
		//		Write(value[i]);
		//}

		//public void Write(string[] value)
		//{
		//	Write(value.Length);
		//	for (int i = 0; i < value.Length; ++i)
		//		Write(value[i]);
		//}
	}
}
