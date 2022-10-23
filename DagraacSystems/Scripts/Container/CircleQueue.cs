using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;


namespace DagraacSystems
{
	public class CircleQueue<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
	{
		private class Enumerator : IEnumerator<T>
		{
			private T[] _data;
			ValueTuple<int, int, int, int> _range;
			private int m_Index;
			private T _current;
			public int HeadIndex => _range.Item1;
			public int TailIndex => _range.Item2;
			public int Count => _range.Item3;
			public int Capacity => _range.Item4;

			public T Current => _current;
			object IEnumerator.Current => _current;

			public Enumerator(CircleQueue<T> queue)
			{
				_data = queue._data;
				_range = (queue._headIndex, queue._tailIndex, queue.Count, queue.Capacity);
				m_Index = HeadIndex;
				_current = default;
			}

			public void Dispose()
			{
				_data = null;
				_current = default;
			}

			public bool MoveNext()
			{
				if (Count == 0)
					return false;

				_current = _data[m_Index];
				if (m_Index < TailIndex)
				{
					++m_Index;
					return (m_Index < TailIndex);
				}
				else
				{
					if (m_Index >= Capacity)
						m_Index = 0;
					else
						++m_Index;

					return (m_Index < TailIndex);
				}
			}

			public void Reset()
			{
				m_Index = HeadIndex;
				_current = default;
			}
		}

		private T[] _data;
		private int _headIndex;
		private int _tailIndex;

		public CircleQueue(int capacity)
		{
			_data = new T[capacity];
			_headIndex = 0;
			_tailIndex = 0;
		}

		public T[] Data => _data;
		public T Front => _data[_headIndex];
		public T Back => _data[_tailIndex];
		public int Capacity => _data.Length;

		public int Count
		{
			get
			{
				if (_headIndex < _tailIndex)
				{
					return (_tailIndex - _headIndex) + 1;
				}
				else if (_headIndex > _tailIndex)
				{
					return (_tailIndex + (Capacity - 1 - _headIndex)) + 1;
				}
				else
				{
					return 0;
				}
			}
		}

		public void Enqueue(T value)
		{
			if (Count == Capacity)
				throw new Exception("CircleQueue is Full.");

			_data[_tailIndex] = value;
			++_tailIndex;
			if (_tailIndex == Capacity)
				_tailIndex = 0;
		}

		public T Peek()
		{
			if (Count == 0) // m_HeadIndex == m_TailIndex
				throw new Exception("CircleQueue is Empty.");

			return _data[_headIndex];
		}

		public T Dequeue()
		{
			if (Count == 0) // m_HeadIndex == m_TailIndex
				throw new Exception("CircleQueue is Empty.");

			if (_headIndex < _tailIndex)
			{
				return _data[_headIndex++];
			}
			else
			{
				if (_headIndex >= Capacity)
				{
					_headIndex = 0;
					return _data[_headIndex++];
				}
				
				return _data[_headIndex++];
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}
	}
}