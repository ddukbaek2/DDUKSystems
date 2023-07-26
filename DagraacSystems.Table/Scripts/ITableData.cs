using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 테이블 필드 정보.
	/// </summary>
	public struct FieldInfo
	{
		public string Name;
		public Type Type;
		public object Value;
	}


	/// <summary>
	/// 테이블 데이터의 인터페이스.
	/// 이를 상속받은 테이블데이터는 제너레이트된 코드이며 명세를 모두 자동 구현한다.
	/// 각 데이터의 필드와 값에 대한 메타정보로 공통코드에서의 일반화된 접근을 가능하게 만든다.
	/// </summary>
	public interface ITableData
	{
		List<FieldInfo> ToFields();
		int GetFieldIndex(string _name);
		string GetFieldName(int _index);
		Type GetFieldType(int _index);
		object GetFieldValue(int _index);
		int GetFieldCount();
	}
}