using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	public static class ContainerExtension
	{
		public static void RemoveAll<TValue>(this List<TValue> list, Predicate<TValue> predicate)
		{
			for (var i = 0; i < list.Count; ++i)
			{
				if (!predicate?.Invoke(list[i]) ?? true)
					continue;

				list.RemoveAt(i);
				--i;
			}
		}

		public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			var removes = new List<TKey>();
			foreach (var pair in dictionary)
			{
				if (!predicate?.Invoke(pair) ?? true)
					continue;

				removes.Add(pair.Key);
			}

			foreach (var remove in removes)
				dictionary.Remove(remove);
		}
	}
}