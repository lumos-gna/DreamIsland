using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeEnemy : BaseEnemy, IPoolableEnemy
{
    [SerializeField] FleeEnemyStats _fleeStats;
    public override FleeEnemyStats FleeEnemyStats => _fleeStats;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // 도망 반경 시각화
        if(_fleeStats != null)
            Gizmos.DrawWireSphere(transform.position, _fleeStats.WanderRadius);
    }
}
