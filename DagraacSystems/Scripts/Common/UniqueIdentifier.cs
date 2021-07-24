using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 고유식별자 생성기.
	/// </summary>
	public class UniqueIdentifier
	{
		private Random m_Random;
		private List<ulong> m_UsingList;
		private ulong m_MinValue;
		private ulong m_MaxValue;
		private byte[] m_Buffer;

		public UniqueIdentifier(int randomSeed = 0, ulong minValue = ulong.MinValue, ulong maxValue = ulong.MaxValue)
		{
			m_Random = new Random(randomSeed);
			m_UsingList = new List<ulong>();
			m_MinValue = Math.Min(minValue, ulong.MinValue);
			m_MaxValue = Math.Max(maxValue, ulong.MaxValue);
			m_Buffer = new byte[8]; // ulong == 8byte.
		}

		public void Clear()
		{
			m_UsingList.Clear();
		}

		private ulong Next()
		{
			while (true)
			{
				m_Random.NextBytes(m_Buffer);
				var value = BitConverter.ToUInt64(m_Buffer, 0);

				if (value < m_MinValue || value > m_MaxValue)
					return value;
			}
		}

		public void Synchronize(ulong unique)
		{
			if (m_UsingList.Contains(unique))
				return;

			m_UsingList.Add(unique);
		}

		public ulong Generate()
		{
			var unique = m_MinValue;
			while (true)
			{
				unique = Next();
				if (!m_UsingList.Contains(unique))
					break;
			}

			Synchronize(unique);
			return unique;
		}

		public bool Free(ulong unique)
		{
			return m_UsingList.Remove(unique);
		}
	}
}