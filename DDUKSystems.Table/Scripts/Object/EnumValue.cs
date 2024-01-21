namespace DDUKSystems
{
	public class EnumValue
	{
		public string Value;

		public EnumValue(string value)
		{
			Value = value;
		}

		public T To<T>() where T : System.Enum
		{
			// c# 시스템 안에서 쉼표 넣어주면 알아서 컴바인플래그로 구분된다. "Enum1, Enum2, Enum3"
			// ignoreCase 옵션에 의해 대소문자 가리지 않음.
			// 값이 없거나 잘못될 경우 enum의 default value를 반환. (코딩규약에서 default는 항상 Invalid or None 작업 필요)
			try
			{
				return (T)System.Enum.Parse(typeof(T), Value, true);
			}
			catch
			{
				return default;
			}
		}

		public static implicit operator EnumValue(string value)
		{
			return new EnumValue(value);
		}

		public static implicit operator string(EnumValue value)
		{
			return value.ToString();
		}

		public override string ToString()
		{
			return Value;
		}
	}
}