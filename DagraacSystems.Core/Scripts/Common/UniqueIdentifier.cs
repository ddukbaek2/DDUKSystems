using System; // Random, Math, BitConverter
using System.Collections.Generic; // List


namespace DagraacSystems
{
    /// <summary>
    /// ulong 고유식별자 생성기.
    /// </summary>
    public class UniqueIdentifier : DisposableObject
    {
        private Random m_Random;
        private List<ulong> m_UsingList;
        private ulong m_MinValue;
        private ulong m_MaxValue;
        private byte[] m_Buffer;

        /// <summary>
        /// 생성됨.
        /// </summary>
        public UniqueIdentifier(int randomSeed = 0, ulong minValue = ulong.MinValue, ulong maxValue = ulong.MaxValue) : base()
        {
            m_Random = new Random(randomSeed);
            m_UsingList = new List<ulong>();
            m_MinValue = Math.Min(minValue, ulong.MinValue);
            m_MaxValue = Math.Max(maxValue, ulong.MaxValue);
            m_Buffer = new byte[sizeof(ulong)]; // ulong == 8byte.
        }

        /// <summary>
        /// 파괴됨.
        /// </summary>
        protected override void OnDispose(bool explicitedDispose)
        {
            m_Random = null;
            m_UsingList = null;
            m_Buffer = null;

            base.OnDispose(explicitedDispose);
        }

        /// <summary>
        /// 파괴.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            Dispose(this);
        }

        /// <summary>
        /// 초기화.
        /// </summary>
        public void Clear()
        {
            m_UsingList.Clear();
        }

        /// <summary>
        /// 다음 랜덤 정수.
        /// </summary>
        private ulong NextInternal()
        {
            while (true)
            {
                m_Random.NextBytes(m_Buffer);
                var value = BitConverter.ToUInt64(m_Buffer, 0);

                if (value < m_MinValue || value > m_MaxValue)
                    continue;

                return value;
            }
        }

        /// <summary>
        /// 동기화.
        /// </summary>
        public void Synchronize(ulong unique)
        {
            if (m_UsingList.Contains(unique))
                return;

            m_UsingList.Add(unique);
        }

        /// <summary>
        /// 생성.
        /// </summary>
        public ulong Generate()
        {
            var unique = m_MinValue;
            while (true)
            {
                unique = NextInternal();
                if (!m_UsingList.Contains(unique))
                    break;
            }

            Synchronize(unique);
            return unique;
        }

        /// <summary>
        /// 제거.
        /// </summary>
        public bool Free(ulong unique)
        {
            return m_UsingList.Remove(unique);
        }

        /// <summary>
        /// 포함 여부.
        /// </summary>
        public bool Contains(ulong unique)
        {
            return m_UsingList.Contains(unique);
        }
    }
}