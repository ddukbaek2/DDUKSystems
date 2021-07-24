using System;
using System.Collections.Generic;


namespace DagraacSystems.Table
{
	/// <summary>
	/// 공유 테이블.
	/// 상속받아서 사용한다.
	/// 목적1. 테이블 시스템으로부터 넘겨받은 테이블 컨테이너를 직접적으로 접근.
	/// 목적2. 테이블 관련 코드에 대한 전용 확장 공간 제공하여 코드 구분 강화.
	/// 싱글톤으로 접근되지만 매니저는 아니다.
	/// 테이블컨테이너를 별도로 확장 관리할 수 있는 체계.
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

		public void SetTableContainer(TableContainer table)
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