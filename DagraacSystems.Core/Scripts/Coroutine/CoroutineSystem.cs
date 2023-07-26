using System.Collections;
using System.Collections.Generic; // List


namespace DagraacSystems
{
    /// <summary>
    /// 코루틴 시스템.
    /// </summary>
    internal class CoroutineSystem : Component
    {
        private List<Coroutine> m_Coroutines;

        /// <summary>
        /// 생성됨.
        /// </summary>
        protected override void OnCreate(params object[] args)
        {
            base.OnCreate(args);

            m_Coroutines = new List<Coroutine>();
        }

        /// <summary>
        /// 해제됨.
        /// </summary>
        protected override void OnDispose(bool explicitedDispose)
        {
            foreach (var coroutine in m_Coroutines)
                coroutine.Dispose();
            m_Coroutines.Clear();

            base.OnDispose(explicitedDispose);
        }

        /// <summary>
        /// 갱신됨.
        /// </summary>
        public void Tick(float tick)
        {
            foreach (var coroutine in m_Coroutines)
            {
                if (coroutine == null || !coroutine.IsRunning)
                    continue;

                coroutine.Tick(tick);
            }

            for (var i = 0; i < m_Coroutines.Count; ++i)
            {
                var coroutine = m_Coroutines[i];
                if (coroutine == null || coroutine.IsDisposed)
                {
                    m_Coroutines.RemoveAt(i);
                    --i;
                }
            }
        }

        /// <summary>
        /// 코루틴 생성.
        /// </summary>
        public Coroutine CreateCoroutine()
        {
            var coroutine = Create<Coroutine>();
            m_Coroutines.Add(coroutine);
            return coroutine;
        }

        /// <summary>
        /// 코루틴 해제.
        /// </summary>
        public void DisposeCoroutine(Coroutine _coroutine)
        {
            m_Coroutines.Remove(_coroutine);
            _coroutine.Dispose();
        }

        public void Start(IEnumerator _process)
        {
            IEnumerator Process(Coroutine _coroutine)
            {
                while (_process.MoveNext())
                    yield return _process;

                _coroutine.Dispose();
            }

            var coroutine = CreateCoroutine();
            coroutine.Start(Process(coroutine));
        }
    }
}