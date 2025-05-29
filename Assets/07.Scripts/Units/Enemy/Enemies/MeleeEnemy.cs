using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy, IPoolableEnemy
{
    [SerializeField] private AttackEnemyStats _attackStats;
    [SerializeField] private float _hitPower = 2f;
    public override AttackEnemyStats AttackEnemyStats => _attackStats;

    public void MeleeAttack()
    {
        if (GetPlayer().TryGetComponent<PlayerCondition>(out var player))
        {
            player.HealthChange(_hitPower);
        }
    }
}
