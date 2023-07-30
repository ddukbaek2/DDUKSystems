using System.Collections;
using System.Collections.Generic;


namespace DagraacSystems
{
    /// <summary>
    /// 프레임워크 오브젝트.
    /// </summary>
    public class FObject : ManagedObject, IUpdateTarget
	{
		/// <summary>
		/// 코루틴 목록.
		/// </summary>
		private List<Coroutine> m_Coroutines;

		private FObject m_Parent;
		private List<FObject> m_Children;

		public FObject Parent => m_Parent;
		public List<FObject> Children => m_Children;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);

			m_Coroutines = new List<Coroutine>();
			m_Parent = null;
			m_Children = new List<FObject>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			StopAllCoroutines();

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 갱신됨.
		/// </summary>
		void IUpdateTarget.Update(float deltaTime)
		{
			var count = m_Coroutines.Count;
			for (var i = 0; i < count; ++i)
			{
				var coroutine = m_Coroutines[i];
				if (coroutine == null || !coroutine.IsRunning || coroutine.IsDisposed)
				{
					m_Coroutines.RemoveAt(i);
					--i;
					--count;
					continue;
				}

				var updateTarget = coroutine as IUpdateTarget;
				updateTarget.Update(deltaTime);
			}
		}

		/// <summary>
		/// 코루틴 시작.
		/// </summary>
		public WaitWhile StartCoroutine(IEnumerator process)
		{
			if (process == null)
				return null;

			IEnumerator Process(Coroutine coroutine)
			{
				coroutine.Start(process);
				while (coroutine.IsRunning)
					yield return null;
				coroutine.Stop();
			}

			var coroutine = Create<Coroutine>();
			m_Coroutines.Add(coroutine);
			coroutine.Start(Process(coroutine));
			return coroutine.WaitForCompletion();
		}

		/// <summary>
		/// 코루틴 정지.
		/// </summary>
		public void StopCoroutine(Coroutine coroutine)
		{
			if (coroutine == null || coroutine.IsDisposed)
				return;

			coroutine.Stop();
		}

		/// <summary>
		/// 모든 코루틴 정지.
		/// </summary>
		public void StopAllCoroutines()
		{
			var count = m_Coroutines.Count;
			for (var i = 0; i < count; ++i)
			{
				var coroutine = m_Coroutines[i];
				StopCoroutine(coroutine);
			}

			m_Coroutines.Clear();
		}


		/// <summary>
		/// 부모 설정.
		/// </summary>
		public void SetParent(FObject targetObject)
		{
			if (m_Parent == targetObject)
				return;

			if (m_Parent != null)
			{
				m_Parent.m_Children.Remove(this);
				m_Parent = null;
			}

			m_Parent = targetObject;
			if (m_Parent != null)
			{
				m_Parent.m_Children.Add(this);
			}
		}
	}
}