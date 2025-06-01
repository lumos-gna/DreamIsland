using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [Header("스폰할 프리팹 목록")]
    public GameObject[] spawnPrefabs;

    [Header("-90도로 고정 회전할 Prefab 이름")]
    public string[] fixedXRotationPrefabs;

    [Header("스폰 개수")]
    public int spawnCount = 1000;

    [Header("스폰 반경 (XZ)")]
    public float radius = 100f;

    [Header("레이 원점 높이")]
    public float rayOriginHeight = 100f;

    [Header("레이 최대 거리")]
    public float rayDistance = 200f;

    [Header("부모 컨테이너")]
    public Transform parentContainer;

    [Header("스케일 범위")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.3f);

    [Header("낮/밤 스폰 여부")]
    public bool spawnInDay = true;
    public bool spawnInNight = true;

    [Header("리스폰 활성화 (2일 주기)")]
    public bool enableRespawn = false;

    [Header("피해야 하는 반경")]
    public float avoidRadius = 2f;

    [HideInInspector]
    [SerializeField] private int _spawnedCount;

    public List<EnvironmentSpawnData> spawnedObjects { get; private set; } = new List<EnvironmentSpawnData>();

    private bool _lastIsDay;
    private int _cycleCount = 0;

    void Start()
    {
        if (parentContainer == null)
            parentContainer = transform;

        if (spawnPrefabs == null || spawnPrefabs.Length == 0)
        {
            enabled = false;
            return;
        }

        foreach (var prefab in spawnPrefabs)
        {
            if (prefab != null)
                MyObjectPoolManager.Instance.CreatePool(prefab.name, prefab, 100, parentContainer);
        }

        var cycle = FindObjectOfType<DayNightCycle>();
        if (cycle != null)
            cycle.OnCycleComplete.AddListener(OnCycleComplete);

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
        if (spawnInDay && spawnInNight)
        {
            if (spawnedObjects.Count == 0)
                SpawnAll();
            return;
        }

        foreach (var sd in spawnedObjects)
        {
            if (sd != null)
                MyObjectPoolManager.Instance.Release(spawnPrefabs[sd.PrefabIndex].name, sd.gameObject);
        }
        spawnedObjects.Clear();

        if ((isDay && spawnInDay) || (!isDay && spawnInNight))
            SpawnAll();
    }

    private void OnCycleComplete()
    {
        if (!enableRespawn)
            return;

        _cycleCount++;
        if (_cycleCount < 2)
            return;

        _cycleCount = 0;

        foreach (var info in EnvironmentSpawnData.destroyedList)
        {
            if (info.prefabIndex < 0 || info.prefabIndex >= spawnPrefabs.Length)
                continue;

            var prefabName = spawnPrefabs[info.prefabIndex].name;
            var go = MyObjectPoolManager.Instance.Get(prefabName, info.position, info.rotation);
            if (go == null)
                continue;

            go.transform.localScale = info.scale;

            var sd = go.GetComponent<EnvironmentSpawnData>();
            if (sd == null)
                sd = go.AddComponent<EnvironmentSpawnData>();

            sd.InitializeAsLanded(info.prefabIndex, info.position, info.rotation, info.scale);
            spawnedObjects.Add(sd);
        }

        EnvironmentSpawnData.destroyedList.Clear();
    }

    private void SpawnAll()
    {
        int groundMask = 1 << LayerMask.NameToLayer("Ground");
        int avoidMask = LayerMask.GetMask("Portal", "Player");

        _spawnedCount = 0;

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
            GameObject chosenPrefab = spawnPrefabs[prefabIndex];
            if (chosenPrefab == null)
                continue;

            // ✅ -90도 회전 prefab만 따로 처리
            bool fixXRotation = System.Array.Exists(fixedXRotationPrefabs, name => name == chosenPrefab.name);
            float xRotation = fixXRotation ? -90f : 0f;
            Quaternion rot = Quaternion.Euler(xRotation, Random.Range(0f, 360f), 0f);
            float rndScale = Random.Range(scaleRange.x, scaleRange.y);

            var go = MyObjectPoolManager.Instance.Get(chosenPrefab.name, spawnPos, rot);
            if (go == null)
                continue;

            go.transform.localScale = chosenPrefab.transform.localScale * rndScale;

            var sd = go.GetComponent<EnvironmentSpawnData>();
            if (sd == null)
                sd = go.AddComponent<EnvironmentSpawnData>();

            sd.InitializeAsLanded(prefabIndex, spawnPos, rot, go.transform.localScale);
            spawnedObjects.Add(sd);

            _spawnedCount++;
        }
    }
}
