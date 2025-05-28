using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeEnemy : BaseEnemy
{
    [SerializeField] private FleeEnemyStats _fleeEnemyStats;
    public override FleeEnemyStats FleeEnemyStats => _fleeEnemyStats;

    // 움직이는 전체 범위 확인
    private void OnDrawGizmosSelected()
    {
        if (_fleeEnemyStats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_fleeEnemyStats.WanderCenterTransform.position, _fleeEnemyStats.WanderRadius);
        }
    }
}
