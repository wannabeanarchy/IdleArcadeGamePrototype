using UnityEngine;

namespace IdleArcade
{ 
    public class SingletonOneScene<T> : MonoBehaviour where T : MonoBehaviour
    { 
        protected static T _instance;

        public static T Instance()
        {
            if (!IsExists())
            {
                Debug.LogError("Singleton: Can't find object " + typeof(T) + " you need manualy add it to scene!");
            }
            return _instance;
        }

        public static void SetInstance(T instance)
        {
            _instance = instance;
        }

        public static bool IsExists()
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance != null)
                {
                    var singleton = _instance as SingletonOneScene<T>;
                    if (singleton != null)
                    {
                        singleton.OnSingletonInit();
                    }
                }
            }
            return _instance != null;
        }

        protected virtual void OnSingletonInit()
        {

        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}