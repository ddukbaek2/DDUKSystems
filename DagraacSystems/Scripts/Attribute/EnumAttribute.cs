using System;


namespace DagraacSystems
{
	public class EnumAttribute : Attribute
	{
		public string LocalizationKey { private set; get; }

		public EnumAttribute(string localizationKey)
		{
			LocalizationKey = localizationKey;
		}
	}
}