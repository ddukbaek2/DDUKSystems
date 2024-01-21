using System;
using System.Collections.Generic;


namespace DDUKSystems
{
	/// <summary>
	/// 통합된 숫자 클래스.
	/// </summary>
	public class Number : IComparable, IComparable<Number>, IEquatable<Number>, IFormattable, IConvertible
	{
		private struct NumberType
		{
			public byte Size { set; get; }
			public Type Type { set; get; }
		}


		private static Dictionary<TypeCode, NumberType> s_NumberTypes = new Dictionary<TypeCode, NumberType>()
		{
			//{ TypeCode.Empty,		new NumberType { Size = 0, Type = null } },
			//{ TypeCode.Object,	new NumberType { Size = 0, Type = null } },
			//{ TypeCode.DBNull,	new NumberType { Size = 0, Type = null } },
			{ TypeCode.Boolean,		new NumberType { Size = sizeof(bool), Type = typeof(bool) } }, // 8bit.
			{ TypeCode.Char,        new NumberType { Size = sizeof(char), Type = typeof(char) } }, // 16bit.
			{ TypeCode.SByte,		new NumberType { Size = sizeof(sbyte), Type = typeof(sbyte) } }, // 8bit.
			{ TypeCode.Byte,		new NumberType { Size = sizeof(byte), Type = typeof(byte) } }, // 8bit.
			{ TypeCode.Int16,		new NumberType { Size = sizeof(short), Type = typeof(short) } }, // 16bit.
			{ TypeCode.UInt16,		new NumberType { Size = sizeof(ushort), Type = typeof(ushort) } }, // 16bit.
			{ TypeCode.Int32,		new NumberType { Size = sizeof(int), Type = typeof(int) } }, // 32bit.
			{ TypeCode.UInt32,		new NumberType { Size = sizeof(uint), Type = typeof(uint) } }, // 32bit.
			{ TypeCode.Int64,		new NumberType { Size = sizeof(long), Type = typeof(long) } }, // 64bit.
			{ TypeCode.UInt64,		new NumberType { Size = sizeof(ulong), Type = typeof(ulong) } }, // 64bit.
			{ TypeCode.Single,		new NumberType { Size = sizeof(float), Type = typeof(float) } }, // 32bit.
			{ TypeCode.Double,		new NumberType { Size = sizeof(double), Type = typeof(double) } }, // 64bit.
			{ TypeCode.Decimal,		new NumberType { Size = sizeof(decimal), Type = typeof(decimal) } }, // 128bit.
			{ TypeCode.DateTime,	new NumberType { Size = sizeof(long), Type = typeof(DateTime) } }, // 64bit.
			//{ TypeCode.String,	new NumberType { Size = 0, Type = null } },
		};

		private static Dictionary<TypeCode, Func<object, byte[]>> s_NumberToBytes = new Dictionary<TypeCode, Func<object, byte[]>>()
		{
			//{ TypeCode.Empty,		(number) => new byte[0] { 0x00 } },
			//{ TypeCode.Object,	(number) => new byte[0] { 0x00 } },
			//{ TypeCode.DBNull,	(number) => new byte[0] { 0x00 } },
			{ TypeCode.Boolean,     (number) => BitConverter.GetBytes((bool)number) },
			{ TypeCode.Char,        (number) => BitConverter.GetBytes((char)number) },
			{ TypeCode.SByte,       (number) => new byte[1] { (byte)number } },
			{ TypeCode.Byte,        (number) => new byte[1] { (byte)number } },
			{ TypeCode.Int16,       (number) => BitConverter.GetBytes((short)number) },
			{ TypeCode.UInt16,      (number) => BitConverter.GetBytes((ushort)number) },
			{ TypeCode.Int32,       (number) => BitConverter.GetBytes((int)number) },
			{ TypeCode.UInt32,      (number) => BitConverter.GetBytes((uint)number) },
			{ TypeCode.Int64,       (number) => BitConverter.GetBytes((long)number) },
			{ TypeCode.UInt64,      (number) => BitConverter.GetBytes((ulong)number) },
			{ TypeCode.Single,      (number) => BitConverter.GetBytes((float)number) },
			{ TypeCode.Double,      (number) => BitConverter.GetBytes((double)number) },
			{ TypeCode.Decimal,     (number) => DecimalToBytes((decimal)number) },
			{ TypeCode.DateTime,    (number) => BitConverter.GetBytes(((DateTime)number).Ticks) },
			//{ TypeCode.String,	(number) => new byte[0] { 0x00 } },
		};

