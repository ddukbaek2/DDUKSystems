﻿using DDUKSystems;
using System;
using System.Reflection; // Activator, IDisposable, Type


namespace DDUKSystems
{
	/// <summary>
	/// 생성, 해제 가능한 오브젝트 (일반 클래스 전용).
	/// 생성시 인자를 넣어줄 수 있음 (다만 박싱으로 인해 과도한 밸류타입 사용 주의).
	/// ManagedObject.Create<TClass>() 로 사용한다.
	/// </summary>
	public class ManagedObject : DisposableObject, ICloneable
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		protected virtual void OnCreate(params object[] arguments)
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}


		/// <summary>
		/// 복제 인터페이스 구현체.
		/// </summary>
		object ICloneable.Clone()
		{
			var type = GetType();

			var managedObject = ManagedObject.Create(type);
			if (managedObject == null)
				return null;

			managedObject.OnCreate();

			while (type != null && type != typeof(object))
			{
				var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

				// 필드 복사.
				foreach (var field in type.GetFields(bindingFlags))
				{
					var value = field.GetValue(this);

					if (value is ICloneable cloneable)
						value = cloneable.Clone();

					field.SetValue(managedObject, value);
				}

				// 프로퍼티 복사.
				foreach (var property in type.GetProperties(bindingFlags))
				{
					if (!property.CanRead || !property.CanWrite)
						continue;

					var value = property.GetValue(this);

					if (value is ICloneable cloneable)
						value = cloneable.Clone();

					property.SetValue(managedObject, value);
				}

				type = type.BaseType;
			}

			return managedObject;
		}


		/// <summary>
		/// 타입을 기준으로 생성.
		/// </summary>
		public static T Create<T>(params object[] arguments) where T : ManagedObject, new()
		{
			return Create(typeof(T), arguments) as T;// new T();
		}

		/// <summary>
		/// 타입 인스턴스를 기준으로 생성.
		/// 별도로 타입 인스턴스를 체크 하지 않고 단순 생성 후 형변환하여 반환 하므로 사용상 주의.
		/// </summary>
		public static ManagedObject Create(Type disposableObjectType, params object[] arguments)
		{
			var managedObject = Activator.CreateInstance(disposableObjectType) as ManagedObject;
			if (managedObject == null)
				return null;

			managedObject.OnCreate(arguments);
			return managedObject;
		}

		/// <summary>
		/// 복제.
		/// 일단은 DisposableObject에 ICloneable를 추가하였으나 수정될 수 있음.
		/// 비용이 비싸므로 한정적인 용도로 사용.
		/// </summary>
		public static ManagedObject Clone(ManagedObject target)
		{
			if (target == null)
				return default;

			var disposable = target as IDisposable;
			if (disposable == null)
				return default;

			var clonable = target as ICloneable;
			if (clonable == null)
				return (ManagedObject)target.MemberwiseClone();

			return (ManagedObject)clonable.Clone();
		}
	}
}