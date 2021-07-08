using System;
using System.Collections.Generic;
using System.Text;


namespace DagraacSystems.Network
{
	/// <summary>
	/// 순서대로 바이트 쌓기.
	/// </summary>
	public class Message
	{
		public static StringBuilder s_Builder = new StringBuilder();

		private List<byte> m_Bytes;
		private byte[] m_ByteArray;
		private int m_ReadOffset;
		private int m_WriteOffset;

		/// <summary>
		/// 바이트 배열.
		/// </summary>
		public byte[] ByteArray
		{
			get
			{
				if (m_ByteArray == null || m_ByteArray.Length != m_Bytes.Count)
				{
					m_ByteArray = m_Bytes.ToArray();
					m_WriteOffset = m_Bytes.Count;
				}

				return m_ByteArray;
			}
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public Message()
		{
			m_Bytes = new List<byte>();
			Clear();
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public Message(byte[] bytes) : this()
		{
			m_Bytes.AddRange(bytes);
			m_ByteArray = bytes;
			m_ReadOffset = 0;
			m_WriteOffset = bytes.Length;
		}

		public void Clear()
		{
			m_Bytes.Clear();
			m_ByteArray = null;
			m_ReadOffset = 0;
			m_WriteOffset = 0;
		}

		public bool Read(out bool value)
		{
			value = BitConverter.ToBoolean(ByteArray, m_ReadOffset);
			m_ReadOffset += sizeof(bool);
			return true;
		}

		public bool Read(out byte value)
		{
			value = m_Bytes[m_ReadOffset];
			m_ReadOffset += sizeof(byte);
			return true;
		}

		public bool Read(out ushort value)
		{
			value = BitConverter.ToUInt16(ByteArray, m_ReadOffset);
			m_ReadOffset += sizeof(ushort);
			return true;
		}

		public bool Read(out uint value)
		{
			value = BitConverter.ToUInt32(ByteArray, m_ReadOffset);
			m_ReadOffset += sizeof(uint);
			return true;
		}

		public bool Read(out ulong value)
		{
			value = BitConverter.ToUInt64(ByteArray, m_ReadOffset);
			m_ReadOffset += sizeof(ulong);
			return true;
		}

		public bool Read(out sbyte value)
		{
			value = (sbyte)m_Bytes[m_ReadOffset];
			m_ReadOffset += sizeof(sbyte);
			return true;
		}

		public bool Read(out char value)
		{
			value = BitConverter.ToChar(ByteArray, m_ReadOffset);
			m_ReadOffset += sizeof(char);
			return true;
		}

		public bool Read(out short value)
		{
			value = BitConverter.ToInt16(ByteArray, m_ReadOffset);
			m_ReadOffset += sizeof(short);
			return true;
		}

		public bool Read(out int value)
		{
			value = BitConverter.ToInt32(ByteArray, m_ReadOffset);
			m_ReadOffset += sizeof(int);
			return true;
		}

		public bool Read(out long value)
		{
			value = BitConverter.ToInt64(ByteArray, m_ReadOffset);
			m_ReadOffset += sizeof(long);
			return true;
		}

		public bool Read(out float value)
		{
			value = BitConverter.ToSingle(ByteArray, m_ReadOffset);
			m_ReadOffset += sizeof(float);
			return true;
		}

		public bool Read(out double value)
		{
			value = BitConverter.ToDouble(ByteArray, m_ReadOffset);
			m_ReadOffset += sizeof(double);
			return true;
		}

		public bool Read(out string value)
		{
			s_Builder.Clear();
			value = string.Empty;

			if (!Read(out int count))
				return false;

			for (int i = 0; i < count; ++i)
			{
				if (!Read(out char ch))
					return false;

				s_Builder.Append(ch);
			}

			value = s_Builder.ToString();
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
			byte[] bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(bool);
		}

		public void Write(byte value)
		{
			m_Bytes.Add(value);
			m_WriteOffset += sizeof(byte);
		}

		public void Write(ushort value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(ushort);
		}

		public void Write(uint value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(uint);
		}

		public void Write(ulong value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(ulong);
		}

		public void Write(sbyte value)
		{
			m_Bytes.Add((byte)value);
			m_WriteOffset += sizeof(sbyte);
		}

		public void Write(char value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(byte);
		}

		public void Write(short value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(short);
		}

		public void Write(int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(int);
		}

		public void Write(long value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(long);
		}

		public void Write(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(float);
		}

		public void Write(double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(double);
		}

		public void Write(decimal value)
		{
			byte[] bytes = BitConverter.GetBytes(Convert.ToDouble(value));
			m_Bytes.AddRange(bytes);
			m_WriteOffset += sizeof(decimal);
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
