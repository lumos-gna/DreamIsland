using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private AttackEnemyStats _attackStats;
    public override AttackEnemyStats AttackEnemyStats => _attackStats;

    // 근접 공격 할때
    public void MeleeAttack()
    {
        // 캐릭터 생명력 관련 로직 예시
        /*
        if (obj.GetPlayer().TryGetComponent<PlayerHandler>(out var player))
        {
            player.TakeDamage(obj.AttackEnemyStats.AttackPower);
        }*/
    }
}
