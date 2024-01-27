using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        [Serializable]
        public class ObjectPool
        {
            [Tooltip("Pool's name"), SerializeField]
            private string name;
            public string Name { get => name; }
            [Tooltip("The object to instantiate"), SerializeField]
            private GameObject prefab;
            [Tooltip("The pool of instantated objects"), SerializeField]
            private List<GameObject> pool = new List<GameObject>();


            /// <summary>
            /// Returns a game object from the pool. Instantiate a new one if none is available
            /// </summary>
            /// <param name="position">Position of the object</param>
            /// <param name="rotation">Rotation of the object</param>
            public GameObject Get(Vector3 position = default(Vector3), Vector3 rotation = default(Vector3), Transform parent = null)
            {
                for (int i = 0; i < pool.Count; i++)
                {
                    if (!pool[i].activeInHierarchy)
                    {
                        pool[i].transform.SetParent(parent);
                        pool[i].transform.position = position;
                        pool[i].transform.rotation = Quaternion.Euler(rotation);
                        pool[i].SetActive(true);
                        return pool[i];
                    }
                }

                pool.Add(Instantiate(prefab, position, Quaternion.Euler(rotation)));
                pool[pool.Count - 1].transform.SetParent(parent);
                return pool[pool.Count - 1];
            }

            /// <summary>
            /// Destroyes currently disabled objects
            /// </summary>
            public void DestroyUnused()
            {
                for (int i = 0; i < pool.Count; i++)
                {
                    if (!pool[i].activeInHierarchy)
                    {
                        Destroy(pool[i]);
                        pool.Remove(pool[i]);
                    }
                }
            }

            /// <summary>
            /// Destroyes all objects
            /// </summary>
            public void DestroyAll()
            {
                for (int i = 0; i < pool.Count; i++)
                    Destroy(pool[i]);

                pool.Clear();
            }

            /// <summary>
            /// Instantiates new objects to the pool
            /// </summary>
            /// <param name="count">Number of objects to prepare</param>
            public void Prepare(int count = 0)
            {
                for (int i = 0; i < count; i++)
                {
                    pool.Add(Instantiate(prefab));
                    pool[pool.Count - 1].SetActive(false);
                }
            }
        }

        //A transform to store inactive objects
        private static Transform inactiveObjects;
        private static Transform InactiveObjects
        {
            get
            {
                if (!inactiveObjects)
                    inactiveObjects = new GameObject("Pool").transform;

                return inactiveObjects;
            }
        }

        public List<ObjectPool> pools = new List<ObjectPool>();


        /// <summary>
        /// Get a pool by its name
        /// </summary>
        /// <param name="name"></param>
        public ObjectPool GetPool(string name)
        {
            foreach(ObjectPool pool in pools)
            {
                if(pool.Name == name)
                {
                    return pool;
                }
            }
            return null;
        }
         

        /// <summary>
        /// Disables the object and moves it under the Pool object
        /// </summary>
        /// <param name="obj">Object to disable</param>
        public static void Remove(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(InactiveObjects);
        }
    }


    public class Strings
    {
        public static string PLAYER = "player";
    }
}