using UnityEngine;


namespace TowerDefense
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool m_ShuttingDown = false;
        private static object m_Lock = new object();
        private static T m_Instance;
    
        protected bool IsInstanced => m_Instance;
    
        public static T Instance
        {
            get
            {
                if (m_ShuttingDown)
                {
                    return null;
                }
    
                lock (m_Lock)
                {
                    FindInstance();
    
                    return m_Instance;
                }
            }
        }
    
        protected static void FindInstance()
        {
            if (!m_Instance)
            {
                m_Instance = (T)FindAnyObjectByType(typeof(T));
            }
        }
    
        private void OnApplicationQuit()
        {
            m_ShuttingDown = true;
        }
    }
}
