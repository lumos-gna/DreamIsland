using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttackEnemyType
{
    Melee,
    Ranged
}
[System.Serializable]
public class AttackEnemyStats
{
    // 공격형 Enemy 추가 스탯
    public AttackEnemyType AttackEnemyType;
    public Transform SpawnPositin;
    public float AttackPower = 10;
    public float CoolTime = 3f;
}
