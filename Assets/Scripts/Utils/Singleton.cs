using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object _Lock = new object();
        private static T _instance;
        public static T instance
        {
            get
            {
                lock (_Lock) //Thread Safe
                {
                    //인스턴스 존재 여부 확인
                    _instance = FindObjectOfType(typeof(T)) as T;
                    if (_instance == null)
                    {
                        //인스턴스 생성
                        _instance = new GameObject(typeof(T).ToString(), typeof(T)).AddComponent<T>();
                    }
                }
                return instance;
            }

        }
    }
}

