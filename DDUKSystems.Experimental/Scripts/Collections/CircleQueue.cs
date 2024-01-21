using System;
using System.Collections;
using System.Collections.Generic;


namespace DDUKSystems
{
    /// <summary>
    /// 원형 큐.
    /// </summary>
    public class CircleQueue<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
    {
        private class Enumerator : IEnumerator<T>
        {
            private T[] m_Data;
            ValueTuple<int, int, int, int> m_Range;
            private int m_Index;
            private T m_Current;

            public int HeadIndex => m_Range.Item1;
            public int TailIndex => m_Range.Item2;
            public int Count => m_Range.Item3;
            public int Capacity => m_Range.Item4;

            public T Current => m_Current;

            object IEnumerator.Current => m_Current;

            public Enumerator(CircleQueue<T> queue)
            {
                m_Data = queue.m_Data;
                m_Range = (queue.m_HeadIndex, queue.m_TailIndex, queue.Count, queue.Capacity);
                m_Index = HeadIndex;
                m_Current = default;
            }

            public void Dispose()
            {
                m_Data = null;
                m_Current = default;
            }

            public bool MoveNext()
            {
                if (Count == 0)
                    return false;

                m_Current = m_Data[m_Index];
                if (m_Index < TailIndex)
                {
                    ++m_Index;
                    return m_Index < TailIndex;
                }
                else
                {
                    if (m_Index >= Capacity)
                        m_Index = 0;
                    else
                        ++m_Index;

                    return m_Index < TailIndex;
                }
            }

            public void Reset()
            {
                m_Index = HeadIndex;
                m_Current = default;
            }
        }


        private T[] m_Data;
        private int m_HeadIndex;
        private int m_TailIndex;

        public T[] Data => m_Data;
        public T Front => m_Data[m_HeadIndex];
        public T Back => m_Data[m_TailIndex];
        public int Capacity => m_Data.Length;

        public int Count
        {
            get
            {
                if (m_HeadIndex < m_TailIndex)
                {
                    return m_TailIndex - m_HeadIndex + 1;
                }
                else if (m_HeadIndex > m_TailIndex)
                {
                    return m_TailIndex + (Capacity - 1 - m_HeadIndex) + 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public CircleQueue(int capacity)
        {
            m_Data = new T[capacity];
            m_HeadIndex = 0;
            m_TailIndex = 0;
        }

        public void Enqueue(T value)
        {
            if (Count == Capacity)
                throw new Exception("CircleQueue is Full.");

            m_Data[m_TailIndex] = value;
            ++m_TailIndex;
            if (m_TailIndex == Capacity)
                m_TailIndex = 0;
        }

        public T Peek()
        {
            if (Count == 0) // m_HeadIndex == m_TailIndex
                throw new Exception("CircleQueue is Empty.");

            return m_Data[m_HeadIndex];
        }

        public T Dequeue()
        {
            if (Count == 0) // m_HeadIndex == m_TailIndex
                throw new Exception("CircleQueue is Empty.");

            if (m_HeadIndex < m_TailIndex)
            {
                return m_Data[m_HeadIndex++];
            }
            else
            {
                if (m_HeadIndex >= Capacity)
                {
                    m_HeadIndex = 0;
                    return m_Data[m_HeadIndex++];
                }

                return m_Data[m_HeadIndex++];
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