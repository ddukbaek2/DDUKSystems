using System;
using System.Collections.Generic;


namespace DagraacSystems.Table
{
	public interface ITableData
	{
		List<Tuple<string, Type, object>> ToFields();
		int GetFieldIndex(string name);
		string GetFieldName(int index);
		Type GetFieldType(int index);
		object GetFieldValue(int index);
		int GetFieldCount();
	}
}