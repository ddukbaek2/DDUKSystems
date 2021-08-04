using System;


namespace DagraacSystems.Obsolete
{
	public class Sequence<TTarget>
	{
		public TTarget Value { private set; get; }

		public Sequence<TTarget> If(Predicate<TTarget> callback)
		{
			return this;
		}

		public Sequence<TTarget> Elif(Predicate<TTarget> callback)
		{
			return this;
		}

		public Sequence<TTarget> Else(Action<TTarget> callback)
		{
			return this;
		}

		public Sequence<TTarget> While(Func<TTarget, bool> callback)
		{
			return this;
		}

		public Sequence<TTarget> Do()
		{
			return this;
		}

		public Sequence<TTarget> Build()
		{
			return this;
		}

		public Sequence<TTarget> Wait()
		{
			return this;
		}

		public static Sequence<T> Create<T>(T value)
		{
			var sequence = new Sequence<T>();
			sequence.Value = value;
			return sequence;
		}

		public static void Example()
		{
			Create(5).While(null).Wait().Wait().Build();

		}
	}
}
