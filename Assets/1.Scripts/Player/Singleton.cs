using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject SingletonObject = new GameObject();
                _instance = SingletonObject.AddComponent<T>();
                SingletonObject.name = typeof(T).ToString() + " " + "Singleton";
                DontDestroyOnLoad(SingletonObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null )
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
