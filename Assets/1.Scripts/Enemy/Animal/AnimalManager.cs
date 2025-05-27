using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] private Transform _poolParent;
    [SerializeField] private List<AnimalSpawnData> _animals; // 스폰할 동물 리스트
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private float _fieldOfView;
    private List<PoolAnimal> _activeAnimals = new List<PoolAnimal>(); // 현재 활성화된 동물들
    private SpawnAnimalPosition _spawnPosition; // 위치 계산

    private void Start()
    {
        _spawnPosition = new SpawnAnimalPosition(_playerCamera, _fieldOfView);

        // 동물 종류별로 초기 수만큼 스폰
        foreach (var animal in _animals)
        {
            for (int i = 0; i < animal.InitialCount; i++)
            {
                SpawnAnimal(animal);
            }
        }
    }

    // 동물 생성 및 초기화
    private void SpawnAnimal(AnimalSpawnData animal)
    {
        GameObject go = Instantiate(animal.Prefab, _poolParent);

        // 프리팹에서 BaseEnemy 컴포넌트를 꺼내서 정보 가져오기 없으면 지우기
        if (!go.TryGetComponent(out BaseEnemy enemy))
        {
            Destroy(go);
            return;
        }

        // 스폰 위치 계산
        Vector3 pos = _spawnPosition.GetSpawnPosition(enemy.AnimalStats.WanderCenter.position, enemy.AnimalStats.FleeDistance);

        // 위치 배치
        go.transform.position = pos;
        go.transform.rotation = Quaternion.identity;

        //  PoolAnimal 초기화
        if (go.TryGetComponent(out PoolAnimal poolAnimal))
        {
            // 위치 전달
            poolAnimal.Init(enemy.AnimalStats.WanderCenter, enemy.AnimalStats.WanderRadius);
            poolAnimal.OnDie += (poolAnimal) => StartCoroutine(Respawn(poolAnimal));
            poolAnimal.OnSpawn();
            _activeAnimals.Add(poolAnimal);
        }
    }

    // 일정 시간 후 동물 스폰
    private IEnumerator Respawn(PoolAnimal animal)
    {
        yield return new WaitForSeconds(10f); // 리스폰 대기 시간
        animal.OnSpawn();

        // 위치 재배치
        Vector3 pos = _spawnPosition.GetSpawnPosition(animal.GetWanderCenter().position, animal.GetWanderRadius());
        animal.transform.position = pos;
    }


    // 밤에 모든 동물 비활성화
    public void OnDespawnAllAnimals()
    {
        foreach (var animal in _activeAnimals)
            animal.OnDespawn();
    }

    // 아침에 모든 동물 활성화
    public void OnSpawnAllAnimals()
    {
        foreach (var animal in _activeAnimals)
            animal.OnSpawn();
    }

}
