using System;
using System.Collections.Generic;
using System.Text;


namespace DDUKSystems
{
	/// <summary>
	/// 변수 버퍼.
	/// Write : 객체를 바이트로 변환.
	/// Read : 바이트를 객체로 변환.
	/// </summary>
	public class VariableBuffer : DisposableObject
	{
		public static StringBuilder s_StringBuilder = new StringBuilder();

		private List<byte> m_Bytes;
		private byte[] m_CachedByteArray;
		private int m_ReadOffset;

		/// <summary>
		/// 바이트 배열.
		/// </summary>
		public byte[] ByteArray
		{
			get
			{
				if (m_CachedByteArray == null || m_CachedByteArray.Length != m_Bytes.Count)
					m_CachedByteArray = m_Bytes.ToArray();

				return m_CachedByteArray;
			}
		}

		/// <summary>
		/// 읽는 시작 위치.
		/// </summary>
		public int ReadOffset
		{
			set
			{
				m_ReadOffset = Math.Min(Math.Max(value, 0), m_Bytes.Count - 1);
			}
			get
			{
				return m_ReadOffset;
			}
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public VariableBuffer(int capacity = 4096) : base()
		{
			m_Bytes = new List<byte>(capacity);
			Clear();
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public VariableBuffer(byte[] bytes) : base()
		{
			m_Bytes = new List<byte>();

			Clear();
			m_Bytes.AddRange(bytes);
			m_CachedByteArray = bytes;
			m_ReadOffset = 0;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
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
		/// 초기화.
		/// </summary>
		public void Clear()
		{
			m_Bytes.Clear();
			m_CachedByteArray = null;
			m_ReadOffset = 0;
		}

		public bool Read(out bool value)
		{
			value = BitConverter.ToBoolean(ByteArray, ReadOffset);
			ReadOffset += sizeof(bool);
			return true;
		}

		public bool Read(out byte value)
		{
			value = m_Bytes[ReadOffset];
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
			value = (sbyte)m_Bytes[ReadOffset];
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
			s_StringBuilder.Clear();
			value = string.Empty;

			if (!Read(out int count))
				return false;

			for (int i = 0; i < count; ++i)
			{
				if (!Read(out char ch))
					return false;

				s_StringBuilder.Append(ch);
			}

			value = s_StringBuilder.ToString();
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
			m_Bytes.AddRange(bytes);
		}

		public void Write(byte value)
		{
			m_Bytes.Add(value);
		}

		public void Write(ushort value)
		{
			var bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
		}

		public void Write(uint value)
		{
			var bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
		}

		public void Write(ulong value)
		{
			var bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
		}

		public void Write(sbyte value)
		{
			m_Bytes.Add((byte)value);
		}

		public void Write(char value)
		{
			var bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
		}

		public void Write(short value)
		{
			var bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
		}

		public void Write(int value)
		{
			var bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
		}

		public void Write(long value)
		{
			var bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
		}

		public void Write(float value)
		{
			var bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
		}

		public void Write(double value)
		{
			var bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
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
