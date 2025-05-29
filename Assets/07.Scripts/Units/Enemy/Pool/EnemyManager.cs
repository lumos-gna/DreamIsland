using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private List<SpawnData> _enemies;       // 스폰할 적 리스트
    [SerializeField] private Transform _playerCamera;        // 플레이어 카메라 트랜스폼
    [SerializeField] private float _fieldOfView;

    [Header("랜덤 스폰 설정")]
    [SerializeField] private float spawnRadius = 50f;   // XZ 반경
    [SerializeField] private float rayOriginHeight = 100f;  // 레이 시작 높이
    [SerializeField] private float rayDistance = 200f;  // 레이 최대 거리
    [SerializeField] private LayerMask groundMask;            // Ground 레이어
    [SerializeField] private float spawnYOffset = 0.5f;  // 땅 위 Y 오프셋

    private List<IPoolableEnemy> _activeEnemies = new List<IPoolableEnemy>();
    private SpawnPosition _spawnPosition;  // 기존 위치 계산 도우미

    private void Awake()
    {
        // ── 자동 할당 로직 추가 ──
        if (_playerCamera == null)
        {
            if (Camera.main != null)
                _playerCamera = Camera.main.transform;
            else
                Debug.LogWarning("[EnemyManager] _playerCamera가 할당되지 않았고, Camera.main도 없습니다.");
        }
        if (_parent == null)
            _parent = this.transform;
    }

    private void Start()
    {
        // 이제 _playerCamera가 null이 아닐 테니 예외 없음
        _spawnPosition = new SpawnPosition(_playerCamera, _fieldOfView);

        foreach (var data in _enemies)
        {
            for (int i = 0; i < data.InitialCount; i++)
                SpawnEnemy(data);
        }
    }

    private void SpawnEnemy(SpawnData data)
    {
        GameObject go = Instantiate(data.Prefab, _parent);

        if (!go.TryGetComponent(out BaseEnemy enemy))
        {
            Destroy(go);
            return;
        }

        Vector3 spawnPos = GetRandomGroundPosition();
        go.transform.position = spawnPos;
        go.transform.rotation = Quaternion.identity;

        if (enemy is IPoolableEnemy poolable)
        {
            poolable.OnDie += e => StartCoroutine(Respawn(e));
            poolable.OnSpawn();
            _activeEnemies.Add(poolable);
        }
    }

    private IEnumerator Respawn(IPoolableEnemy enemy)
    {
        yield return new WaitForSeconds(10f);
        enemy.OnSpawn();
        (enemy as MonoBehaviour).transform.position = GetRandomGroundPosition();
    }

    private Vector3 GetRandomGroundPosition()
    {
        Vector2 rndXZ = Random.insideUnitCircle * spawnRadius;
        Vector3 origin = transform.position + new Vector3(rndXZ.x, rayOriginHeight, rndXZ.y);

        if (Physics.Raycast(origin, Vector3.down, out var hit, rayDistance, groundMask))
            return hit.point + Vector3.up * spawnYOffset;

        // 폴백
        return transform.position + Vector3.up * spawnYOffset;
    }

    public void OnDespawnAllEnemies()
    {
        foreach (var enemy in _activeEnemies)
            enemy.OnDespawn();
    }

    public void OnSpawnAllEnemies()
    {
        foreach (var enemy in _activeEnemies)
            enemy.OnSpawn();
    }
}
