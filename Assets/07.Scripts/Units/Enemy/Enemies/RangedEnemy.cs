using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy, IRangedEnemy
{
    [SerializeField] private AttackEnemyStats _attackStats;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _throwPower;
    public override AttackEnemyStats AttackEnemyStats => _attackStats;

    public GameObject GetProjectilePrefab() => _projectilePrefab;
    public Transform GetProjectileSpawnPoint() => _projectileSpawnPoint;


    public void RangedAttack()
    {
        ThrowProjectile();
        // 플레이어 맞는 로직
    }
    public void MeleeAttack()
    {
        // 플레이 맞는 로직
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
