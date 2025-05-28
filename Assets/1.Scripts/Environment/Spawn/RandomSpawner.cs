// RandomSpawner.cs
using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [Header("프리팹 리스트")]
    public GameObject[] spawnPrefabs;

    [Header("스폰 개수")]
    public int spawnCount = 100;

    [Header("스폰 반경 (XZ)")]
    public float radius = 50f;

    [Header("레이캐스트 시작 높이")]
    public float rayOriginHeight = 100f;

    [Header("지형 레이캐스트 최대 거리")]
    public float rayDistance = 200f;

    [Header("땅 위 Y 오프셋")]
    public float spawnYOffset = 0.5f;

    [Header("부모 컨테이너")]
    public Transform parentContainer;

    public List<EnvironmentSpawnData> spawnedObjects { get; private set; }
        = new List<EnvironmentSpawnData>();

    // 땅 위치만 저장
    private List<Vector3> _spawnPositions = new List<Vector3>();

    void Start()
    {
        if (parentContainer == null)
            parentContainer = transform;

        // 땅 위치 캐싱
        int groundMask = LayerMask.GetMask("Ground");
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 rndXZ = Random.insideUnitCircle * radius;
            Vector3 origin = transform.position + new Vector3(rndXZ.x, rayOriginHeight, rndXZ.y);

            if (Physics.Raycast(origin, Vector3.down, out var hit, rayDistance, groundMask))
            {
                _spawnPositions.Add(hit.point);
            }
            else
            {
                Debug.LogWarning($"[RandomSpawner] Ground 못 찾음 at {origin}");
            }
        }

        // 캐시된 위치 위에 생성
        foreach (var point in _spawnPositions)
        {

            Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            // 땅에서 살짝 띄워서 생성할때 씀
            Vector3 spawnPos = point + Vector3.up * spawnYOffset;

            GameObject prefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];
            GameObject go = Instantiate(prefab, spawnPos, rot, parentContainer);

            // SpawnData 부착 및 초기화
            var sd = go.AddComponent<EnvironmentSpawnData>();
            sd.InitializeAsLanded(point);
            spawnedObjects.Add(sd);
        }
    }
}
