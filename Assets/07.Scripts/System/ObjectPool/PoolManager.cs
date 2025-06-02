using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, object> _poolDict = new();

    ObjectPool<T> CreatePool<T>(T prefab) where T : Component, IPoolable
    {
        if (!_poolDict.ContainsKey(prefab.gameObject.name))
        {
            var newPool = new ObjectPool<T>(prefab);

            _poolDict[prefab.gameObject.name] = newPool;

            return newPool;
        }

        return null;
    }

    public ObjectPool<T> GetPool<T>(T prefab) where T : Component, IPoolable
    {
        if (_poolDict.ContainsKey(prefab.gameObject.name))
        {
            return (ObjectPool<T>)_poolDict[prefab.gameObject.name];
        }
        else
        {
            return CreatePool(prefab);
        }
    }
}
