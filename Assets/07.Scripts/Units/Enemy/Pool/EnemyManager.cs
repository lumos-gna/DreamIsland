using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private List<SpawnData> _enemies; // 스폰할 적 리스트
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private float _fieldOfView;

    private void Start()
    {
        _spawnPosition = new SpawnPosition(_playerCamera, _fieldOfView);

        // 적 종류별로 초기 수만큼 스폰
        foreach (var enemy in _enemies)
        {
            for (int i = 0; i < enemy.InitialCount; i++)
            {
                SpawnEnemy(enemy);
            }
        }
    }

    // 적 생성 및 초기화
    private void SpawnEnemy(SpawnData data)
    {
        GameObject go = Instantiate(data.Prefab, _parent);

        // 프리팹에서 BaseEnemy 컴포넌트를 꺼내서 정보 가져오기 없으면 지우기
        if (!go.TryGetComponent(out BaseEnemy enemy))
        {
            Destroy(go);
            return;
        }

        // 위치 재배치
        Vector3 pos = _spawnPosition.GetSpawnPosition(poolFleeEnemy.GetWanderCenter().position, poolFleeEnemy.GetWanderRadius());
        poolFleeEnemy.transform.position = pos;
    }


    // 밤에 모든 적 비활성화
    public void OnDespawnAllEnemies()
    {
        foreach (var enemy in _activeEnemies)
            enemy.OnDespawn();
    }

    // 아침에 모든  활성화
    public void OnSpawnAllEnemies()
    {
        foreach (var enemy in _activeEnemies)
            enemy.OnSpawn();
    }

}
