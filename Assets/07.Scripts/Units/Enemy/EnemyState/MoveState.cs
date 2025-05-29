using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : IState<BaseEnemy>
{
    private float _timer = 0f;
    private float _pathUpdateInterval = 0.5f;// 플레이어 위치 갱신 간격
   
    private float _waitTime = 0f; // 동시에 움직이는걸 방지하기 위한 대기 시간
    private float _minWaitTime = 1f;
    private float _maxWaitTime = 3f;
    private bool _isDestination = false;

    private Vector3 _lastTargetPos;
    private float _updateThreshold = 0.5f;

    public void Enter(BaseEnemy obj)
    {
        obj.GetAgent().isStopped = false;
        obj.GetAgent().speed = obj.Stats.WalkSpeed;
      
        _waitTime = Random.Range(_minWaitTime, _maxWaitTime);
        _timer = 0f;

        obj.GetAgent().autoBraking = false;
        obj.GetAnimator()?.CrossFade("Move", 0.1f);
 
        // 경로 업데이트
        UpdatePath(obj);
    }

    public void Update(BaseEnemy obj)
    {
        _timer += Time.deltaTime;

        if (obj.GetPlayer() != null && obj.PlayerInRange())
        {
            if (obj.GetEnemyType() == EnemyType.Attack)
                obj.GetFSM().ChangeState(obj.StateFactory.Get<EnemyAttackState>());
            else
                obj.GetFSM().ChangeState(obj.StateFactory.Get<FleeState>());
            return;
        }

        if (obj.GetEnemyType() == EnemyType.Attack)
        {

            UpdatePath(obj);
        }
        else
        {
            if (!_isDestination && HasReachedDestination(obj.GetAgent()))
            {
                _isDestination = true;
            }

            if (_isDestination && _timer >= _waitTime)
            {
                obj.GetFSM().ChangeState(obj.StateFactory.Get<IdleState>());
            }
        }
    }
    public void Exit(BaseEnemy obj)
    {
        obj.GetAgent().isStopped = true;
    }

    // 경로 갱신
    private void UpdatePath(BaseEnemy obj)
    {
        _isDestination = false;

        if (obj.GetEnemyType() == EnemyType.Attack)
            UpdateAttackEnemyPath(obj);
        else
            UpdateFleeEnemyPath(obj);
    }

    // 공격하는 적은 플레이어 위치로 이동
    private void UpdateAttackEnemyPath(BaseEnemy obj)
    {
        var player = obj.GetPlayer()?.transform;
        if (player != null && obj.GetAgent().isOnNavMesh)
        {
            float moved = Vector3.Distance(_lastTargetPos, player.position);
            if (moved > _updateThreshold)
            {
                _lastTargetPos = player.position;

                if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    obj.GetAgent().SetDestination(hit.position);
                }
            }
        }
    }

    // 도망치는 적은 랜덤 위치 이동
    private void UpdateFleeEnemyPath(BaseEnemy obj)
    {
        // null 체크 추가 
        var stats = obj.FleeEnemyStats;
        if (stats == null || stats.WanderCenter == null)
            return;

        // 중심 위치를 WanderCenter로 변
        Vector3 center = stats.WanderCenter.position;
        float radius = stats.WanderRadius;

        Vector2 rand = Random.insideUnitCircle * obj.FleeEnemyStats.WanderRadius;
        Vector3 pos = center + new Vector3(rand.x, 0, rand.y);

        obj.TrySetDestination(pos);
    }

    // 목적지 도달 판정
    private bool HasReachedDestination(NavMeshAgent agent)
    {
        return !agent.pathPending && agent.remainingDistance < 0.5f && agent.velocity.sqrMagnitude < 0.01f;
    }
}
