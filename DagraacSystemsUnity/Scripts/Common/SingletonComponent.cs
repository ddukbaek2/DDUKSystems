//using UnityEngine;


//namespace DagraacSystems.Unity
//{
//	public class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T>
//	{
//		private static T m_Instance = null;
//		public static T Instance => Create(null);

//		protected virtual void Awake()
//		{
//			if (m_Instance)
//			{
//				GameObject.DestroyImmediate(gameObject);
//				return;
//			}
//			else
//			{
//				m_Instance = Find();
//				if (m_Instance)
//				{
//					GameObject.DestroyImmediate(gameObject);
//					return;
//				}
//			}
//		}

//		private static T Find()
//		{
//			if (m_Instance)
//				return m_Instance;

//			return GameObject.FindObjectOfType<T>();
//		}

//		public static T Create(GameObject target = null)
//		{
//			m_Instance = Find();
//			if (m_Instance)
//				return m_Instance;

//			if (target)
//			{
//				m_Instance = target.GetOrAddComponent<T>();
//				if (m_Instance)
//					return m_Instance;
//			}
//			else
//			{
//				var type = typeof(T);
//				var newGameObject = new GameObject(type.Name);
//				m_Instance = newGameObject.AddComponent<T>();
//				return m_Instance;
//			}
//		}
//	}
//}