using System;
using System.Collections.Generic;


namespace DagraacSystems.Table
{
	/// <summary>
	/// 공유 테이블.
	/// 목적1. 테이블 시스템으로부터 넘겨받은 테이블 컨테이너를 직접적으로 접근.
	/// 목적2. 테이블 관련 코드에 대한 전용 확장 공간 제공하여 코드 구분 강화.
	/// </summary>
	public class SharedTableTemplete<TSharedTable, TKey, TTableData> : Singleton<TSharedTable>
	  where TSharedTable : SharedTableTemplete<TSharedTable, TKey, TTableData>, new()
	  where TTableData : ITableData
	{
		protected TableContainer m_Container;

		public SharedTableTemplete()
		{
		}

		protected virtual void OnSetContainer() 
		{
		}

		public void SetContainer(TableContainer table)
		{
			m_Container = table;
			OnSetContainer();
		}

		public TTableData Find(Predicate<TTableData> predicate)
		{
			return m_Container.Find<TTableData>(predicate);
		}

		public TTableData Find(TKey key)
		{
			return m_Container.Get<TKey, TTableData>(key);
		}

		public List<TTableData> FindAll(Predicate<TTableData> predicate)
		{
			return m_Container.FindAll<TTableData>(predicate);
		}

		public List<TTableData> All()
		{
			return m_Container.All<TTableData>();
		}
	}
}