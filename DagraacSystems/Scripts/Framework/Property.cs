using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	public enum ValueType
	{
		None,
		Number,
		Real,
		Boolean,
		Text,
		Object,
		Array
	}

	public interface IValue
	{
		object GetValue();
		ValueType GetType();
	}

	public struct Value : IValue
	{
		public ValueType Type;
		public int Number;
		public double Real;
		public bool Boolean;
		public string Text;
		public Nullable<Value> Object;
		public List<Value> Array;

		public Value(int value)
		{
			Type = ValueType.Number;
			Number = value;
			Real = 0.0;
			Boolean = false;
			Text = string.Empty;
			Object = null;
			Array = new List<Value>();
		}

		public Value(double value)
		{
			Type = ValueType.Real;
			Number = 0;
			Real = value;
			Boolean = false;
			Text = string.Empty;
			Object = null;
			Array = new List<Value>();
		}

		public Value(bool value)
		{
			Type = ValueType.Boolean;
			Number = 0;
			Real = 0.0;
			Boolean = value;
			Text = string.Empty;
			Object = null;
			Array = new List<Value>();
		}

		public Value(string value)
		{
			Type = ValueType.Text;
			Number = 0;
			Real = 0.0;
			Boolean = false;
			Text = value;
			Object = null;
			Array = new List<Value>();
		}

		public Value(Value value)
		{
			Type = value.Type;
			Number = value.Number;
			Real = value.Real;
			Boolean = value.Boolean;
			Text = value.Text;
			Object = new Nullable<Value>(value.Object.Value);
			Array = new List<Value>(value.Array);
		}

		public Value(List<Value> value)
		{
			Type = ValueType.Array;
			Number = 0;
			Real = 0.0;
			Boolean = false;
			Text = string.Empty;
			Object = null;
			Array = new List<Value>(value);
		}

		object IValue.GetValue()
		{
			switch(Type)
			{
				case ValueType.Number:
					return Number;
				case ValueType.Real:
					return Real;
				case ValueType.Text:
					return Text;
				case ValueType.Array:
					return Array;
				case ValueType.Object:
					return Object;
			}

			return null;
		}

		ValueType IValue.GetType()
		{
			return Type;
		}

		public static implicit operator Value(int value) => new Value(value);
		public static implicit operator Value(double value) => new Value(value);
		public static implicit operator Value(bool value) => new Value(value);
		public static implicit operator Value(string value) => new Value(value);
		public static implicit operator Value(List<Value> value) => new Value(value);

		public static implicit operator int(Value value) => value.Number;
		public static implicit operator double(Value value) => value.Real;
		public static implicit operator bool(Value value) => value.Boolean;
		public static implicit operator string(Value value) => value.Text;
		public static implicit operator List<Value>(Value value) => value.Array;

		public static ValueType GetValueType(object value)
		{
			//var type = value.GetType();
			//switch (type)
			//{
			//	case typeof(long):
			//	case typeof(int):
			//	case typeof(short):
			//		return ValueType.Number;
			//}
			return ValueType.Object;
		}
	}


	/// <summary>
	/// 속성 객체의 기본 틀.
	/// </summary>
	public class Property : FrameworkObject
	{
		public string Name;
		public Property Parent;
		public List<Property> Children;
		public Value Value;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Property() : base()
		{
			Name = string.Empty;
			Parent = null;
			Children = new List<Property>();
			Value = new Value();
		}

		public void SetValue(Value value)
		{
			Value = value;
		}

		public Value GetValue()
		{
			return Value;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		public static bool LoadProperty(byte[] bytes, out Property property)
		{
			property = new Property();
			return false;
		}

		public static bool SaveProperty(Property property, out byte[] bytes)
		{
			bytes = new byte[0];
			return false;
		}

		public bool IsDirty()
		{
			return false;
		}
	}
}