		private static Dictionary<TypeCode, Func<byte[], object>> s_ByteToNumbers = new Dictionary<TypeCode, Func<byte[], object>>()
		{
			//{ TypeCode.Empty,		(bytes) => (object)0x00 },
			//{ TypeCode.Object,	(bytes) => (object)0x00 },
			//{ TypeCode.DBNull,	(bytes) => (object)0x00 },
			{ TypeCode.Boolean,		(bytes) => (object)bytes[0] },
			{ TypeCode.Char,		(bytes) => (object)BitConverter.ToChar(bytes, 0) },
			{ TypeCode.SByte,		(bytes) => (object)bytes[0] },
			{ TypeCode.Byte,		(bytes) => (object)bytes[0] },
			{ TypeCode.Int16,		(bytes) => (object)BitConverter.ToInt16(bytes, 0) },
			{ TypeCode.UInt16,		(bytes) => (object)BitConverter.ToUInt16(bytes, 0) },
			{ TypeCode.Int32,		(bytes) => (object)BitConverter.ToInt32(bytes, 0) },
			{ TypeCode.UInt32,		(bytes) => (object)BitConverter.ToUInt32(bytes, 0) },
			{ TypeCode.Int64,		(bytes) => (object)BitConverter.ToInt64(bytes, 0) },
			{ TypeCode.UInt64,		(bytes) => (object)BitConverter.ToUInt64(bytes, 0) },
			{ TypeCode.Single,		(bytes) => (object)BitConverter.ToSingle(bytes, 0) },
			{ TypeCode.Double,		(bytes) => (object)BitConverter.ToDouble(bytes, 0) },
			{ TypeCode.Decimal,		(bytes) => (object)BytesToDecimal(bytes) },
			{ TypeCode.DateTime,    (bytes) => (object)new DateTime(BitConverter.ToInt64(bytes, 0)) },
			//{ TypeCode.String,	(bytes) => (object)0x00 },
		};

		private Type m_Type;
		private List<byte> m_Buffer;
		private byte[] m_Bytes;

		public bool BoolValue => GetValue<bool>();
		public char CharValue => GetValue<char>();
		public sbyte SByteValue => GetValue<sbyte>();
		public short ShortValue => GetValue<short>();
		public int IntValue => GetValue<int>();
		public long LongValue => GetValue<long>();
		public byte ByteValue => GetValue<byte>();
		public ushort UShortValue => GetValue<ushort>();
		public uint UIntValue => GetValue<uint>();
		public ulong ULongValue => GetValue<ulong>();
		public float FloatValue => GetValue<float>();
		public double DoubleValue => GetValue<double>();
		public decimal DecimalValue => GetValue<decimal>();
		public DateTime DateTimeValue => GetValue<DateTime>();

		public Number()
		{
			m_Type = typeof(int);
			m_Buffer = new List<byte>();
			m_Bytes = m_Buffer.ToArray();
		}

		public Number(Type type)
		{
			m_Type = type;
			m_Buffer = new List<byte>();
			SetValue(default);
		}

		public Number(Number other)
		{
			m_Type = other.m_Type;
			m_Buffer = new List<byte>();
			m_Buffer.AddRange(other.m_Buffer);
			m_Bytes = m_Buffer.ToArray();
		}

		/// <summary>
		/// 타입 변환.
		/// </summary>
		public void ChangeType(TypeCode typeCode)
		{
			if (!s_NumberTypes.TryGetValue(typeCode, out var numberType))
			{
				throw new NotSupportedException("not supported number type.");
			}

			m_Type = numberType.Type;
			m_Buffer.Clear();
			m_Buffer.Capacity = numberType.Size;
			for (var i = 0; i < numberType.Size; ++i)
				m_Buffer.Add(0x00);
			m_Bytes = m_Buffer.ToArray();
		}

		/// <summary>
		/// 지원 타입 여부.
		/// </summary>
		public static bool IsSupportedType(TypeCode typeCode)
		{
			return s_NumberTypes.ContainsKey(typeCode);
		}

