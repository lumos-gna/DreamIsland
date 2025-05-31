// RandomSpawner.cs
using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [Header("������ ����Ʈ")]
    public GameObject[] spawnPrefabs;

    [Header("���� ����")]
    public int spawnCount = 100;

    [Header("���� �ݰ� (XZ)")]
    public float radius = 50f;

    [Header("����ĳ��Ʈ ���� ����")]
    public float rayOriginHeight = 100f;

    [Header("���� ����ĳ��Ʈ �ִ� �Ÿ�")]
    public float rayDistance = 200f;

    [Header("�θ� �����̳�")]
    public Transform parentContainer;

    [Header("���� ������ ����")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.3f);

    [Header("��/�� ���� ����")]
    public bool spawnInDay = true;      // ���� ����
    public bool spawnInNight = true;    // �㿡 ����

    [Header("������ ��� (2days ���� �罺��)")]
    public bool enableRespawn = false;

    [Header("���� ȸ�� �ݰ�")]
    public float avoidRadius = 2f;

    // ������ ������Ʈ ����
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

        // ��/�� ��ȯ �̺�Ʈ ����
        var cycle = FindObjectOfType<DayNightCycle>();
        if (cycle != null)
            cycle.OnCycleComplete.AddListener(OnCycleComplete);

        // ù ����
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
        // ������ �� �� ������, ���� �� ���� SpawnAll, ��ȯ �ÿ� �ƹ� �۾� �� ��
        if (spawnInDay && spawnInNight)
        {
            if (spawnedObjects.Count == 0)
                SpawnAll();
            return;
        }

        // ������ �ѷ��� �� ����
        foreach (var sd in spawnedObjects)
            if (sd != null) Destroy(sd.gameObject);
        spawnedObjects.Clear();

        // ������ �ð��뿡�� ����
        if ((isDay && spawnInDay) || (!isDay && spawnInNight))
            SpawnAll();
    }

    private void OnCycleComplete()
    {
        if (!enableRespawn) return;

        // �����泷 �Ǵ� ��泷���, �� 2�� ��ȯ ��
        _cycleCount++;
        if (_cycleCount < 2) return;
        _cycleCount = 0;

        // �ı��� �͸� �ٽ� ����
        foreach (var info in EnvironmentSpawnData.destroyedList)
        {
            var prefab = spawnPrefabs[info.prefabIndex];
            GameObject go = Instantiate(prefab,
                                        info.position,
                                        info.rotation,
                                        parentContainer);
            go.transform.localScale = info.scale;

            // �ٽ� ���� ������� ���
            var sd = go.AddComponent<EnvironmentSpawnData>();
            sd.InitializeAsLanded(
                info.prefabIndex,
                info.position,
                info.rotation,
                info.scale
            );
            spawnedObjects.Add(sd);
        }

        // ��� �ʱ�ȭ
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
