using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, object> _poolDict = new();
    

    public ObjectPool<T> CreatePool<T>(T prefab) where T : Component, IPoolable
    {
        string targetName = prefab.gameObject.name;
        
        if (!_poolDict.ContainsKey(targetName))
        {
            var newPool =  new ObjectPool<T>(prefab);

            _poolDict[targetName] = newPool;

            return newPool;
        }

        return null;
    }
    
    public ObjectPool<T> GetPool<T>(string key) where T : Component, IPoolable
    {
        if (_poolDict.ContainsKey(key))
        {
            return (ObjectPool<T>)_poolDict[key];
        }

        return null;
    }
}