		/// <summary>
		/// 지원 타입 여부.
		/// </summary>
		public static bool IsSupportedType(Type type)
		{
			return IsSupportedType(ConvertTypeToTypeCode(type));
		}

		/// <summary>
		/// 값 설정.
		/// </summary>
		public bool SetValue<T>(T value)
		{
			var typeCode = Number.ConvertTypeToTypeCode(typeof(T));
			if (!s_NumberToBytes.TryGetValue(typeCode, out var convertFunction))
				return false;

			if (GetTypeCode() != typeCode)
				ChangeType(typeCode);

			var bytes = convertFunction(value);

			m_Buffer.Clear();
			m_Buffer.AddRange(bytes);
			m_Bytes = m_Buffer.ToArray();

			return true;
		}

		/// <summary>
		/// 값 설정.
		/// </summary>
		public bool SetValue(object value)
		{
			var typeCode = Number.ConvertTypeToTypeCode(value.GetType());
			if (!s_NumberToBytes.TryGetValue(typeCode, out var convertFunction))
				return false;

			if (GetTypeCode() != typeCode)
				ChangeType(typeCode);

			var bytes = convertFunction(value);

			m_Buffer.Clear();
			m_Buffer.AddRange(bytes);
			m_Bytes = m_Buffer.ToArray();

			return true;
		}

		/// <summary>
		/// 값 반환.
		/// </summary>
		public T GetValue<T>()
		{
			var typeCode = GetTypeCode();
			if (!s_ByteToNumbers.TryGetValue(typeCode, out var convertFunction))
				return default;

			return (T)convertFunction(m_Bytes);
		}

		/// <summary>
		/// 값 반환.
		/// </summary>
		public object GetValue()
		{
			var typeCode = GetTypeCode();
			if (!s_ByteToNumbers.TryGetValue(typeCode, out var convertFunction))
				return default;

			switch (typeCode)
			{
				case TypeCode.Boolean:	return (bool)convertFunction(m_Bytes);
				case TypeCode.Char:		return (char)convertFunction(m_Bytes);
				case TypeCode.SByte:	return (sbyte)convertFunction(m_Bytes);
				case TypeCode.Byte:		return (byte)convertFunction(m_Bytes);
				case TypeCode.Int16:	return (short)convertFunction(m_Bytes);
				case TypeCode.UInt16:	return (ushort)convertFunction(m_Bytes);
				case TypeCode.Int32:	return (int)convertFunction(m_Bytes);
				case TypeCode.UInt32:	return (uint)convertFunction(m_Bytes);
				case TypeCode.Int64:	return (long)convertFunction(m_Bytes);
				case TypeCode.UInt64:	return (ulong)convertFunction(m_Bytes);
				case TypeCode.Single:	return (float)convertFunction(m_Bytes);
				case TypeCode.Double:	return (double)convertFunction(m_Bytes);
				case TypeCode.Decimal:	return (decimal)convertFunction(m_Bytes);
				case TypeCode.DateTime:	return (DateTime)convertFunction(m_Bytes);
				default:				throw new NotSupportedException("not supported object type.");
			}
		}

		/// <summary>
		/// 인터페이스 구현.
		/// </summary>
		int IComparable.CompareTo(object other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (this == other)
				return 0;

			if (other is Number number)
			{
				if (!IsSupportedType(number.m_Type))
					throw new NotSupportedException("not supported number type.");

				return CompareTo(number);
			}

			throw new NotSupportedException("not supported object type.");
		}

