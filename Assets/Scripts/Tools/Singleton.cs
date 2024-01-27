using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tools
{
    public class Singleton<T> : MonoBehaviour where T:Singleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (Singleton<T>.instance == null)
                {
                    Singleton<T>.instance = FindAnyObjectByType<T>();
                    if (Singleton<T>.instance == null)
                    {
                        Debug.Log($"No {typeof(T).Name} Singleton Instance");
                    }
                
                }
                return Singleton<T>.instance;
            }
        }
        protected virtual void Awake()
        {
            this.CheckInstance();
        }
        public static bool HasInstance
        {
            get
            {
                return Singleton<T>.instance != null;
            }
        }
        protected bool CheckInstance()
        {
            if (Singleton<T>.instance == null)
            {
                Singleton<T>.instance = (T)((object)this);
                DontDestroyOnLoad(this);
                return true;
            }
            if (Singleton<T>.instance == this)
            {
                DontDestroyOnLoad(this);
                return true;
            }
            Destroy(this.gameObject);
            return false;
        }
    }
}

