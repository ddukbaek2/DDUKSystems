using System;
using System.Collections.Generic;
using DagraacSystems.Core.Scripts.Common;

namespace DagraacSystems
{
    /// <summary>
    /// 공유 테이블.
    /// 상속받아서 사용한다.
    /// 목적1. 테이블 시스템으로부터 넘겨받은 테이블 컨테이너를 직접적으로 접근.
    /// 목적2. 테이블 관련 코드에 대한 전용 확장 공간 제공하여 코드 구분 강화.
    /// 싱글톤으로 접근되지만 매니저는 아니다.
    /// 테이블컨테이너를 별도로 확장 관리할 수 있는 체계.
    /// </summary>
    public class SharedTable<TSharedTable, TKey, TTableData> : SharedClass<TSharedTable>
	  where TSharedTable : SharedTable<TSharedTable, TKey, TTableData>, new()
	  where TTableData : ITableData
	{
		protected TableContainer _container;

		protected virtual void OnSetContainer(TableContainer table) 
		{
		}

		protected virtual void OnUnsetContainer(TableContainer table)
		{
		}

		public void SetTableContainer(TableContainer table)
		{
			if (_container != table)
			{
				_container = table;
				if (table == null)
					OnUnsetContainer(_container); // 이전 컨테이너를 셋팅.
				else
					OnSetContainer(table); // 다음컨테이너를 셋팅.
			}
		}

		public TTableData Find(Predicate<TTableData> predicate)
		{
			return _container.Find<TTableData>(predicate);
		}

		public TTableData Find(TKey key)
		{
			return _container.Get<TKey, TTableData>(key);
		}

		public List<TTableData> FindAll(Predicate<TTableData> predicate)
		{
			return _container.FindAll<TTableData>(predicate);
		}

		public List<TTableData> All()
		{
			return _container.All<TTableData>();
		}
	}
}