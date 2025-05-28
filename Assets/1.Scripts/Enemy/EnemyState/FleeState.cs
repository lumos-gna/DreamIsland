using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FleeState : IState<BaseEnemy>
{
    private float _timer;
    private float _pathUpdateInterval = 0.5f;
    public void Enter(BaseEnemy obj)
    {
        obj.GetAnimator()?.CrossFade("Run", 0.1f);
        obj.GetAgent().isStopped = false;
        obj.GetAgent().speed = obj.Stats.RunSpeed;
        UpdateFleeEnemyPath(obj);
        _timer = _pathUpdateInterval; // 바로 도망 시작
    }
    public void Update(BaseEnemy obj)
    {
        _timer += Time.deltaTime;

        if (!obj.PlayerInRange())
        {
            obj.GetAgent().velocity = Vector3.zero;
            obj.GetFSM().ChangeState(obj.StateFactory.Get<IdleState>());
            return;
        }

        // 다음 도망 위치 업데이트
        if (_timer >= _pathUpdateInterval)
        {
            UpdateFleeEnemyPath(obj);
            _timer = 0f;
        }
    }
    public void Exit(BaseEnemy obj)
    {
        obj.GetAgent().isStopped = true;
    }

    // 최대 범위내 도망칠 수 있는 가장 먼 위치 설정
    private void UpdateFleeEnemyPath(BaseEnemy obj)
    {
        Vector3 center = obj.FleeEnemyStats.WanderCenterTransform.position;
        float radius = obj.FleeEnemyStats.WanderRadius;
        Vector3 playerPos = obj.GetPlayer().transform.position;

        NavMeshAgent agent = obj.GetAgent();
        Vector3 bestPos = obj.transform.position;
        float maxDistance = 0f;

        for (int i = 0; i < 30; i++)
        {
            // 랜덤 위치 (반경 안)
            Vector2 rand = Random.insideUnitCircle * radius;
            Vector3 pos = center + new Vector3(rand.x, 0f, rand.y);

            // NavMesh 유효성 체크
            if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();

                if (agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    float dist = Vector3.Distance(hit.position, playerPos);

                    // 더 멀다면 갱신
                    if (dist > maxDistance)
                    {
                        maxDistance = dist;
                        bestPos = hit.position;
                    }
                }
            }
        }

        obj.TrySetDestination(bestPos);
    }
}
