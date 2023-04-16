using System;
using System.Reflection; 


namespace DagraacSystems
{
	/// <summary>
	/// 리플렉션 유틸리티.
	/// </summary>
	public static class ReflectionUtility
	{
		/// <summary>
		/// 대상 참조타입을 통해 공개되지 않은 일반 함수를 호출.
		/// </summary>
		public static object InvokeByReferenceType(object _target, Type _targettype, string _methodname, params object[] _parameters)
		{
			if (_target == null)
				return null;

			var bindingFlags =  BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
			var targetType = _target.GetType();

			try
			{
				return targetType.InvokeMember(_methodname, bindingFlags, System.Type.DefaultBinder, _target, _parameters);
			}
			catch (System.Exception e)
			{
				//Debug.LogError(e.ToString());
				//Debug.LogException(e);
			}

			return null;
		}

		/// <summary>
		/// 대상 참조타입을 통해 공개되지 않은 일반 함수를 호출.
		/// </summary>
		public static TReturnType InvokeByReferenceType<TReferenceType, TReturnType>(TReferenceType _target, string _methodname, params object[] _parameters) where TReferenceType : class
		{
			if (_target == null)
				return default;

			var returnValue = InvokeByReferenceType(_target, typeof(TReferenceType), _methodname, _parameters);
			if (returnValue == null)
				return default;

			return (TReturnType)returnValue;
		}

		/// <summary>
		/// 대상 참조타입을 통해 공개되지 않은 일반 함수를 호출.
		/// </summary>
		public static void InvokeByReferenceType<TReferenceType>(TReferenceType _target, string _methodname, params object[] _parameters) where TReferenceType : class
		{
			if (_target == null)
				return;

			InvokeByReferenceType(_target, typeof(TReferenceType), _methodname, _parameters);
		}

		/// <summary>
		/// 대상 값타입을 통해 공개되지 않은 일반 함수를 호출.
		/// </summary>
		public static TReturnType InvokeByValueType<TValueType, TReturnType>(TValueType _target, string _methodname, params object[] _parameters) //where TValueType : notnull
		{
			var bindingFlags =  BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
			var targetType = _target.GetType();

			try
			{
				var returnValue = targetType.InvokeMember(_methodname, bindingFlags, System.Type.DefaultBinder, _target, _parameters);
				if (returnValue == null)
					return default;

				return (TReturnType)returnValue;
			}
			catch (System.Exception e)
			{
				//Debug.LogError(e.ToString());
				//Debug.LogException(e);
			}

			return default;
		}

		/// <summary>
		/// 대상 값타입을 통해 공개되지 않은 일반 함수를 호출.
		/// </summary>
		public static void InvokeByValueType<TValueType>(TValueType _target, string _methodname, params object[] _parameters) //where TValueType : notnull
		{
			var bindingFlags =  BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
			var targetType = _target.GetType();

			try
			{
				targetType.InvokeMember(_methodname, bindingFlags, System.Type.DefaultBinder, _target, _parameters);
			}
			catch (System.Exception e)
			{
				//Debug.LogError(e.ToString());
				//Debug.LogException(e);
			}
		}

		/// <summary>
		/// 대상 클래스를 통해 공개되지 않은 스태틱 함수를 호출.
		/// </summary>
		public static object InvokeByStatic(Type _targettype, string _methodname, params object[] _parameters)
		{
			var bindingFlags =  BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

			try
			{
				return _targettype.InvokeMember(_methodname, bindingFlags, System.Type.DefaultBinder, null, _parameters);
			}
			catch (System.Exception e)
			{
				//Debug.LogError(e.ToString());
				//Debug.LogException(e);
			}

			return null;
		}

		/// <summary>
		/// 대상 클래스를 통해 공개되지 않은 스태틱 함수를 호출.
		/// </summary>
		public static object InvokeStatic<T>(string _methodname, params object[] _parameters)
		{
			return InvokeByStatic(typeof(T), _methodname, _parameters);
		}
	}
}