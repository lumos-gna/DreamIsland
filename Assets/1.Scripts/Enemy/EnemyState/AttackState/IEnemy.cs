using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    Transform GetPlayerTransform(); // 플레이어 위치
    Transform GetEnemyTransform(); // 적 위치
    float GetAttackPower(); // 데미지 
    Animator GetAnimator();
}
