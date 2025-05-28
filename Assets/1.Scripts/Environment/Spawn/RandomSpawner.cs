using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [Header("프리팹 리스트")]
    public GameObject[] spawnPrefabs;

    [Header("스폰 개수")]
    public int spawnCount = 100;

    [Header("스폰 반경")]
    public float radius = 50f;

    [Header("높이: 지면 위에서 떨어뜨릴 시작 Y")]
    public float spawnHeight = 50f;

    [Header("지형 레이캐스트 최대 거리")]
    public float rayDistance = 100f;

    [Header("부모 컨테이너")]
    public Transform parentContainer;

    // 생성된 오브젝트 정보
    public List<EnvironmentSpawnData> spawnedObjects { get; private set; } = new List<EnvironmentSpawnData>();

    void Start()
    {
        if (parentContainer == null)
            parentContainer = transform;

        for (int i = 0; i < spawnCount; i++)
        {
            // 반경 내 랜덤 XZ
            Vector2 rndXZ = Random.insideUnitCircle * radius;
            Vector3 origin = new Vector3(rndXZ.x, spawnHeight, rndXZ.y) + transform.position;

            // 위치 걸러내기레이 재시도
            bool valid = false;
            Vector3 spawnPos = origin;
            for (int attempt = 0; attempt < 5; attempt++)
            {
                if (Physics.Raycast(origin, Vector3.down, out var hit, rayDistance))
                {
                    valid = true;
                    // 살짝 위에서 떨어뜨리도록
                    spawnPos = hit.point + Vector3.up * 2f;
                    break;
                }
                // 재시도 시엔 XZ만 새로 뽑고 Y는 동일
                rndXZ = Random.insideUnitCircle * radius;
                origin = new Vector3(rndXZ.x, spawnHeight, rndXZ.y) + transform.position;
            }

            if (!valid)
            {
                // 5번 재시도해도 안 걸리면, 기본 높이로
                spawnPos = origin;
            }

            // 랜덤 프리팹 인스턴스
            var prefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];
            var go = Instantiate(prefab, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0), parentContainer);

            // SpawnData 붙이고, 나중에 땅에 닿은 지점을 기록하게
            var sd = go.AddComponent<EnvironmentSpawnData>();
            spawnedObjects.Add(sd);
        }
    }
}
