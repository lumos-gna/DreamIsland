using UnityEngine;

public class RangedEnemy : BaseEnemy, IRangedEnemy, IPoolableEnemy
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
    }
    public void MeleeAttack()
    {
        Vector3 origin = transform.position + Vector3.up * 1f; 
        Vector3 direction = transform.forward;
        float range = 4f;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.TryGetComponent<PlayerCondition>(out var player))
                {
                    player.HealthChange(-_attackStats.AttackPower * Time.deltaTime); // 데미지 적용
                }
            }
        }

        Debug.DrawRay(origin, direction * range, Color.red, 1.0f);
    }

    public void ThrowProjectile()
    {
        Vector3 dir = GetPlayer().transform.position - _projectileSpawnPoint.position;
        
        GameObject projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, Quaternion.LookRotation(dir));

        if (projectile.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = (dir.normalized + Vector3.up * 0.1f).normalized * _throwPower;
        }
    }
}
