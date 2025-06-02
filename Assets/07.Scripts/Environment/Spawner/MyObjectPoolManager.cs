using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabRotationData
{
    public GameObject prefab;
    public bool fixXRotationToMinus90 = false;
}

public class MyObjectPoolManager : MonoBehaviour
{
    private class Pool
    {
        public Queue<GameObject> objects = new Queue<GameObject>();
        public GameObject prefab;
        public Transform parent;
    }

    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    public static MyObjectPoolManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CreatePool(string key, GameObject prefab, int initialSize, Transform parent = null)
    {
        if (_pools.ContainsKey(key))
            return;

        Pool pool = new Pool
        {
            prefab = prefab,
            parent = parent
        };

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab, parent);
            obj.SetActive(false);
            pool.objects.Enqueue(obj);
        }

        _pools[key] = pool;
    }

    public GameObject Get(string key, Vector3 position, Quaternion rotation)
    {
        if (!_pools.ContainsKey(key))
            return null;

        Pool pool = _pools[key];
        GameObject obj;

        if (pool.objects.Count > 0)
        {
            obj = pool.objects.Dequeue();
        }
        else
        {
            obj = Instantiate(pool.prefab, pool.parent);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        return obj;
    }

    public void Release(string key, GameObject obj)
    {
        if (!_pools.ContainsKey(key))
        {
            Destroy(obj);
            return;
        }

        var pool = _pools[key];
        obj.SetActive(false);
        pool.objects.Enqueue(obj);
    }
}
