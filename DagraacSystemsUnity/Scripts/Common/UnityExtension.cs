using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace DagraacSystems.Unity
{
	public static class UnityExtension
	{
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour
		{
			var component = gameObject.GetComponent<T>();
			if (component)
				return component;

			component = gameObject.AddComponent<T>();
			return component;
		}

		public static Color MakeColor(string hexColor)
		{
			var color = Color.white;

			var prevHexNumber = 0;
			var index = 0;
			foreach (var hexChar in hexColor)
			{
				if (hexChar == 35) // #
					continue;

				var hexNumber = 0;
				if (hexChar > 47 && hexChar < 58) // 숫자 0~9
					hexNumber = hexChar - 48;
				else if (hexChar > 64 && hexChar < 71) // 대문자 A~F.
					hexNumber = (hexChar - 65) + 10;
				else if (hexChar > 96 && hexChar < 103) // 소문자 a~f.
					hexNumber = (hexChar - 97) + 10;

				if (index % 2 != 0)
					color[index / 2] = hexNumber + (prevHexNumber * 16);
				else
					prevHexNumber = hexNumber;

				++index;
			}

			return color;
		}
	}
}
