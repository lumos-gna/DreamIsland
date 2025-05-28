using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : BaseEnemy, IRangedEnemy
{
    [SerializeField] private AttackEnemyStats _attackStats;
    [SerializeField] private GameObject _projectilePrefab;
    public override AttackEnemyStats AttackEnemyStats => _attackStats;

    public GameObject GetProjectilePrefab() => _projectilePrefab;

}
