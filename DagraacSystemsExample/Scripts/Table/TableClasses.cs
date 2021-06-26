using DagraacSystems.Table;
using System.Collections.Generic;


namespace DagraacSystemsExample
{
	public class CommonTable : SharedTableTemplete<CommonTable, string, ITableData>
	{
		protected override void OnSetContainer()
		{
			base.OnSetContainer();

			// 초기화.
		}
	}
}