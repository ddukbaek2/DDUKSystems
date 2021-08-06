using System;
using System.Reflection;


namespace DagraacSystems
{
	/// <summary>
	/// 리플렉션 베이스의 대산 오브젝트의 함수 호출.
	/// </summary>
	public static class Message
	{
		/// <summary>
		/// 객체의 스태틱한 함수를 호출.
		/// </summary>
		public static object SendToStatic(object target, string name, params object[] parameters)
		{
			var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			try
			{
				var type = target.GetType();

				// 인자가 있을 경우.
				if (parameters != null && parameters.Length > 0)
					return type.InvokeMember(name, bindingFlags, Type.DefaultBinder, target, parameters);
				else
					return type.InvokeMember(name, bindingFlags, Type.DefaultBinder, target, new object[0]);
			}
			catch (Exception exception)
			{
				return exception;
			}
		}

		/// <summary>
		/// 객체의 일반 함수를 호출.
		/// 상속을 포함한 어떤 함수든 호출.
		/// </summary>
		public static object SendTo(object target, string name, params object[] parameters)
		{
			var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
			try
			{
				var type = target.GetType();

				// 인자가 있을 경우.
				if (parameters != null && parameters.Length > 0)
					return type.InvokeMember(name, bindingFlags, Type.DefaultBinder, target, parameters);
				else
					return type.InvokeMember(name, bindingFlags, Type.DefaultBinder, target, new object[0]);
			}
			catch (Exception exception)
			{
				return exception;
			}
		}
	}
}