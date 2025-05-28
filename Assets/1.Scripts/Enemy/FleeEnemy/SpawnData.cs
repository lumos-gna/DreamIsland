using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public EnemyType EnemyType;
    public GameObject Prefab; // 생성될 적 프리팹
    public int InitialCount = 10; // 생성될 적 갯수
}
