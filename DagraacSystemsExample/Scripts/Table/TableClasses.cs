using DagraacSystems.Table;
using System.Collections.Generic;


namespace DagraacSystemsExample
{
	/// <summary>
	/// 샘플 테이블.
	/// </summary>
	public class ExampleTable : SharedTableTemplete<ExampleTable, int, ExampleTableData>
	{
		protected override void OnSetContainer()
		{
			base.OnSetContainer();

			// 초기화.
		}
	}
}