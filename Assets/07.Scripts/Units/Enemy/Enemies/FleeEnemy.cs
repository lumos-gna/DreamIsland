using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeEnemy : BaseEnemy
{
    [SerializeField] FleeEnemyStats _fleeStats;
    public override FleeEnemyStats FleeEnemyStats => _fleeStats;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // 도망 반경 시각화
        if(_fleeStats != null)
            Gizmos.DrawWireSphere(_fleeStats.WanderCenter.position, _fleeStats.WanderRadius);
    }
}
