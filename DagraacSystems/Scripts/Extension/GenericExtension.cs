using System;
using System.Collections.Generic;


namespace DagraacSystems.Extension
{
	public static class GenericExtension
	{
		private static List<int> s_Indices = new List<int>();

		public static bool TryGetKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value, out TKey key)
		{
			key = default;
			foreach (var pair in dictionary)
			{
				if (pair.Value.Equals(value))
				{
					key = pair.Key;
					return true;
				}
			}

			return false;
		}

		public static bool RemoveIf<TValue>(this List<TValue> list, Predicate<TValue> match)
		{
			if (match == null)
				return false;

			var index = list.FindIndex(match);
			if (index == -1)
				return false;

			list.RemoveAt(index);
			return true;
		}

		public static bool RemoveIfAll<TValue>(this List<TValue> list, Predicate<TValue> match)
		{
			if (match == null)
				return false;

			s_Indices.Clear();
			for (var i = 0; i < list.Count; ++i)
			{
				var value = list[i];
				if (match(value))
				{
					s_Indices.Add(i);
				}
			}

			s_Indices.Sort();
			for (var i = s_Indices.Count - 1; i >= 0; --i)
			{
				var index = s_Indices[i];
				list.RemoveAt(index);
			}

			return true;
		}
	}
}