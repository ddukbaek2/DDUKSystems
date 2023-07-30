using System; // Block


namespace DagraacSystems
{
    /// <summary>
    /// 바이트 버퍼.
    /// </summary>
    public class ByteBuffer : DisposableObject
    {
        private byte[] m_Buffer;

        public int Capacity => m_Buffer.Length;

        /// <summary>
        /// 인덱서.
        /// </summary>
        public byte this[int index]
        {
            get => m_Buffer[index];
			set => m_Buffer[index] = value;
		}

        /// <summary>
        /// 생성됨.
        /// </summary>
        public ByteBuffer(int capacity = 4096)
        {
            m_Buffer = new byte[capacity];
        }

        /// <summary>
        /// 해제됨.
        /// </summary>
        protected override void OnDispose(bool explicitedDispose)
        {
            m_Buffer = null;

            base.OnDispose(explicitedDispose);
        }

        /// <summary>
        /// 해제.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            Dispose(this);
        }

        /// <summary>
        /// 비우기.
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < m_Buffer.Length; ++i)
                m_Buffer[i] = 0x00;
        }

        public void Set(int index, byte value)
        {
            m_Buffer[index] = value;
        }

        public byte Get(int index)
        {
            return m_Buffer[index];
        }

        public byte[] Copy(int offset, int length)
        {
            var copy = new byte[length];
            Buffer.BlockCopy(m_Buffer, offset, copy, 0, length);
            return copy;
        }
    }
}