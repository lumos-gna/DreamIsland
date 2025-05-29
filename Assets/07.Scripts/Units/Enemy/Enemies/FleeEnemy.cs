using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeEnemy : BaseEnemy, IPoolableEnemy
{
    [SerializeField] FleeEnemyStats _fleeStats;
    public override FleeEnemyStats FleeEnemyStats => _fleeStats;

    protected override void Awake()
    {
        base.Awake();
        // FleeEnemyStats.WanderCenter가 비어 있으면 Player의 Transform으로 채워주기
        if (Stats.SpawnTransform == null)
        {
            if (GetPlayer() != null)
            {
                Stats.SpawnTransform = GetPlayer().transform;
            }
            else
            {
                var found = GameObject.FindWithTag("Player");
                if (found != null)
                    Stats.SpawnTransform = found.transform;
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
