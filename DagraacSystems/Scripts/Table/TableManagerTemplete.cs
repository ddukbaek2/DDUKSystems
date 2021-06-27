using System;
using System.Collections.Generic;


namespace DagraacSystems.Table
{
	public abstract class TableManagerTemplete<TTableManager, TTableID> : Manager<TTableManager>
		where TTableManager : TableManagerTemplete<TTableManager, TTableID>, new()
		where TTableID : Enum, new()
	{
		private Dictionary<TTableID, TableContainer> m_Tables;

		public TableManagerTemplete() : base()
		{
			m_Tables = new Dictionary<TTableID, TableContainer>();
		}

		protected override void OnDispose(bool disposing)
		{
			UnloadAll();
		}

		protected virtual void OnLoaded(TTableID tableID, TableContainer tableContainer)
		{
		}

		protected virtual bool OnCheckIntegrity(TTableID tableID, TableContainer tableContainer)
		{
			return true;
		}

		protected virtual void OnLoadAll()
		{
		}

		protected abstract TTableData[] LoadFromFile<TTableData>(string path) where TTableData : ITableData;

		protected abstract void LoadFromFileAsync<TTableData>(string path, Action<TTableData[]> onLoadComplete) where TTableData : ITableData;

		/// <summary>
		/// 해당 아이디를 식별자로 삼는 테이블 컨테이너에 해당 경로의 json에서 테이블 데이터를 불러와 적재한다.
		/// 동일 아이디로 셋팅할 경우 머지옵션을 통해 각 경로의 여러개의 테이블을 하나로 만들 수 있다.
		/// 현재 함수는 불러온 인덱스 (0~(Count-1))를 고유식별자로 삼는다.
		/// </summary>
		public bool Load<TTableData>(TTableID tableID, string path, bool isMerge = false) where TTableData : ITableData
		{
			return Load<TTableData>(tableID, path, (Func<int, ITableData, string>)((index, tableData) => index.ToString()), isMerge);
		}

		/// <summary>
		/// 키로 삼을 필드의 값을 고유식별자로 삼는다.(보통 'ID')
		/// </summary>
		public bool Load<TTableData>(TTableID tableID, string path, string keyName, bool isMerge = false) where TTableData : ITableData
		{
			return Load<TTableData>(tableID, path, (index, tableData) => tableData.GetFieldValue(tableData.GetFieldIndex(keyName)).ToString(), isMerge);
		}

		/// <summary>
		/// 키로 삼을 필드의 값을 콜백함수를 통해 직접 임의의 고유식별자를 설정하여 반환한다.
		/// </summary>
		public bool Load<TTableData>(TTableID tableID, string path, Func<int, ITableData, string> generateKeyCallback, bool isMerge = false) where TTableData : ITableData
		{
			var tableDataArray = LoadFromFile<TTableData>(path);
			if (tableDataArray == null)
				return false;

			Load(tableID, tableDataArray, generateKeyCallback, isMerge);
			return true;
		}

		/// <summary>
		/// 비동기버전으로 결과 타이밍은 OnLoaded로 날아가서 따로 콜백이 없음.
		/// </summary>
		public void LoadAsync<TTableData>(TTableID tableID, string path, bool isMerge = false) where TTableData : ITableData
		{
			LoadAsync<TTableData>(tableID, path, (Func<int, ITableData, string>)((index, tableData) => index.ToString()), isMerge);
		}

		/// <summary>
		/// 비동기버전으로 결과 타이밍은 OnLoaded로 날아가서 따로 콜백이 없음.
		/// </summary>
		public void LoadAsync<TTableData>(TTableID tableID, string path, string keyName, bool isMerge = false) where TTableData : ITableData
		{
			LoadAsync<TTableData>(tableID, path, (index, tableData) => tableData.GetFieldValue(tableData.GetFieldIndex(keyName)).ToString(), isMerge);
		}

		/// <summary>
		/// 비동기버전으로 결과 타이밍은 OnLoaded로 날아가서 따로 콜백이 없음.
		/// </summary>
		public void LoadAsync<TTableData>(TTableID tableID, string path, Func<int, ITableData, string> generateKeyCallback, bool isMerge = false) where TTableData : ITableData
		{
			LoadFromFileAsync<TTableData>(path, tableDataArray =>
			{
				Load(tableID, tableDataArray, generateKeyCallback, isMerge);
			});
		}

		/// <summary>
		/// 실제 컨테이너에 적재.
		/// </summary>
		private void Load<TTableData>(TTableID tableID, TTableData[] tableDataArray, Func<int, ITableData, string> generateKeyCallback, bool isMerge = false) where TTableData : ITableData
		{
			var tableContainer = default(TableContainer);
			if (m_Tables.ContainsKey(tableID))
			{
				tableContainer = m_Tables[tableID];
			}
			else
			{
				isMerge = false;
				tableContainer = new TableContainer();
				m_Tables.Add(tableID, tableContainer);
			}

			var tableDataList = new ITableData[tableDataArray.Length];
			for (var index = 0; index < tableDataArray.Length; ++index)
				tableDataList[index] = (ITableData)tableDataArray[index];

			if (isMerge)
				tableContainer.AddContainer(tableDataList);
			else
				tableContainer.SetContainer(tableDataList, generateKeyCallback);

			OnLoaded(tableID, tableContainer);
		}

		public bool CheckIntegrity(TTableID tableID)
		{
			return OnCheckIntegrity(tableID, GetTable(tableID));
		}

		public void LoadAll()
		{
			OnLoadAll();
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

		public TableContainer GetTable(TTableID tableID)
		{
			return m_Tables[tableID];
		}

		public TTableContainer GetTable<TTableContainer>(TTableID tableID) where TTableContainer : TableContainer
		{
			return (TTableContainer)m_Tables[tableID];
		}

		public bool Cotains(TTableID tableID)
		{
			return m_Tables.ContainsKey(tableID);
		}
	}
}