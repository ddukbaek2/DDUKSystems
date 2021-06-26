using System;


namespace DagraacSystems.Table
{
	public interface ITableLoader<TTableID> where TTableID : Enum, new()
	{
		void OnLoadAll();
		void OnLoaded(TTableID tableID);
		TTableData[] LoadFromFile<TTableData>(string path) where TTableData : ITableData;
	}
}