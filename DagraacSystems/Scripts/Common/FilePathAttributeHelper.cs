using System;


namespace DagraacSystems
{
	public static class FilePathAttributeHelper
	{
		public static string GetFilePath(Enum value)
		{
			var type = value.GetType();
			var fieldInfo = type.GetField(value.ToString());
			var attributes = fieldInfo.GetCustomAttributes(typeof(FilePathAttribute), false) as FilePathAttribute[];
			if (attributes.Length > 0)
				return attributes[0].Value;
			return string.Empty;
		}
	}
}