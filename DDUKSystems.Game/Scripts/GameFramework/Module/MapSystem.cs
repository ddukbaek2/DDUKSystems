﻿using DDUKSystems;
using System.Collections;
using System.Collections.Generic;


namespace DDUKSystems.Game
{
	/// <summary>
	/// 맵 모듈.
	/// </summary>
	public class MapSystem : FModule
	{
		Dictionary<long, MapModel> _models;

		/// <summary>
		/// 로드됨.
		/// </summary>
		protected override void OnLoad(OnModuleLoad message)
		{
			base.OnLoad(message);
		}

		/// <summary>
		/// 언로드됨.
		/// </summary>
		protected override void OnUnload(OnModuleUnload message)
		{
			base.OnUnload(message);
		}

		/// <summary>
		/// 업데이트됨.
		/// </summary>
		protected override void OnTick(OnModuleTick message)
		{
			base.OnTick(message);
		}
	}
}