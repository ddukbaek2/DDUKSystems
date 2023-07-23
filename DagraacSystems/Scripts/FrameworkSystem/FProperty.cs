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

	public class Variable : IValue
	{
		public ValueType Type;
		public int Number;
		public double Real;
		public bool Boolean;
		public string Text;
		public Variable Object;
		public List<Variable> Array;

		public Variable()
		{
			Type = ValueType.None;
			Number = 0;
			Real = 0.0;
			Boolean = false;
			Text = string.Empty;
			Object = null;
			Array = new List<Variable>();
		}

		public Variable(int value)
		{
			Type = ValueType.Number;
			Number = value;
			Real = 0.0;
			Boolean = false;
			Text = string.Empty;
			Object = null;
			Array = new List<Variable>();
		}

		public Variable(double value)
		{
			Type = ValueType.Real;
			Number = 0;
			Real = value;
			Boolean = false;
			Text = string.Empty;
			Object = null;
			Array = new List<Variable>();
		}

		public Variable(bool value)
		{
			Type = ValueType.Boolean;
			Number = 0;
			Real = 0.0;
			Boolean = value;
			Text = string.Empty;
			Object = null;
			Array = new List<Variable>();
		}

		public Variable(string value)
		{
			Type = ValueType.Text;
			Number = 0;
			Real = 0.0;
			Boolean = false;
			Text = value;
			Object = null;
			Array = new List<Variable>();
		}

		public Variable(Variable value)
		{
			Type = value.Type;
			Number = value.Number;
			Real = value.Real;
			Boolean = value.Boolean;
			Text = value.Text;
			Object = value.Object;
			Array = new List<Variable>(value.Array);
		}

		public Variable(List<Variable> value)
		{
			Type = ValueType.Array;
			Number = 0;
			Real = 0.0;
			Boolean = false;
			Text = string.Empty;
			Object = null;
			Array = new List<Variable>(value);
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
				default:
					return null;
			}
		}

		ValueType IValue.GetType()
		{
			return Type;
		}

		public static implicit operator Variable(int value) => new Variable(value);
		public static implicit operator Variable(double value) => new Variable(value);
		public static implicit operator Variable(bool value) => new Variable(value);
		public static implicit operator Variable(string value) => new Variable(value);
		public static implicit operator Variable(List<Variable> value) => new Variable(value);

		public static implicit operator int(Variable value) => value.Number;
		public static implicit operator double(Variable value) => value.Real;
		public static implicit operator bool(Variable value) => value.Boolean;
		public static implicit operator string(Variable value) => value.Text;
		public static implicit operator List<Variable>(Variable value) => value.Array;

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
	public class FProperty : FObject
	{
		public string Name;
		public FProperty Parent;
		public List<FProperty> Children;
		public Variable Value;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public FProperty() : base()
		{
			Name = string.Empty;
			Parent = null;
			Children = new List<FProperty>();
			Value = new Variable();
		}

		public void SetValue(Variable value)
		{
			Value = value;
		}

		public Variable GetValue()
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

		public static bool LoadProperty(byte[] bytes, out FProperty property)
		{
			property = new FProperty();
			return false;
		}

		public static bool SaveProperty(FProperty property, out byte[] bytes)
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