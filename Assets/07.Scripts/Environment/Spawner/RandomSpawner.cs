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

    [Header("낮/밤 스폰 여부")]
    public bool spawnInDay = true;      // 낮에 스폰
    public bool spawnInNight = true;    // 밤에 스폰

    [Header("스폰 회피 반경")]
    public float avoidRadius = 2f;

    // 생성된 오브젝트 정보
    public List<EnvironmentSpawnData> spawnedObjects { get; private set; }
        = new List<EnvironmentSpawnData>();

    private bool _lastIsDay;

    void Start()
    {
        if (parentContainer == null) parentContainer = transform;
        if (spawnPrefabs == null || spawnPrefabs.Length == 0)
        {
            enabled = false;
            return;
        }

        // 첫 스폰 시점에 현재 낮/밤 상태를 읽어서 처리
        _lastIsDay = DayNightCycle.IsDay;
        HandleCycleChange(_lastIsDay);
    }
    void Update()
    {
        bool isDay = DayNightCycle.IsDay;
        if (isDay != _lastIsDay)
        {
            // 낮→밤 or 밤→낮 전환 시
            HandleCycleChange(isDay);
            _lastIsDay = isDay;
        }
    }

    private void HandleCycleChange(bool isDay)
    {
        // 1) 기존에 뿌려진 것 삭제
        foreach (var sd in spawnedObjects)
            if (sd != null) Destroy(sd.gameObject);
        spawnedObjects.Clear();

        // 2) 조건에 맞을 때만 SpawnAll 실행
        if ((isDay && spawnInDay) ||
            (!isDay && spawnInNight))
        {
            SpawnAll();
        }
    }

    // 실제 스폰 루프 (기존 Start() 안의 for문을 옮긴 것)
    private void SpawnAll()
    {
        int groundLayer = LayerMask.NameToLayer("Ground");
        int groundMask = 1 << groundLayer;
        int avoidMask = LayerMask.GetMask("Portal", "Player");

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 rnd = Random.insideUnitCircle * radius;
            Vector3 rayOrigin = transform.position + new Vector3(rnd.x, rayOriginHeight, rnd.y);

            if (!Physics.Raycast(rayOrigin, Vector3.down, out var hit, rayDistance, groundMask))
                continue;

            Vector3 spawnPos = hit.point;
            if (Physics.OverlapSphere(spawnPos, avoidRadius, avoidMask).Length > 0)
                continue;

            Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            float rndScale = Random.Range(scaleRange.x, scaleRange.y);

            GameObject go = Instantiate(
                spawnPrefabs[Random.Range(0, spawnPrefabs.Length)],
                spawnPos,
                rot,
                parentContainer);
            go.transform.localScale *= rndScale;

            var sd = go.AddComponent<EnvironmentSpawnData>();
            sd.InitializeAsLanded(spawnPos);
            spawnedObjects.Add(sd);
        }
    }
}