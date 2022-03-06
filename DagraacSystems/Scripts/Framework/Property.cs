using DagraacSystems.Node;
using System.Collections.Generic;


namespace DagraacSystems.Framework
{
	/// <summary>
	/// 속성 객체의 기본 틀.
	/// </summary>
	public class Property : Object
	{
		public class PropertyValue
		{
			public enum ValueType { Number, Real, Boolean, Text, Array, }
			public ValueType Type = ValueType.Number;
			public int Number = 0;
			public float Real = 0f;
			public bool Boolean = false;
			public string Text = string.Empty;
			public List<PropertyValue> Array = new List<PropertyValue>();
		}



		public string Name;
		public Property Parent;
		public List<Property> Children;
		public PropertyValue Value;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Property() : base()
		{
			Name = string.Empty;
			Parent = null;
			Children = new List<Property>();
			Value = new PropertyValue();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		public static void SaveProperty(Property property)
		{

		}
	}
}