		/// <summary>
		/// 비교.
		/// </summary>
		public int CompareTo(Number other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (this == other)
				return 0;

			var typeCode = GetTypeCode();
			if (typeCode != other.GetTypeCode())
				throw new NotSupportedException("mismatch typeCode.");

			switch (typeCode)
			{
				case TypeCode.Boolean:	return BoolValue.CompareTo(other.BoolValue);
				case TypeCode.Char:		return CharValue.CompareTo(other.CharValue);
				case TypeCode.SByte:	return SByteValue.CompareTo(other.SByteValue);
				case TypeCode.Byte:		return ByteValue.CompareTo(other.ByteValue);
				case TypeCode.Int16:	return ShortValue.CompareTo(other.ShortValue);
				case TypeCode.UInt16:	return UShortValue.CompareTo(other.UShortValue);
				case TypeCode.Int32:	return IntValue.CompareTo(other.IntValue);
				case TypeCode.UInt32:	return UIntValue.CompareTo(other.UIntValue);
				case TypeCode.Int64:	return LongValue.CompareTo(other.LongValue);
				case TypeCode.UInt64:	return ULongValue.CompareTo(other.ULongValue);
				case TypeCode.Single:	return FloatValue.CompareTo(other.FloatValue);
				case TypeCode.Double:	return DoubleValue.CompareTo(other.DoubleValue);
				case TypeCode.Decimal:	return DecimalValue.CompareTo(other.DecimalValue);
				case TypeCode.DateTime:	return DateTimeValue.CompareTo(other.DateTimeValue);
				default:				throw new NotSupportedException("not supported object type.");
			}
		}

		/// <summary>
		/// 비교.
		/// - 같은 객체일 경우 같다고 판단.
		/// - 타입과 값이 같은 개체도 같다고 판단.
		/// </summary>
		public bool Equals(Number other)
		{
			if (other == null)
				return false;

			if (this == other)
				return true;

			try
			{
				var typeCode = GetTypeCode();
				if (typeCode != other.GetTypeCode())
					return false;

				return CompareTo(other) == 0;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 문자열 변환.
		/// </summary>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			object value = GetValue();
			if (value is IFormattable formattable)
			{
				return formattable.ToString(format, formatProvider);
			}
			else
			{
				return value.ToString();
			}
		}

		/// <summary>
		/// 현재 수의 타입코드 반환.
		/// </summary>
		public TypeCode GetTypeCode()
		{
			return Type.GetTypeCode(m_Type);
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return GetValue<bool>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return GetValue<byte>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return GetValue<char>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return GetValue<DateTime>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return GetValue<decimal>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return GetValue<double>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return GetValue<short>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return GetValue<int>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return GetValue<long>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return GetValue<sbyte>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return GetValue<float>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// 단, 문자열 변환은 지원하지 않음.
		/// </summary>
		string IConvertible.ToString(IFormatProvider provider)
		{
			var value = GetValue();
			return value.ToString();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// 단, 다른 타입으로의 변환은 지원하지 않음.
		/// </summary>
		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			if (conversionType == null)
				throw new ArgumentNullException(nameof(conversionType));

			object value = GetValue();

			try
			{
				return Convert.ChangeType(value, conversionType, provider);
			}
			catch (InvalidCastException)
			{
				throw new InvalidCastException($"Cannot convert Number to {conversionType.Name}");
			}
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return GetValue<ushort>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return GetValue<uint>();
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return GetValue<ulong>();
		}

		/// <summary>
		/// 타입코드를 타입으로 변환.
		/// </summary>
		private static Type ConvertTypeCodeToType(TypeCode typeCode)
		{
			if (!s_NumberTypes.TryGetValue(typeCode, out var numberType))
				return null;

			return numberType.Type;
		}

		/// <summary>
		/// 타입을 타입코드로 변환.
		/// </summary>
		private static TypeCode ConvertTypeToTypeCode(Type type)
		{
			if (type == null)
				return TypeCode.Empty;

			foreach (var numberType in s_NumberTypes)
			{
				if (numberType.Value.Type == type)
					return numberType.Key;
			}

			return TypeCode.Empty;
		}

		/// <summary>
		/// 디시멀을 바이트배열로 변환.
		/// </summary>
		private static byte[] DecimalToBytes(decimal value)
		{
			var intArray = decimal.GetBits(value);
			var bytes = new byte[16];
			for (var i = 0; i < 4; ++i)
				Array.Copy(BitConverter.GetBytes(intArray[i]), 0, bytes, i * 4, 4);

			return bytes;
		}

		/// <summary>
		/// 바이트배열을 디시멀로 변환.
		/// </summary>
		private static decimal BytesToDecimal(byte[] bytes)
		{
			var intArray = new int[4];
			for (var i = 0; i < 4; ++i)
				intArray[i] = BitConverter.ToInt32(bytes, i * 4);

			return new decimal(intArray);
		}
	}
}