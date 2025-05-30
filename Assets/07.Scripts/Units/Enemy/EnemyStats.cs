using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyStats
{
    // Enemy 관련 스탯들 여기서 설정    
    public float WalkSpeed = 8f;
    public float RunSpeed = 5f;
    public float DetectDistance = 3f; // 플레이어 감지 범위
    public Transform SpawnTransform; // 스폰 위치
}
