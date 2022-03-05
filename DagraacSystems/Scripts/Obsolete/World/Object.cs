//using System;
//using System.Collections.Generic;
//using System.Linq;


//namespace DagraacSystems.World
//{
//	public class Object : IDisposable
//	{
//		public static Dictionary<Guid, Object> Objects { private set; get; } = new Dictionary<Guid, Object>();

//		private bool m_IsDisposed = false;

//		public string Name { set; get; } = string.Empty;
//		public Guid InstanceID { private set; get; } = Guid.Empty;

//		protected Object()
//		{
//			InstanceID = Guid.NewGuid();
//			Objects.Add(InstanceID, this);
//		}

//		~Object()
//		{
//			Dispose();
//		}

//		protected virtual void OnCreate(params object[] args) { }
//		protected virtual void OnDispose() { }
//		protected virtual void OnUpdate(float deltaTime) { }

//		public void Dispose()
//		{
//			if (!m_IsDisposed)
//			{
//				m_IsDisposed = true;
//				OnDispose();
//				Objects.Remove(InstanceID);
//				InstanceID = Guid.Empty;
//				GC.SuppressFinalize(this);
//			}
//		}

//		public static void Dispose(ref Object targetObject)
//		{
//			targetObject?.Dispose();
//			targetObject = null;
//		}

//		public static T Find<T>(Func<T, bool> predicate) where T : Object
//		{
//			return Object.Objects.Values.OfType<T>().Where(predicate).FirstOrDefault();
//		}

//		public static List<T> FindAll<T>(Func<T, bool> predicate) where T : Object
//		{
//			return Object.Objects.Values.OfType<T>().Where(predicate).ToList();
//		}

//		public static T Create<T>(params object[] args) where T : Object, new()
//		{
//			var targetObject = new T();
//			targetObject.OnCreate(args);
//			return targetObject;
//		}
//	}
//}
