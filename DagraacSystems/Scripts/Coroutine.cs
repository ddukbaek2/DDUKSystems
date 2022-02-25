using System;
using System.Collections;
using System.Collections.Generic;


namespace DagraacSystems
{
	public class Yield
	{
		public Yield(object target)
		{
		}

		public virtual bool Check()
		{
			return true;
		}
	}

	public class Coroutine
	{
		public enum Condition { Continue, Wait, Break, }

		private IEnumerator _enumerator;
		private Yield _yield;
		private Condition _condition;

		public void StartCoroutine(IEnumerator enumerator)
		{
			_enumerator = enumerator;
			_yield = null;
			_condition = Condition.Continue;
		}

		public void StopCoroutine()
		{
			_enumerator = null;
			_yield = null;
			_condition = Condition.Break;
		}


		public void FrameMove(float deltaTime)
		{
			switch (_condition)
			{
				case Condition.Continue:
					{
						_condition = Processing();
						break;
					}

				case Condition.Wait:
					{
						if (_yield.Check())
							_condition = Condition.Continue;
						break;
					}

				case Condition.Break:
					{
						StopCoroutine();
						break;
					}
			}
		}

		private Condition Processing()
		{
			if (_enumerator.MoveNext())
			{
				_yield = (Yield)_enumerator.Current;
				return Condition.Wait;
			}

			return Condition.Break;
		}
	}
	
	public class CoroutineProcess
	{
		Coroutine _coroutine;

		public void Foo()
		{
			_coroutine = new Coroutine();
			_coroutine.StartCoroutine(Process());
		}

		public IEnumerator Process()
		{
			yield break;
		}
	}
}
