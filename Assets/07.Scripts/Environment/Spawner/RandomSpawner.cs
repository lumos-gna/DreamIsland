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

    [Header("부모 컨테이너")]
    public Transform parentContainer;

    [Header("랜덤 스케일 범위")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.3f);

    [Header("낮/밤 스폰 여부")]
    public bool spawnInDay = true;      // 낮에 스폰
    public bool spawnInNight = true;    // 밤에 스폰

    [Header("리스폰 기능 (2days 마다 재스폰)")]
    public bool enableRespawn = false;

    [Header("스폰 회피 반경")]
    public float avoidRadius = 2f;

    // 생성된 오브젝트 정보
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

        // 낮/밤 전환 이벤트 구독
        var cycle = FindObjectOfType<DayNightCycle>();
        if (cycle != null)
            cycle.OnCycleComplete.AddListener(OnCycleComplete);

        // 첫 스폰
        _lastIsDay = DayNightCycle.IsDay;
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
        // 낮·밤 둘 다 켜지면, 최초 한 번만 SpawnAll, 전환 시엔 아무 작업 안 함
        if (spawnInDay && spawnInNight)
        {
            if (spawnedObjects.Count == 0)
                SpawnAll();
            return;
        }

        // 기존에 뿌려진 것 삭제
        foreach (var sd in spawnedObjects)
            if (sd != null) Destroy(sd.gameObject);
        spawnedObjects.Clear();

        // 설정된 시간대에만 스폰
        if ((isDay && spawnInDay) || (!isDay && spawnInNight))
            SpawnAll();
    }

    private void OnCycleComplete()
    {
        if (!enableRespawn) return;

        // 낮→밤→낮 또는 밤→낮→밤, 즉 2번 전환 후
        _cycleCount++;
        if (_cycleCount < 2) return;
        _cycleCount = 0;

        // 파괴된 것만 다시 스폰
        foreach (var info in EnvironmentSpawnData.destroyedList)
        {
            var prefab = spawnPrefabs[info.prefabIndex];
            GameObject go = Instantiate(prefab,
                                        info.position,
                                        info.rotation,
                                        parentContainer);
            go.transform.localScale = info.scale;

            // 다시 추적 대상으로 등록
            var sd = go.AddComponent<EnvironmentSpawnData>();
            sd.InitializeAsLanded(
                info.prefabIndex,
                info.position,
                info.rotation,
                info.scale
            );
            spawnedObjects.Add(sd);
        }

        // 기록 초기화
        EnvironmentSpawnData.destroyedList.Clear();
    }

    private void SpawnAll()
    {
        int groundMask = 1 << LayerMask.NameToLayer("Ground");
        int avoidMask = LayerMask.GetMask("Portal", "Player");

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 rnd = Random.insideUnitCircle * radius;
            Vector3 origin = transform.position + new Vector3(rnd.x, rayOriginHeight, rnd.y);

            if (!Physics.Raycast(origin, Vector3.down, out var hit, rayDistance, groundMask))
                continue;

            Vector3 spawnPos = hit.point;
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
