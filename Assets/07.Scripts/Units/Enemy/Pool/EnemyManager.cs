using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;


public class EnemyManager : MonoBehaviour
{

    [SerializeField] private RandomSpawner _enemySpawner;

    private List<IPoolableEnemy> _activeEnemies = new List<IPoolableEnemy>();

    private void Awake()
    {
        if (_enemySpawner == null)
        {
            Debug.LogError($"[{name}] _enemySpawner가 할당되지 않았습니다!");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        StartCoroutine(InitializeEnemies());
    }

    private IEnumerator InitializeEnemies()
    {
        // spawnedObjects가 하나라도 있을 때까지 대기
        yield return new WaitUntil(() => _enemySpawner.spawnedObjects.Count > 0);

        foreach (var sd in _enemySpawner.spawnedObjects)
        {
            var go = sd.gameObject;
            if (go.TryGetComponent<IPoolableEnemy>(out var poolEnemy))
            {
                _activeEnemies.Add(poolEnemy);
                poolEnemy.OnRespawn += HandleRespawnEnemy;
            }
        }
    }

    private void HandleRespawnEnemy(IPoolableEnemy enemy)
    {
        StartCoroutine(RespawnEnemy(enemy));
    }

    private IEnumerator RespawnEnemy(IPoolableEnemy enemy)
    {
        // Pool 쪽 Delay가 없다면 임의 값(예: 10초)
        yield return new WaitForSeconds(1f);
        enemy.OnSpawn();
    }

    // 밤에 모든 적 비활성화
    public void OnDespawnAllEnemies()
    {
        foreach (var e in _activeEnemies)
            e.OnDespawn();
    }

    // 아침에 모든 적 활성화
    public void OnSpawnAllEnemies()
    {
        foreach (var e in _activeEnemies)
            e.OnSpawn();
    }
}
