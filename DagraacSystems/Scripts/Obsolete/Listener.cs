//using System;
//using System.Collections.Generic;
//using System.Text;


//namespace DagraacSystems.React
//{
//	public class Detector<T> : IObserver<T>
//	{
//		private List<Listener<T>> m_Listeners;

//		public Detector()
//		{
//			m_Listeners = new List<Listener<T>>();
//		}

//		void IObserver<T>.OnCompleted()
//		{
//		}

//		void IObserver<T>.OnError(Exception error)
//		{
//		}

//		void IObserver<T>.OnNext(T value)
//		{
//			foreach (var listener in m_Listeners)
//				listener.Subscribe(this);
//		}

//		public TListener CreateListener<TListener>() where TListener : Listener<T>, new()
//		{
//			var listener = new TListener();
//			listener.Subscribe(this);
//			m_Listeners.Add(listener);
//			return listener;
//		}
//	}


//	public class Listener<T> : IObservable<T>
//	{
//		internal class Disposable : IDisposable
//		{
//			Listener<T> m_Listener;

//			public Disposable(Listener<T> listener)
//			{
//				m_Listener = listener;
//			}

//			void IDisposable.Dispose()
//			{
//				// 해제.
//				m_Listener.OnDispose();
//			}
//		}

//		public Listener()
//		{
//		}

//		protected virtual void OnDispose()
//		{
//		}

//		/// <summary>
//		/// 이벤트를 쏴줌.
//		/// </summary>
//		public IDisposable Subscribe(IObserver<T> observer)
//		{
//			return new Disposable(this);
//		}

//	}
//}
