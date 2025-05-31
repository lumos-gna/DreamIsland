using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] private Rigidbody rigid;

    [SerializeField] private float maxLifeTime;
    

    private Projectile _originPrefab;
    
    private float _curLifeTime;


    public void Fire(Projectile originPrefab, Vector3 point,  Vector3 dir, float force)
    {
        _originPrefab = originPrefab;
        
        transform.position = point;

        transform.forward = dir;

        rigid.AddForce(force * dir, ForceMode.Impulse);
    }

    private void Update()
    {
        _curLifeTime += Time.deltaTime;

        if (_curLifeTime >= maxLifeTime)
        {
            PoolManager.Instance.GetPool(_originPrefab).Despawn(this);
        }
    }


    public void OnSpawn()
    {
        _curLifeTime = 0;
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        rigid.velocity = Vector3.zero;
        
        gameObject.SetActive(false);
    }
}
