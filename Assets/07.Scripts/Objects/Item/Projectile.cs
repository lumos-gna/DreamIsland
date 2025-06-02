using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float maxLifeTime;

    private Projectile _originPrefab;
    private float _curLifeTime;
    private float _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (other.TryGetComponent<BaseEnemy>(out var enemy))
            {
                enemy.GetHealthBar().SetIsDectect(true);
                enemy.GetEnemyHealth().SetDamage(((int)_damage));
                enemy.TakeDamage((int)_damage);
            }
        }

        Destroy(gameObject, 1f);
    }

    public void Fire(Projectile originPrefab, Vector3 point, Vector3 dir, float force, float damage)
    {
        _originPrefab = originPrefab;
        transform.position = point;
        transform.forward = dir;
        rigid.AddForce(force * dir, ForceMode.Impulse);
        _damage = damage;
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
