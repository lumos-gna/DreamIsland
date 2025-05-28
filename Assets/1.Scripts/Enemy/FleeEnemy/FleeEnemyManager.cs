using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class FleeEnemyMananger : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private List<SpawnData> _fleeEnemies; // 스폰할 적 리스트
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private float _fieldOfView;
    private List<PoolFleeEnemy> _activeFleeEnemies = new List<PoolFleeEnemy>(); // 현재 활성화된 적들
    private SpawnFleeEnemyPosition _spawnPosition; // 위치 계산 도우미

    private void Start()
    {
        _spawnPosition = new SpawnFleeEnemyPosition(_playerCamera, _fieldOfView);

        // 적 종류별로 초기 수만큼 스폰
        foreach (var fleeEnemy in _fleeEnemies)
        {
            for (int i = 0; i < fleeEnemy.InitialCount; i++)
            {
                Spawn(fleeEnemy);
            }
        }
    }

    // 도망가는 적 생성 및 초기화
    private void Spawn(SpawnData data)
    {
        GameObject go = Instantiate(data.Prefab, _parent);

        // 프리팹에서 BaseEnemy 컴포넌트를 꺼내서 정보 가져오기 없으면 지우기
        if (!go.TryGetComponent(out BaseEnemy enemy))
        {
            Destroy(go);
            return;
        }

        // _spawnPosition이용해서 스폰 위치 계산
        Vector3 pos = _spawnPosition.GetSpawnPosition(enemy.FleeEnemyStats.WanderCenterTransform.position, enemy.FleeEnemyStats.FleeDistance);

        // 위치 배치
        go.transform.position = pos;
        go.transform.rotation = Quaternion.identity;

        //  PoolFleeEnemy 초기화
        if (go.TryGetComponent(out PoolFleeEnemy poolFleeEnemy))
        {
            // 위치 전달
            poolFleeEnemy.Init(enemy.FleeEnemyStats.WanderCenterTransform, enemy.FleeEnemyStats.WanderRadius);
            poolFleeEnemy.OnDie += (fleeEnemy) => StartCoroutine(Respawn(fleeEnemy));
            poolFleeEnemy.OnSpawn();
            _activeFleeEnemies.Add(poolFleeEnemy);
        }
    }

    // 일정 시간 후 적 스폰
    private IEnumerator Respawn(PoolFleeEnemy poolFleeEnemy)
    {
        yield return new WaitForSeconds(10f); // 리스폰 대기 시간
        poolFleeEnemy.OnSpawn();

        // 위치 재배치
        Vector3 pos = _spawnPosition.GetSpawnPosition(poolFleeEnemy.GetWanderCenter().position, poolFleeEnemy.GetWanderRadius());
        poolFleeEnemy.transform.position = pos;
    }


    // 밤에 모든 적 비활성화
    public void OnDespawnAllFleeEnemies()
    {
        foreach (var fleeEnemy in _activeFleeEnemies)
            fleeEnemy.OnDespawn();
    }

    // 아침에 모든 적 활성화
    public void OnSpawnAllEnemies()
    {
        foreach (var fleeEnemy in _activeFleeEnemies)
            fleeEnemy.OnSpawn();
    }

}
