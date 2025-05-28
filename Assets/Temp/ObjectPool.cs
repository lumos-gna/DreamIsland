using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component, IPoolable
{
    private T _prefab;
    
    private Queue<T> _pool = new();

    public ObjectPool(T prefab)
    {
        _prefab = prefab;
    }
    
    
    public T Spawn(Transform parent)
    {
        T target = _pool.Count > 0 ? _pool.Dequeue() : Object.Instantiate(_prefab, parent);

        target.OnSpawn();

        return target;
    }
    

    public void Despawn(T obj)
    {
        _pool.Enqueue(obj);
        
        obj.OnDespawn();
    }
}
