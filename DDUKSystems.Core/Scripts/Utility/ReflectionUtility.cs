using System; // Type, Exception
using System.Collections.Generic; // List
using System.Reflection; // BindingFlags


namespace DDUKSystems
{
    /// <summary>
    /// 리플렉션 기능.
    /// </summary>
    public static class ReflectionUtility
    {
        /// <summary>
        /// 클래스의 지정타입에 대한 변수명 목록을 얻어옴.
        /// Public일 경우 그냥 목록에 포함되고, NonPublic일 경우 어트리뷰트타입을 보유한 대상만 목록에 포함됨.
        /// (변수명, 배열여부).
        /// </summary>
        public static List<string> FindFieldNamesByType(object _target, Type _targettype, Type _findtype, Type _attributetype = null)
        {
            var result = new List<string>();
            if (_target == null)
                return result;

            var fieldinfos = _targettype.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            foreach (var fieldinfo in fieldinfos)
            {
                if (fieldinfo.FieldType != _findtype)
                    continue;

                if (fieldinfo.IsPublic)
                {
                    result.Add(fieldinfo.Name);
                }
                else if (_attributetype != null)
                {
                    var attributetypes = Attribute.GetCustomAttributes(_attributetype);
                    if (attributetypes.Length == 0)
                        continue;

                    result.Add(fieldinfo.Name);
                }
            }

            return result;
        }

        /// <summary>
        /// 대상 참조타입을 통해 공개되지 않은 일반 변수의 값을 반환.
        /// </summary>
        public static object GetFieldValue(object _target, Type _targettype, string _fieldname)
        {
            if (_target == null)
                return null;

            var fieldinfos = _targettype.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            foreach (var fieldinfo in fieldinfos)
            {
                if (fieldinfo.Name != _fieldname)
                    continue;

                return fieldinfo.GetValue(_target);
            }

            return null;
        }

        /// <summary>
        /// 대상 참조타입을 통해 공개되지 않은 클래스 변수의 값을 반환.
        /// </summary>
        public static TValueType GetFieldValue<TClassType, TValueType>(object _target, string _fieldname)
        {
            var returnValue = GetFieldValue(_target, typeof(TClassType), _fieldname);
            if (returnValue == null)
                return default;

            return (TValueType)returnValue;
        }

        /// <summary>
        /// 대상 참조타입을 통해 공개되지 않은 일반 변수의 값을 설정.
        /// </summary>
        public static void SetFieldValue(object _target, Type _targettype, string _fieldname, object _value)
        {
            if (_target == null)
                return;

            var fieldinfos = _targettype.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            foreach (var fieldinfo in fieldinfos)
            {
                if (fieldinfo.Name != _fieldname)
                    continue;

                fieldinfo.SetValue(_target, _value);
                return;
            }
        }

        /// <summary>
        /// 대상 참조타입을 통해 공개되지 않은 일반 변수의 값을 설정.
        /// </summary>
        public static void SetFieldValue<TValueType>(object _target, Type _targettype, string _fieldname, TValueType _values)
        {
            SetFieldValue(_target, _targettype, _fieldname, (object)_values);
        }

        /// <summary>
        /// 대상 참조타입을 통해 공개되지 않은 클래스 변수의 값을 설정.
        /// </summary>
        public static void SetFieldValue<TClassType, TValueType>(object _target, string _fieldname, TValueType _value)
        {
            SetFieldValue(_target, typeof(TClassType), _fieldname, _value);
        }

        /// <summary>
        /// 대상 참조타입을 통해 공개되지 않은 일반 함수를 호출.
        /// </summary>
        public static object InvokeMethodByReferenceType(object target, Type targettype, string methodname, params object[] parameters)
        {
            if (target == null)
                return null;

            var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

            try
            {
                return targettype.InvokeMember(methodname, bindingFlags, Type.DefaultBinder, target, parameters);
            }
            catch// (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 대상 참조타입을 통해 공개되지 않은 일반 함수를 호출.
        /// </summary>
        public static TReturnType InvokeMethodByReferenceType<TReferenceType, TReturnType>(TReferenceType target, string methodname, params object[] parameters) where TReferenceType : class
        {
            if (target == null)
                return default;

            var returnValue = InvokeMethodByReferenceType(target, typeof(TReferenceType), methodname, parameters);
            if (returnValue == null)
                return default;

            return (TReturnType)returnValue;
        }

        /// <summary>
        /// 대상 참조타입을 통해 공개되지 않은 일반 함수를 호출.
        /// </summary>
        public static object InvokeMethodByReferenceType<TReferenceType>(TReferenceType target, string methodname, params object[] parameters) where TReferenceType : class
        {
            if (target == null)
                return null;

            return InvokeMethodByReferenceType(target, typeof(TReferenceType), methodname, parameters);
        }

        /// <summary>
        /// 대상 값타입을 통해 공개되지 않은 일반 함수를 호출.
        /// </summary>
        public static TReturnType InvokeMethodByValueType<TValueType, TReturnType>(TValueType target, string methodname, params object[] parameters) //where TValueType : notnull
        {
            var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            var targetType = target.GetType();

            try
            {
                var returnValue = targetType.InvokeMember(methodname, bindingFlags, Type.DefaultBinder, target, parameters);
                if (returnValue == null)
                    return default;

                return (TReturnType)returnValue;
            }
            catch// (Exception e)
            {
            }

            return default;
        }

        /// <summary>
        /// 대상 값타입을 통해 공개되지 않은 일반 함수를 호출.
        /// </summary>
        public static object InvokeMethodByValueType<TValueType>(TValueType target, string methodname, params object[] parameters) //where TValueType : notnull
        {
            var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            var targetType = target.GetType();

            try
            {
                return targetType.InvokeMember(methodname, bindingFlags, Type.DefaultBinder, target, parameters);
            }
            catch// (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 대상 클래스를 통해 공개되지 않은 스태틱 함수를 호출.
        /// </summary>
        public static object InvokeMethodByStatic(Type targettype, string methodname, params object[] parameters)
        {
            var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            try
            {
                return targettype.InvokeMember(methodname, bindingFlags, Type.DefaultBinder, null, parameters);
            }
            catch// (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 대상 클래스를 통해 공개되지 않은 스태틱 함수를 호출.
        /// </summary>
        public static object InvokeMethodByStatic<T>(string methodname, params object[] parameters)
        {
            return InvokeMethodByStatic(typeof(T), methodname, parameters);
        }
    }
}