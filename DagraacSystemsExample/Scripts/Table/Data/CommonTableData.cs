using DagraacSystems.Table;
using DagraacSystems.Table.Extension;
using System;
using System.Collections.Generic;


public class CommonTableData : ITableData
{
	int ID;
	int Key;
	string Value;

	public int GetFieldCount()
	{
		throw new NotImplementedException();
	}

	public int GetFieldIndex(string name)
	{
		throw new NotImplementedException();
	}

	public string GetFieldName(int index)
	{
		throw new NotImplementedException();
	}

	public Type GetFieldType(int index)
	{
		throw new NotImplementedException();
	}

	public object GetFieldValue(int index)
	{
		throw new NotImplementedException();
	}

	public List<Tuple<string, Type, object>> ToFields()
	{
		throw new NotImplementedException();
	}
}