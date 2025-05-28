using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : BaseEnemy
{
    [SerializeField] private EnemyAttackStats _attackStats;
    public float GetAttackPower() => _attackStats.AttackPower;

}
