using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Attack,
    Animal
}
public interface IEnemy
{
    Transform GetPlayerTransform(); // 플레이어 위치
    Animator GetAnimator();
    EnemyType GetEnemyType();
}
