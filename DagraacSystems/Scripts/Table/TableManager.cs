using System;
using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 테이블 매니저 베이스.
	/// 실제 파일에서 구조체까지 뽑아오는 코드는 제외되어있다.
	/// </summary>
	public abstract class TableManager<TTableManager, TTableID> : Singleton<TTableManager>
		where TTableManager : TableManager<TTableManager, TTableID>, new()
		where TTableID : Enum, new()
	{
		private Dictionary<TTableID, TableContainer> _tables;

		public TableManager() : base()
		{
			_tables = new Dictionary<TTableID, TableContainer>();
		}

		protected override void OnCreate()
		{
			base.OnCreate();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			_tables.Clear();

			base.OnDispose(explicitedDispose);
		}

		protected virtual void OnLoaded(TTableID tableID, TableContainer tableContainer)
		{
		}

		/// <summary>
		/// 정합성 체크.
		/// </summary>
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
			if (_tables.ContainsKey(tableID))
			{
				tableContainer = _tables[tableID];
			}
			else
			{
				isMerge = false;
				tableContainer = new TableContainer();
				_tables.Add(tableID, tableContainer);
			}

			var tableDataList = new ITableData[tableDataArray.Length];
			for (var index = 0; index < tableDataArray.Length; ++index)
				tableDataList[index] = (ITableData)tableDataArray[index];

			if (isMerge)
				tableContainer.AddTableData(tableDataList);
			else
				tableContainer.SetTableData(tableDataList, generateKeyCallback);

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
			return _tables.Remove(tableID);
		}

		public void UnloadAll()
		{
			_tables.Clear();
		}

		public TableContainer GetTable(TTableID tableID)
		{
			return _tables[tableID];
		}

		public TTableContainer GetTable<TTableContainer>(TTableID tableID) where TTableContainer : TableContainer
		{
			return (TTableContainer)_tables[tableID];
		}

		public bool Cotains(TTableID tableID)
		{
			return _tables.ContainsKey(tableID);
		}
	}
}