using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 열거체값과 문자열값을 연결.
	/// </summary>
	public class ValueAttribute : Attribute
	{
		private string _value;
		public string Value => _value;

		public ValueAttribute(string value)
		{
			_value = value;
		}
	}
}