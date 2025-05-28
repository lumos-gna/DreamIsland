using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeEnemy : BaseEnemy
{
    [SerializeField] FleeEnemyStats _fleeStats;
    public override FleeEnemyStats FleeEnemyStats => _fleeStats;

    protected override void Awake()
    {
        base.Awake();
        // FleeEnemyStats.WanderCenter가 비어 있으면 Player의 Transform으로 채워주기
        if (_fleeStats.WanderCenter == null)
        {
            GameObject playerObj = GetPlayer();
            if (playerObj != null)
            {
                _fleeStats.WanderCenter = playerObj.transform;
            }
            else
            {
                var found = GameObject.FindWithTag("Player");
                if (found != null)
                    _fleeStats.WanderCenter = found.transform;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // 도망 반경 시각화
        if(_fleeStats != null)
            Gizmos.DrawWireSphere(_fleeStats.WanderCenter.position, _fleeStats.WanderRadius);
    }
}
