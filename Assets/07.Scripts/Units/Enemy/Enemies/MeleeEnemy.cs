using UnityEngine;

public class MeleeEnemy : BaseEnemy, IPoolableEnemy
{
    [SerializeField] private AttackEnemyStats _attackStats;
    
    public override AttackEnemyStats AttackEnemyStats => _attackStats;

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
}
