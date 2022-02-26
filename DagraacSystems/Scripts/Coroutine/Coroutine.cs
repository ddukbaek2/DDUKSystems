using System.Collections;


namespace DagraacSystems
{
	/// <summary>
	/// 코루틴.
	/// </summary>
	public class Coroutine
	{
		public enum Condition { Continue, Wait, Break, }

		private IEnumerator _enumerator;
		private IYield _yield;
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

		public void Tick(float tick)
		{
			switch (_condition)
			{
				case Condition.Continue:
					{
						_condition = Continue();
						if (_condition == Condition.Wait)
						{
							_yield.OnBegin();
						}

						break;
					}

				case Condition.Wait:
					{
						if (_yield.OnStay(tick))
						{
							_yield.OnEnd();
							_condition = Condition.Continue;
						}

						break;
					}

				case Condition.Break:
					{
						StopCoroutine();
						break;
					}
			}
		}

		private Condition Continue()
		{
			if (_enumerator.MoveNext())
			{
				_yield = (IYield)_enumerator.Current;
				return Condition.Wait;
			}

			return Condition.Break;
		}
	}
}