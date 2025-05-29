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

    [Header("부모 컨테이너")]
    public Transform parentContainer;

    [Header("랜덤 스케일 범위")]                              
    public Vector2 scaleRange = new Vector2(0.8f, 1.3f);

    // 생성된 오브젝트 정보
    public List<EnvironmentSpawnData> spawnedObjects { get; private set; }
        = new List<EnvironmentSpawnData>();

    void Start()
    {
        if (parentContainer == null) parentContainer = transform;
     
        if (spawnPrefabs == null || spawnPrefabs.Length == 0)
        {
            enabled = false;  // 이 스크립트 비활성화
            return;
        }

        // Ground 레이어 마스크 미리 계산
        int groundLayer = LayerMask.NameToLayer("Ground");
        int groundMask = 1 << groundLayer;

        for (int i = 0; i < spawnCount; i++)
        {
            // 1) 반경 내 랜덤 XZ 좌표
            Vector2 rnd = Random.insideUnitCircle * radius;
            Vector3 rayOrigin = new Vector3(rnd.x, rayOriginHeight, rnd.y) + transform.position;

            // 2) 바로 레이캐스트 ↓ 방향으로 쏴서 땅 위치만 구함
            if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, groundMask))
            {
                continue;
            }

            // 3) hit.point에 바로 생성
            Vector3 spawnPos = hit.point;

            // 4) 랜덤 Y 회전
            Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // 5) 랜덤 스케일
            float randomScale = Random.Range(scaleRange.x, scaleRange.y);

            // 6) Instantiate 하면서 절대 떨어뜨리지 않음
            GameObject go = Instantiate(
                spawnPrefabs[Random.Range(0, spawnPrefabs.Length)],
                spawnPos,
                rot,
                parentContainer);

            // 인스턴스 스케일 적용
            go.transform.localScale *= randomScale;

            // 7) SpawnData에 착지 위치·시간 기록
            var sd = go.AddComponent<EnvironmentSpawnData>();
            sd.InitializeAsLanded(spawnPos);
            spawnedObjects.Add(sd);
        }
    }
}