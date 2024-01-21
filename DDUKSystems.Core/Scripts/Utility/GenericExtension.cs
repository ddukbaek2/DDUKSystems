using System; // Predicate
using System.Collections.Generic; // List, Dictionary


namespace DDUKSystems
{
    /// <summary>
    /// 제너릭 확장.
    /// </summary>
    public static class GenericExtension
    {
        private static List<int> s_Indices = new List<int>();

        /// <summary>
        /// 값으로 키를 반환.
        /// </summary>
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

        /// <summary>
        /// 조건에 맞는 대상 제거.
        /// </summary>
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

        /// <summary>
        /// 조건에 맞는 모든 대상 제거.
        /// </summary>
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