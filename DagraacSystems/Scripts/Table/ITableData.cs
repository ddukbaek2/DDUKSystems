using System;


namespace DagraacSystems.Table
{
	public interface ITableData
	{
		int GetFieldIndex(string name);
		string GetFieldName(int index);
		Type GetFieldType(int index);
		object GetFieldValue(int index);
		int GetFieldCount();
	}
}