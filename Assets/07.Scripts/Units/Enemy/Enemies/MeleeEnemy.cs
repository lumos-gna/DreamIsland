using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy, IPoolableEnemy
{
    [SerializeField] private AttackEnemyStats _attackStats;
    public override AttackEnemyStats AttackEnemyStats => _attackStats;

    public void MeleeAttack()
    {
        // 플레이 맞는 로직
    }
}
