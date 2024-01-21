using System;
using System.Collections;
using System.Collections.Generic;


namespace DDUKSystems
{
	/// <summary>
	/// 컬렉션.
	/// 리스트<T> 구현체.
	/// </summary>
	public class Collection<T> : DisposableObject,
		ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>,
		IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList
	{
		/// <summary>
		/// 열거자.
		/// </summary>
		public class Enumerator : DisposableObject, IEnumerator, IEnumerator<T>
		{
			private List<T> m_Data;
			private int m_Index;

			/// <summary>
			/// 현재 요소.
			/// </summary>
			public T Current => m_Data[m_Index];

			/// <summary>
			/// 현재 요소.
			/// </summary>
			object IEnumerator.Current => m_Data[m_Index];

			/// <summary>
			/// 생성됨.
			/// </summary>
			public Enumerator(IEnumerable<T> collection)
			{
				m_Data = new List<T>();
				m_Data.AddRange(collection);

				m_Index = -1;
			}

			/// <summary>
			/// 해제됨.
			/// </summary>
			protected override void OnDispose(bool explicitedDispose)
			{
				m_Data = null;

				base.OnDispose(explicitedDispose);
			}

			/// <summary>
			/// 해제.
			/// </summary>
			public void Dispose()
			{
				if (IsDisposed)
					return;

				DisposableObject.Dispose(this);
			}

			/// <summary>
			/// 초기화.
			/// 해제 후에는 사용할 수 없음.
			/// </summary>
			public void Reset()
			{
				m_Index = -1;
			}

			/// <summary>
			/// 다음 요소로 이동.
			/// </summary>
			public bool MoveNext()
			{
				++m_Index;
				if (m_Index < m_Data.Count)
					return true;
				return false;
			}
		}

		/// <summary>
		/// 실제 데이터.
		/// </summary>
		private List<T> m_Data;

		/// <summary>
		/// 실제 데이터.
		/// </summary>
		public List<T> Data => m_Data;

		/// <summary>
		/// 컬렉션 안의 요소 갯수.
		/// </summary>
		public int Count => m_Data.Count;

		/// <summary>
		/// 컬렉션이 읽기 전용인지 여부 (List.IsReadOnly == false).
		/// </summary>
		public bool IsReadOnly => ((ICollection<T>)m_Data).IsReadOnly;

		/// <summary>
		/// 쓰레드 안전성 여부 (List.IsSynchronized == false).
		/// </summary>
		public bool IsSynchronized => ((ICollection)m_Data).IsSynchronized;

		/// <summary>
		/// 쓰레드 동기화를 위한 오브젝트 (List.SyncRoot == this).
		/// </summary>
		public object SyncRoot => ((ICollection)m_Data).SyncRoot;

		/// <summary>
		/// 컬렉션이 고정 크기를 가졌는지 여부 (List.IsFixedSize == false).
		/// </summary>
		public bool IsFixedSize => ((IList)m_Data).IsFixedSize;

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		object IList.this[int index] { get => ((IList)m_Data)[index]; set => ((IList)m_Data)[index] = value; }

		/// <summary>
		/// 배열접근을 위한 인덱서.
		/// </summary>
		public T this[int index] { get => ((IList<T>)m_Data)[index]; set => ((IList<T>)m_Data)[index] = value; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Collection() : base()
		{
			m_Data = new List<T>();
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Collection(IEnumerable<T> collection) : base()
		{
			m_Data = new List<T>();
			m_Data.AddRange(collection);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		/// <summary>
		/// 열거자 생성.
		/// </summary>
		public IEnumerator<T> GetEnumerator()
		{
			return new Enumerator(m_Data);
		}

		/// <summary>
		/// 요소로 인덱스 찾기.
		/// </summary>
		public int IndexOf(T item)
		{
			return m_Data.IndexOf(item);
		}

		/// <summary>
		/// 요소 삽입.
		/// </summary>
		public void Insert(int index, T item)
		{
			m_Data.Insert(index, item);
		}

		/// <summary>
		/// 요소 제거.
		/// </summary>
		public void RemoveAt(int index)
		{
			m_Data.RemoveAt(index);
		}

		/// <summary>
		/// 요소 추가.
		/// </summary>
		public void Add(T item)
		{
			m_Data.Add(item);
		}

		/// <summary>
		/// 요소 제거.
		/// </summary>
		public bool Remove(T item)
		{
			return m_Data.Remove(item);
		}

		/// <summary>
		/// 요소 전체 비우기.
		/// </summary>
		public void Clear()
		{
			m_Data.Clear();
		}

		/// <summary>
		/// 요소가 포함되어있는지 여부.
		/// </summary>
		public bool Contains(T item)
		{
			return m_Data.Contains(item);
		}

		/// <summary>
		/// 현재 컨테이너가 들고 있는 요소들을 arrayIndex위치부터 array에 복사.
		/// </summary>
		public void CopyTo(T[] array, int arrayIndex)
		{
			m_Data.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		void ICollection.CopyTo(Array array, int index)
		{
			var data = array as T[];
			if (data == null)
				return;

			m_Data.CopyTo(data, index);
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		int IList.Add(object value)
		{
			var data = (T)value;
			if (data.Equals(default))
				return -1;

			m_Data.Add(data);
			return m_Data.Count - 1;
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		bool IList.Contains(object value)
		{
			var data = (T)value;
			if (data.Equals(default))
				return false;

			return m_Data.Contains(data);
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		int IList.IndexOf(object value)
		{
			var data = (T)value;
			if (data.Equals(default))
				return -1;

			return m_Data.IndexOf(data);
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		void IList.Insert(int index, object value)
		{
			var data = (T)value;
			if (data.Equals(default))
				return;

			m_Data.Insert(index, data);
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		void IList.Remove(object value)
		{
			var data = (T)value;
			if (data.Equals(default))
				return;

			m_Data.Remove(data);
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(m_Data);
		}
	}
}