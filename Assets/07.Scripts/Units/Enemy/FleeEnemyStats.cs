using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FleeEnemyStats
{
    //도망칠때 중심을 자기 자신으로 하려고 추가함
    public Transform WanderCenter;

    // FleeEnemy 추가 스텟
    public float WanderRadius = 10f;
    public float FleeDuration = 2f;
}
