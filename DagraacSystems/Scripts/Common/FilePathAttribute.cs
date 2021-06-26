using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	public class FilePathAttribute : Attribute
	{
		public string Value { private set; get; }

		public FilePathAttribute(string filePath)
		{
			Value = filePath;
		}
	}
}