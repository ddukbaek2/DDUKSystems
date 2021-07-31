using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	public struct Parameters
	{
		private List<object> m_Args;

		public Parameters(params object[] args)
		{
			m_Args = new List<object>();
			if (args != null && args.Length > 0)
				m_Args.AddRange(args);
		}

		public void Clear()
		{
			Clear();
		}

		public void Push<T>(T value)
		{
			m_Args.Add(value);
		}

		public T Pop<T>()
		{
			var value = (T)m_Args[0];
			m_Args.RemoveAt(0);
			return value;
		}

		public void Set<T1>(T1 t1)
		{
			Clear();
			m_Args.Add(t1);
		}

		public void Set<T1, T2>(T1 t1, T2 t2)
		{
			Clear();
			m_Args.Add(t1);
			m_Args.Add(t2);
		}

		public void Set<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
		{
			Clear();
			m_Args.Add(t1);
			m_Args.Add(t2);
			m_Args.Add(t3);
		}

		public void Set<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
		{
			Clear();
			m_Args.Add(t1);
			m_Args.Add(t2);
			m_Args.Add(t3);
			m_Args.Add(t4);
		}

		public void Set<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
		{
			Clear();
			m_Args.Add(t1);
			m_Args.Add(t2);
			m_Args.Add(t3);
			m_Args.Add(t4);
			m_Args.Add(t5);
		}

		public void Set<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
		{
			Clear();
			m_Args.Add(t1);
			m_Args.Add(t2);
			m_Args.Add(t3);
			m_Args.Add(t4);
			m_Args.Add(t5);
			m_Args.Add(t6);
		}

		public void Set<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
		{
			Clear();
			m_Args.Add(t1);
			m_Args.Add(t2);
			m_Args.Add(t3);
			m_Args.Add(t4);
			m_Args.Add(t5);
			m_Args.Add(t6);
			m_Args.Add(t7);
		}

		public void Set<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
		{
			Clear();
			m_Args.Add(t1);
			m_Args.Add(t2);
			m_Args.Add(t3);
			m_Args.Add(t4);
			m_Args.Add(t5);
			m_Args.Add(t6);
			m_Args.Add(t7);
			m_Args.Add(t8);
		}

		public T Get<T>(int index)
		{
			return (T)m_Args[index];
		}

		public Tuple<T1, T2> Get<T1, T2>(int index1 = 0, int index2 = 1)
		{
			return new Tuple<T1, T2>(Get<T1>(index1), Get<T2>(index2));
		}

		public Tuple<T1, T2, T3> Get<T1, T2, T3>(int index1 = 0, int index2 = 1, int index3 = 2)
		{
			return new Tuple<T1, T2, T3>(Get<T1>(index1), Get<T2>(index2), Get<T3>(index3));
		}

		public Tuple<T1, T2, T3, T4> Get<T1, T2, T3, T4>(int index1 = 0, int index2 = 1, int index3 = 2, int index4 = 3)
		{
			return new Tuple<T1, T2, T3, T4>(Get<T1>(index1), Get<T2>(index2), Get<T3>(index3), Get<T4>(index4));
		}

		public Tuple<T1, T2, T3, T4, T5> Get<T1, T2, T3, T4, T5>(int index1 = 0, int index2 = 1, int index3 = 2, int index4 = 3, int index5 = 4)
		{
			return new Tuple<T1, T2, T3, T4, T5>(Get<T1>(index1), Get<T2>(index2), Get<T3>(index3), Get<T4>(index4), Get<T5>(index5));
		}

		public Tuple<T1, T2, T3, T4, T5, T6> Get<T1, T2, T3, T4, T5, T6>(int index1 = 0, int index2 = 1, int index3 = 2, int index4 = 3, int index5 = 4, int index6 = 5)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6>(Get<T1>(index1), Get<T2>(index2), Get<T3>(index3), Get<T4>(index4), Get<T5>(index5), Get<T6>(index6));
		}

		public Tuple<T1, T2, T3, T4, T5, T6, T7> Get<T1, T2, T3, T4, T5, T6, T7>(int index1 = 0, int index2 = 1, int index3 = 2, int index4 = 3, int index5 = 4, int index6 = 5, int index7 = 6)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7>(Get<T1>(index1), Get<T2>(index2), Get<T3>(index3), Get<T4>(index4), Get<T5>(index5), Get<T6>(index6), Get<T7>(index7));
		}

		public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Get<T1, T2, T3, T4, T5, T6, T7, T8>()
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(Get<T1>(0), Get<T2>(1), Get<T3>(2), Get<T4>(3), Get<T5>(4), Get<T6>(5), Get<T7>(6), Get<T8>(7));
		}

		public static Parameters Create(params object[] args)
		{
			var combinedParameters = new Parameters(args);
			return combinedParameters;
		}

		//public static Parameters Create<T1>(T1 t1)
		//{
		//	var combinedParameters = new Parameters();
		//	combinedParameters.Set(t1);
		//	return combinedParameters;
		//}

		//public static Parameters Create<T1, T2>(T1 t1, T2 t2)
		//{
		//	var combinedParameters = new Parameters();
		//	combinedParameters.Set(t1, t2);
		//	return combinedParameters;
		//}

		//public static Parameters Create<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
		//{
		//	var combinedParameters = new Parameters();
		//	combinedParameters.Set(t1, t2, t3);
		//	return combinedParameters;
		//}

		//public static Parameters Create<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
		//{
		//	var combinedParameters = new Parameters();
		//	combinedParameters.Set(t1, t2, t3, t4);
		//	return combinedParameters;
		//}

		//public static Parameters Create<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
		//{
		//	var combinedParameters = new Parameters();
		//	combinedParameters.Set(t1, t2, t3, t4, t5);
		//	return combinedParameters;
		//}

		//public static Parameters Create<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
		//{
		//	var combinedParameters = new Parameters();
		//	combinedParameters.Set(t1, t2, t3, t4, t5, t6);
		//	return combinedParameters;
		//}

		//public static Parameters Create<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
		//{
		//	var combinedParameters = new Parameters();
		//	combinedParameters.Set(t1, t2, t3, t4, t5, t6, t7);
		//	return combinedParameters;
		//}

		//public static Parameters Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
		//{
		//	var combinedParameters = new Parameters();
		//	combinedParameters.Set(t1, t2, t3, t4, t5, t6, t7, t8);
		//	return combinedParameters;
		//}
	}
}