using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [Header("스폰할 프리팹 목록")]
    public GameObject[] spawnPrefabs;

    [Header("스폰 개수")]
    public int spawnCount = 100;

    [Header("스폰 반경 (XZ)")]
    public float radius = 50f;

    [Header("레이 원점 높이")]
    public float rayOriginHeight = 100f;

    [Header("레이 최대 거리")]
    public float rayDistance = 200f;

    [Header("부모 컨테이너")]
    public Transform parentContainer;

    [Header("스케일 범위")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.3f);

    [Header("낮/밤 스폰 여부")]
    // 스폰 유지되려면 둘다 체크 
    public bool spawnInDay = true;      // 낮에 스폰할지
    public bool spawnInNight = true;    // 밤에 스폰할지 

    [Header("리스폰 활성화 (2일 주기)")]
    public bool enableRespawn = false;

    [Header("피해야 하는 반경")]
    public float avoidRadius = 2f;

    // 스폰된 오브젝트 정보
    public List<EnvironmentSpawnData> spawnedObjects { get; private set; }
        = new List<EnvironmentSpawnData>();

    private bool _lastIsDay;
    private int _cycleCount = 0;

    void Start()
    {
        if (parentContainer == null) parentContainer = transform;
        if (spawnPrefabs == null || spawnPrefabs.Length == 0)
        {
            enabled = false;
            return;
        }

        // 1) 낮/밤 사이클 오브젝트 찾기
        var cycle = FindObjectOfType<DayNightCycle>();
        if (cycle != null)
        {
            cycle.OnCycleComplete.AddListener(OnCycleComplete);

            _lastIsDay = DayNightCycle.IsDay;
        }
        else
        {
            _lastIsDay = true;
        }

        HandleCycleChange(_lastIsDay);
    }


    void Update()
    {
        bool isDay = DayNightCycle.IsDay;
        if (isDay != _lastIsDay)
        {
            HandleCycleChange(isDay);
            _lastIsDay = isDay;
        }
    }

    private void HandleCycleChange(bool isDay)
    {
        // 낮/밤 전부 스폰할 경우, 처음 한 번만 스폰
        if (spawnInDay && spawnInNight)
        {
            if (spawnedObjects.Count == 0)
                SpawnAll();
            return;
        }

        // 기존 스폰 오브젝트 전부 삭제
        foreach (var sd in spawnedObjects)
            if (sd != null) Destroy(sd.gameObject);
        spawnedObjects.Clear();

        // 조건 만족할 때만 스폰
        if ((isDay && spawnInDay) || (!isDay && spawnInNight))
            SpawnAll();
    }

    private void OnCycleComplete()
    {
        if (!enableRespawn) return;

        _cycleCount++;
        if (_cycleCount < 2) return; // 2 사이클 지날 때까지 기다려
        _cycleCount = 0;

        // 파괴된 리스트를 순회하며 재생성
        foreach (var info in EnvironmentSpawnData.destroyedList)
        {
            var prefab = spawnPrefabs[info.prefabIndex];
            GameObject go = Instantiate(
                prefab,
                info.position,
                info.rotation,
                parentContainer);
            go.transform.localScale = info.scale;

            var sd = go.AddComponent<EnvironmentSpawnData>();
            sd.InitializeAsLanded(
                info.prefabIndex,
                info.position,
                info.rotation,
                info.scale
            );
            spawnedObjects.Add(sd);
        }

        // 리스트 초기화
        EnvironmentSpawnData.destroyedList.Clear();
    }

    private void SpawnAll()
    {
        int groundMask = 1 << LayerMask.NameToLayer("Ground");
        int avoidMask = LayerMask.GetMask("Portal", "Player");

        for (int i = 0; i < spawnCount; i++)
        {
            // 무작위 XZ 위치 계산
            Vector2 rnd = Random.insideUnitCircle * radius;
            Vector3 origin = transform.position + new Vector3(rnd.x, rayOriginHeight, rnd.y);

            // 땅까지 레이캐스트
            if (!Physics.Raycast(origin, Vector3.down, out var hit, rayDistance, groundMask))
                continue;

            Vector3 spawnPos = hit.point;

            // 플레이어나 포탈 가까이 있으면 건너뛰기
            if (Physics.OverlapSphere(spawnPos, avoidRadius, avoidMask).Length > 0)
                continue;

            int prefabIndex = Random.Range(0, spawnPrefabs.Length);
            Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            float rndScale = Random.Range(scaleRange.x, scaleRange.y);

            GameObject go = Instantiate(
                spawnPrefabs[prefabIndex],
                spawnPos,
                rot,
                parentContainer);
            go.transform.localScale *= rndScale;

            var sd = go.AddComponent<EnvironmentSpawnData>();
            sd.InitializeAsLanded(
                prefabIndex,
                spawnPos,
                rot,
                go.transform.localScale
            );
            spawnedObjects.Add(sd);
        }
    }
}
