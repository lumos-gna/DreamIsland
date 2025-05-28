using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FleeEnemyStats
{
    // 도망가는 Enemy 추가 스텟
    public Transform WanderCenterTransform;
    public float WanderRadius = 10f;
    public float FleeDistance = 5f;
    public float FleeDuration = 2f;
}
