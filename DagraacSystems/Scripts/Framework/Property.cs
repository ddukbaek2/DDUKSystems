using System.Collections.Generic;


namespace DagraacSystems.Framework
{
	public class Value
	{
		public enum eType { Number, Real, Boolean, Text, Array, }

		public eType Type;
		public int Number;
		public float Real;
		public bool Boolean;
		public string Text;

		public List<Value> Array;
	}


	/// <summary>
	/// 속성 객체의 기본 틀.
	/// </summary>
	public class Property : Object
	{
		public string Name;
		public string Value;
		public Property Parent;
		public List<Property> Children;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Property() : base()
		{
			Name = string.Empty;
			Value = string.Empty;
			Parent = null;
			Children = new List<Property>();
		}

		/// <summary>
		/// 파괴됨.
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