namespace DagraacSystems.Table.Extension
{
	public struct Number
	{
		public enum NumberType
		{
			String,
			SByte,
			Byte,
			Short,
			UShort,
			Int,
			UInt,
			Long,
			ULong,
			Float,
			Double,
		}

		public NumberType Type;
		public string Value;

		public Number(string value)
		{
			Value = value;
			Type = GetNumberType(value);
		}

		public Number(string value, NumberType type)
		{
			Value = value;
			Type = type;
		}

		public override string ToString()
		{
			return Value;
		}

		public static NumberType GetNumberType(string value)
		{
			if (value.Contains("u"))
			{
				if (byte.TryParse(value, out var byteValue))
					return NumberType.Byte;
				else if (ushort.TryParse(value, out var usrhotValue))
					return NumberType.UShort;
				else if (uint.TryParse(value, out var uintValue))
					return NumberType.UInt;
				else if (ulong.TryParse(value, out var ulongValue))
					return NumberType.ULong;
				else
					return NumberType.String;
			}
			else if (value.Contains("."))
			{
				if (float.TryParse(value, out var floatValue))
					return NumberType.Float;
				else if (double.TryParse(value, out var doubleValue))
					return NumberType.Double;
				else
					return NumberType.String;
			}
			else
			{
				if (sbyte.TryParse(value, out var sbyteValue))
					return NumberType.SByte;
				else if (short.TryParse(value, out var srhotValue))
					return NumberType.Short;
				else if (int.TryParse(value, out var intValue))
					return NumberType.Int;
				else if (long.TryParse(value, out var longValue))
					return NumberType.Long;
				else
					return NumberType.String;
			}
		}

		public static implicit operator Number(string value)
		{
			return new Number(value, NumberType.String);
		}

		public static implicit operator Number(sbyte value)
		{
			return new Number(value.ToString(), NumberType.SByte);
		}

		public static implicit operator Number(short value)
		{
			return new Number(value.ToString(), NumberType.Short);
		}

		public static implicit operator Number(int value)
		{
			return new Number(value.ToString(), NumberType.Int);
		}

		public static implicit operator Number(long value)
		{
			return new Number(value.ToString(), NumberType.UInt);
		}

		public static implicit operator Number(byte value)
		{
			return new Number(value.ToString(), NumberType.Byte);
		}

		public static implicit operator Number(ushort value)
		{
			return new Number(value.ToString(), NumberType.UShort);
		}

		public static implicit operator Number(uint value)
		{
			return new Number(value.ToString(), NumberType.UInt);
		}

		public static implicit operator Number(ulong value)
		{
			return new Number(value.ToString(), NumberType.ULong);
		}

		public static implicit operator Number(float value)
		{
			return new Number(value.ToString(), NumberType.Float);
		}

		public static implicit operator Number(double value)
		{
			return new Number(value.ToString(), NumberType.Double);
		}

		public static implicit operator string(Number value)
		{
			return value.ToString();
		}

		public static implicit operator sbyte(Number value)
		{
			return (sbyte)(value.Type == NumberType.SByte ? sbyte.Parse(value.Value) : 0);
		}

		public static implicit operator short(Number value)
		{
			return (short)(value.Type == NumberType.Short ? short.Parse(value.Value) : 0);
		}

		public static implicit operator int(Number value)
		{
			return value.Type == NumberType.Int ? int.Parse(value.Value) : 0;
		}

		public static implicit operator long(Number value)
		{
			return value.Type == NumberType.Long ? long.Parse(value.Value) : 0L;
		}

		public static implicit operator byte(Number value)
		{
			return (byte)(value.Type == NumberType.Byte ? byte.Parse(value.Value) : 0);
		}

		public static implicit operator ushort(Number value)
		{
			return (ushort)(value.Type == NumberType.UShort ? ushort.Parse(value.Value) : 0);
		}

		public static implicit operator uint(Number value)
		{
			return value.Type == NumberType.UInt ? uint.Parse(value.Value) : 0u;
		}

		public static implicit operator ulong(Number value)
		{
			return value.Type == NumberType.ULong ? ulong.Parse(value.Value) : 0ul;
		}

		public static implicit operator float(Number value)
		{
			return value.Type == NumberType.Float ? float.Parse(value.Value) : 0.0f;
		}

		public static implicit operator double(Number value)
		{
			return value.Type == NumberType.Double ? double.Parse(value.Value) : 0.0d;
		}
	}
}
