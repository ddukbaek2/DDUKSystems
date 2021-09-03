//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace DagraacSystems
//{
//	public class Trie<T>
//	{
//		protected Trie<T> _previous;
//		protected SortedDictionary<T, Trie<T>> _next;
//		protected T _value;

//		public Trie(Trie<T> previous = null, T value = default)
//		{
//			_previous = previous;
//			_next = new SortedDictionary<T, Trie<T>>();
//			_value = value;
//		}

//		public void Clear()
//		{
//			_next.Clear();
//		}

//		public void Add(IEnumerator<T> enumerator)
//		{
//			if (enumerator == null)
//				return;

//			var node = this;
//			while (enumerator.MoveNext())
//			{
//				var value = enumerator.Current;
//				if (!node._next.TryGetValue(value, out var next))
//				{
//					next = new Trie<T>(node, value);
//					node._next.Add(value, next);
//					node = next;
//				}
//			}
//		}

//		public void Remove(IEnumerator<T> enumerator)
//		{
//			var nodes = Find(enumerator);
//			if (nodes == null)
//				return;

//			nodes.Reverse();
//			foreach (var node in nodes)
//				node._previous._next.Remove(node._value);
//		}

//		public List<Trie<T>> Find(IEnumerator<T> enumerator)
//		{
//			if (enumerator == null)
//				return null;

//			var result = new List<Trie<T>>();

//			var node = this;
//			while (enumerator.MoveNext())
//			{
//				var key = enumerator.Current;
//				if (!node._next.TryGetValue(key, out var next))
//					return null;

//				node = next;
//				result.Add(next);
//			}

//			return result;
//		}

//		public bool Exists(IEnumerator<T> enumerator)
//		{
//			if (enumerator == null)
//				return false;

//			var node = this;
//			while (enumerator.MoveNext())
//			{
//				var key = enumerator.Current;
//				if (!node._next.TryGetValue(key, out var next))
//					return false;

//				node = next;
//			}

//			return true;
//		}
//	}

//	public class CharTrie : Trie<char>
//	{
//		public void Add(string text)
//		{
//			Add(text.GetEnumerator());
//		}

//		public void Like()
//		{
//		}

//		public void Match()
//		{
//		}
//	}

//	public class TextFilter
//	{
//		private CharTrie _bannedWords;

//		public TextFilter()
//		{
//			_bannedWords = new CharTrie();
//		}

//		public void LoadTable()
//		{
//			_bannedWords.Add("씨발");
//			_bannedWords.Add("씨발놈");
//			_bannedWords.Add("좆");
//			_bannedWords.Add("좆같은새끼");
//		}

//		public bool CheckByMatch(string text)
//		{
//			_bannedWords.Exists()
//		}
//	}
//}