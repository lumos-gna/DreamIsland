using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalStats
{
    // 동물 Enemy 추가 스텟
    public Transform WanderCenter;
    public float WanderRadius = 10f;
    public float FleeDistance = 5f;
    public float FleeDuration = 2f;
}
