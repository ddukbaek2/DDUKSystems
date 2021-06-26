using System;
using System.Collections.Generic;


namespace DagraacSystems.Table
{
	public class TableManager<TTableID> : Manager<TableManager<TTableID>> where TTableID : Enum, new()
	{
		private Dictionary<TTableID, TableContainer> m_Tables;
		private ITableLoader<TTableID> m_TableLoader;

		public TableManager() : base()
		{
			m_Tables = new Dictionary<TTableID, TableContainer>();
		}

		protected override void OnDispose(bool disposing)
		{
			UnloadAll();
		}

		public void SetLoader(ITableLoader<TTableID> tableLoader)
		{
			m_TableLoader = tableLoader;
		}

		/// <summary>
		/// 해당 아이디를 식별자로 삼는 테이블 컨테이너에 해당 경로의 json에서 테이블 데이터를 불러와 적재한다.
		/// 동일 아이디로 셋팅할 경우 머지옵션을 통해 각 경로의 여러개의 테이블을 하나로 만들 수 있다.
		/// 현재 함수는 불러온 인덱스 (0~(Count-1))를 고유식별자로 삼는다.
		/// </summary>
		public void Load<TTableData>(TTableID tableID, string path, bool isMerge = false) where TTableData : ITableData
		{
			Load<TTableData>(tableID, path, (Func<int, ITableData, string>)((index, tableData) => index.ToString()), isMerge);
		}

		/// <summary>
		/// 키로 삼을 필드의 값을 고유식별자로 삼는다.(보통 'ID')
		/// </summary>
		public void Load<TTableData>(TTableID tableID, string path, string keyName = "ID", bool isMerge = false) where TTableData : ITableData
		{
			Load<TTableData>(tableID, path, (index, tableData) => tableData.GetFieldValue(tableData.GetFieldIndex(keyName)).ToString(), isMerge);
		}

		/// <summary>
		/// 키로 삼을 필드의 값을 콜백함수를 통해 직접 임의의 고유식별자를 설정하여 반환한다.
		/// </summary>
		public void Load<TTableData>(TTableID tableID, string path, Func<int, ITableData, string> generateKeyCallback, bool isMerge = false) where TTableData : ITableData
		{
			if (m_TableLoader == null)
				return;

			var tableDataArray = m_TableLoader.LoadFromFile<TTableData>(path);

			var table = default(TableContainer);
			if (m_Tables.ContainsKey(tableID))
			{
				table = m_Tables[tableID];
			}
			else
			{
				isMerge = false;
				table = new TableContainer();
				m_Tables.Add(tableID, table);
			}

			var tableDataList = new ITableData[tableDataArray.Length];
			for (int index = 0; index < tableDataArray.Length; ++index)
				tableDataList[index] = (ITableData)tableDataArray[index];
			if (isMerge)
				table.AddContainer(tableDataList);
			else
				table.SetContainer(tableDataList, generateKeyCallback);

			m_TableLoader.OnLoaded(tableID);
		}

		public void LoadAll()
		{
			m_TableLoader.OnLoadAll();
			//foreach (TTableID tableID in Enum.GetValues(typeof(TTableID)))
			//{
			//	Load(tableID, );
			//}
		}

		public bool Unload(TTableID tableID)
		{
			return m_Tables.Remove(tableID);
		}

		public void UnloadAll()
		{
			m_Tables.Clear();
		}

		public T GetTable<T>(TTableID tableID) where T : TableContainer
		{
			return (T)m_Tables[tableID];
		}

		public bool Cotains(TTableID tableID)
		{
			return m_Tables.ContainsKey(tableID);
		}
	}
}