using System; // Block
using DagraacSystems.Core.Scripts.Common;

namespace DagraacSystems.Scripts.Collections
{
    /// <summary>
    /// 바이트 버퍼.
    /// </summary>
    public class ByteBuffer : DisposableObject
    {
        private byte[] m_Buffer;

        public int Capacity => m_Buffer.Length;

        public byte this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        /// <summary>
        /// 생성됨.
        /// </summary>
        public ByteBuffer(int _capacity = 4096)
        {
            m_Buffer = new byte[_capacity];
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

        public byte[] Copy(int offset, int size)
        {
            var copy = new byte[size];
            Buffer.BlockCopy(m_Buffer, offset, copy, 0, size);
            return copy;
        }
    }
}