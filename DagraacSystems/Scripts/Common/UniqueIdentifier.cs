using System; // Random, Math, BitConverter
using System.Collections.Generic; // List


namespace DagraacSystems
{
	/// <summary>
	/// ulong 고유식별자 생성기.
	/// </summary>
	public class UniqueIdentifier : DisposableObject
	{
		private Random _random;
		private List<ulong> _usingList;
		private ulong _minValue;
		private ulong _maxValue;
		private byte[] _buffer;

		public UniqueIdentifier(int randomSeed = 0, ulong minValue = ulong.MinValue, ulong maxValue = ulong.MaxValue) : base()
		{
			_random = new Random(randomSeed);
			_usingList = new List<ulong>();
			_minValue = Math.Min(minValue, ulong.MinValue);
			_maxValue = Math.Max(maxValue, ulong.MaxValue);
			_buffer = new byte[sizeof(ulong)]; // ulong == 8byte.
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			_random = null;
			_usingList = null;
			_buffer = null;

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 파괴.
		/// </summary>
		public virtual void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}


		public void Clear()
		{
			_usingList.Clear();
		}

		private ulong Next()
		{
			while (true)
			{
				_random.NextBytes(_buffer);
				var value = BitConverter.ToUInt64(_buffer, 0);

				if (value < _minValue || value > _maxValue)
					continue;

				return value;
			}
		}

		public void Synchronize(ulong unique)
		{
			if (_usingList.Contains(unique))
				return;

			_usingList.Add(unique);
		}

		public ulong Generate()
		{
			var unique = _minValue;
			while (true)
			{
				unique = Next();
				if (!_usingList.Contains(unique))
					break;
			}

			Synchronize(unique);
			return unique;
		}

		public bool Free(ulong unique)
		{
			return _usingList.Remove(unique);
		}

		public bool Contains(ulong unique)
		{
			return _usingList.Contains(unique);
		}
	}
}