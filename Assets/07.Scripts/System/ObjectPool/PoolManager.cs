using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, object> _poolDict = new();
    

    public ObjectPool<T> CreatePool<T>(T prefab) where T : Component, IPoolable
    {
        string targetName = typeof(T).Name;
        
        if (!_poolDict.ContainsKey(targetName))
        {
            var newPool =  new ObjectPool<T>(prefab);

            _poolDict[targetName] = newPool;

            return newPool;
        }

        return null;
    }
    
    public ObjectPool<T> GetPool<T>() where T : Component, IPoolable
    {
        string targetName = typeof(T).Name;

        if (_poolDict.ContainsKey(targetName))
        {
            return (ObjectPool<T>)_poolDict[targetName];
        }

        return null;
    }
}
