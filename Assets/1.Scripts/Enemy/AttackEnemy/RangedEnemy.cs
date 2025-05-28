using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy, IRangedEnemy
{
    [SerializeField] private AttackEnemyStats _attackStats;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    public override AttackEnemyStats AttackEnemyStats => _attackStats;

    public GameObject GetProjectilePrefab() => _projectilePrefab;

    public Transform GetProjectileSpawnPoint() => _projectileSpawnPoint;

    public void ThrowProjectile()
    {
        Vector3 start = _projectileSpawnPoint.position;
        Vector3 target = GetPlayer().transform.position;
        Vector3 flatDir = target - start;
        flatDir.y = 0;

        GameObject projectile = Instantiate(_projectilePrefab, start, Quaternion.LookRotation(flatDir));

        if (projectile.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            float gravity = Mathf.Abs(Physics.gravity.y);
            float launchSpeed = 10f; // 속도 조절 가능
            float time = flatDir.magnitude / launchSpeed;

            Vector3 velocity = flatDir.normalized * launchSpeed;
            velocity.y = (target.y - start.y + 0.5f * gravity * time) / time;

            rb.velocity = velocity;
            rb.useGravity = true;
        }
        // 데미지 지정
        if (projectile.TryGetComponent<EnemyProjectile>(out EnemyProjectile proj))
        {
            proj.SetDamage((int)AttackEnemyStats.AttackPower);
        }
    }

    // 근접 공격 할때
    public void MeleeAttack()
    {
        // 캐릭터 생명력 관련 로직 예시
        /*
        if (obj.GetPlayer().TryGetComponent<PlayerHandler>(out var player))
        {
            player.TakeDamage(obj.AttackEnemyStats.AttackPower);
        }*/
    }
}
