using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy, IRangedEnemy, IPoolableEnemy
{
    [SerializeField] private AttackEnemyStats _attackStats;
    [SerializeField] float _hitPower = 2f;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _throwPower;
    
    public override AttackEnemyStats AttackEnemyStats => _attackStats;

    public GameObject GetProjectilePrefab() => _projectilePrefab;
    public Transform GetProjectileSpawnPoint() => _projectileSpawnPoint;
    private bool isMeleeAttack;

    public void RangedAttack()
    {
        ThrowProjectile();
        if (GetPlayer().TryGetComponent<PlayerCondition>(out var player))
        {
           player.HealthChange(_attackStats.AttackPower);
        }
    }
    public void MeleeAttack()
    {
        isMeleeAttack = true;
        if (GetPlayer().TryGetComponent<PlayerCondition>(out var player))
        {
            player.HealthChange(_hitPower);
        }
    }

    public void ThrowProjectile()
    {
        Vector3 dir = GetPlayer().transform.position - _projectileSpawnPoint.position;
        GameObject projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, Quaternion.LookRotation(dir));

        if (projectile.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = dir.normalized * _throwPower;
        }
    }
}
