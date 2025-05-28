using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SpawnPosition
{
    private Transform _playerCamera;
    private float _fieldOfView;
    private float _minDistance = 8f; // 적이 스폰될때 플레이어와의 최소 거리
    
    public SpawnPosition(Transform playerCamera, float filedOfView)
    {
        _playerCamera = playerCamera;
        if (_playerCamera.TryGetComponent(out Camera cam))
            _fieldOfView = cam.fieldOfView;
        else
            _fieldOfView = 90f;
    }

    // 주어진 중심 위치와 반경 안에서 플레이어 시야 밖에 유효한 위치 반환
    // fieldofview는 시야각
    public Vector3 GetSpawnPosition(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            // 중심 위치 기준 랜덤 위치 계산
            Vector2 rand = Random.insideUnitCircle * radius;
            Vector3 pos = center + new Vector3(rand.x, 0, rand.y);

            // 시야 벗어났는지 확인
            float angle = Vector3.Angle(_playerCamera.forward, (pos - _playerCamera.position).normalized); 
            float dist = Vector3.Distance(_playerCamera.position, pos);

            // 시야 밖이며 일정 거리 이상일때 고려
            if (angle > _fieldOfView / 2f && dist > _minDistance) 
            {
                // NavMesh 위에 유효한 위치 인지 확인
                if (NavMesh.SamplePosition(pos, out NavMeshHit hit, radius, NavMesh.AllAreas))
                {
                    // 실제 반경 안에 있는지 확인
                    if (Vector3.Distance(center, hit.position) <= radius)
                        return hit.position;
                }
            }
        }
        // 유효한 위치 못찾으면 그냥 중심 위치 반환
        return center;
    }
}
