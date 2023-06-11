using System; // Type, Exception
using System.Reflection; // BindingFlags


namespace DagraacSystems
{
	/// <summary>
	/// 리플렉션 기능.
	/// </summary>
	public static class Reflection
	{
		/// <summary>
		/// 대상 참조타입을 통해 공개되지 않은 일반 함수를 호출.
		/// </summary>
		public static object InvokeByReferenceType(object target, Type targettype, string methodname, params object[] parameters)
		{
			if (target == null)
				return null;

			var bindingFlags =  BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

			try
			{
				return targettype.InvokeMember(methodname, bindingFlags, Type.DefaultBinder, target, parameters);
			}
			catch (Exception e)
			{
				//Debug.LogError(e.ToString());
				//Debug.LogException(e);
			}

			return null;
		}

		/// <summary>
		/// 대상 참조타입을 통해 공개되지 않은 일반 함수를 호출.
		/// </summary>
		public static TReturnType InvokeByReferenceType<TReferenceType, TReturnType>(TReferenceType target, string methodname, params object[] parameters) where TReferenceType : class
		{
			if (target == null)
				return default;

			var returnValue = InvokeByReferenceType(target, typeof(TReferenceType), methodname, parameters);
			if (returnValue == null)
				return default;

			return (TReturnType)returnValue;
		}

		/// <summary>
		/// 대상 참조타입을 통해 공개되지 않은 일반 함수를 호출.
		/// </summary>
		public static void InvokeByReferenceType<TReferenceType>(TReferenceType target, string methodname, params object[] parameters) where TReferenceType : class
		{
			if (target == null)
				return;

			InvokeByReferenceType(target, typeof(TReferenceType), methodname, parameters);
		}

		/// <summary>
		/// 대상 값타입을 통해 공개되지 않은 일반 함수를 호출.
		/// </summary>
		public static TReturnType InvokeByValueType<TValueType, TReturnType>(TValueType target, string methodname, params object[] parameters) //where TValueType : notnull
		{
			var bindingFlags =  BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
			var targetType = target.GetType();

			try
			{
				var returnValue = targetType.InvokeMember(methodname, bindingFlags, Type.DefaultBinder, target, parameters);
				if (returnValue == null)
					return default;

				return (TReturnType)returnValue;
			}
			catch (Exception e)
			{
				//Debug.LogError(e.ToString());
				//Debug.LogException(e);
			}

			return default;
		}

		/// <summary>
		/// 대상 값타입을 통해 공개되지 않은 일반 함수를 호출.
		/// </summary>
		public static void InvokeByValueType<TValueType>(TValueType target, string methodname, params object[] parameters) //where TValueType : notnull
		{
			var bindingFlags =  BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
			var targetType = target.GetType();

			try
			{
				targetType.InvokeMember(methodname, bindingFlags, Type.DefaultBinder, target, parameters);
			}
			catch (Exception e)
			{
				//Debug.LogError(e.ToString());
				//Debug.LogException(e);
			}
		}

		/// <summary>
		/// 대상 클래스를 통해 공개되지 않은 스태틱 함수를 호출.
		/// </summary>
		public static object InvokeByStatic(Type targettype, string methodname, params object[] parameters)
		{
			var bindingFlags =  BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

			try
			{
				return targettype.InvokeMember(methodname, bindingFlags, Type.DefaultBinder, null, parameters);
			}
			catch (Exception e)
			{
				//Debug.LogError(e.ToString());
				//Debug.LogException(e);
			}

			return null;
		}

		/// <summary>
		/// 대상 클래스를 통해 공개되지 않은 스태틱 함수를 호출.
		/// </summary>
		public static object InvokeStatic<T>(string methodname, params object[] parameters)
		{
			return InvokeByStatic(typeof(T), methodname, parameters);
		}
	}
}