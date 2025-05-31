using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeEnemy : BaseEnemy, IPoolableEnemy
{
    [SerializeField] FleeEnemyStats _fleeStats;
    [SerializeField] float _hitPower = 2f;
    public override FleeEnemyStats FleeEnemyStats => _fleeStats;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<PlayerCondition>(out var condition))
            {
                condition.HealthChange(-2f);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // 도망 반경 시각화
        if(_fleeStats != null)
            Gizmos.DrawWireSphere(transform.position, _fleeStats.WanderRadius);
    }
}
