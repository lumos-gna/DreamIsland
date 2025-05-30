using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component, IPoolable
{
    private T _prefab;
    
    private Queue<T> _pool = new();
    private List<T> _activeObjects = new();
    public ObjectPool(T prefab)
    {
        _prefab = prefab;
    }
    
    
    public T Spawn(Transform parent)
    {
        T target = _pool.Count > 0 ? _pool.Dequeue() : Object.Instantiate(_prefab, parent);

        _activeObjects.Add(target);
        
        target.OnSpawn();

        return target;
    }
    

    public void Despawn(T obj)
    {
        if (_activeObjects.Contains(obj))
        {
            _activeObjects.Remove(obj);
        }
        
        _pool.Enqueue(obj);
        
        obj.OnDespawn();
    }
    
    public void DespawnAll()
    {
        foreach (var obj in _activeObjects)
        {
            _pool.Enqueue(obj);
            obj.OnDespawn();
        }

        _activeObjects.Clear();
    }

 
}
