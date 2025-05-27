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

    public void Enter(BaseEnemy obj)
    {
        obj.GetAnimator()?.CrossFade("Move", 0.1f);
        obj.GetAgent().isStopped = false;
        obj.GetAgent().speed = obj.Stats.WalkSpeed;

        _waitTime = Random.Range(_minWaitTime, _maxWaitTime);
        _timer = 0f;
        _isDestination = false;

        UpdatePath(obj);

    }

    public void Update(BaseEnemy obj)
    {
        _timer += Time.deltaTime;
        
        if (obj.GetEnemyType() == EnemyType.Enemy)
        {
            // 플레이어가 범위 안에 있으면 공격 상태로 전환
            if (obj.PlayerInRange())
            {
                obj.GetFSM().ChangeState(obj.StateFactory.Get<AttackState>());
                return;
            }

            if (_timer >= _pathUpdateInterval)
            {
                UpdatePath(obj);
                _timer = 0f;
            }
        }
        else
        {
            // 플레이어가 범위 안에 있으면 도망 상태로 전환
            if(obj.PlayerInRange())
            {
                obj.GetFSM().ChangeState(obj.StateFactory.Get<FleeState>());
                return;
            }
            // 목적지 도착 체크
            if (!_isDestination && !obj.GetAgent().pathPending && obj.GetAgent().remainingDistance < 0.5f && obj.GetAgent().velocity.sqrMagnitude < 0.01f)
            {
                _isDestination = true;
            }

            // 목적지 도착후 waitTime이 넘었으면 Idle 상태로 변환
            if (_isDestination)
            {
                if (_timer >= _waitTime)
                {
                    obj.GetFSM().ChangeState(obj.StateFactory.Get<IdleState>());
                }
            }
        }
    }

    public void Exit(BaseEnemy obj)
    {
        obj.GetAgent().isStopped = true;
    }

    private void UpdatePath(BaseEnemy obj)
    {
        if (obj.GetEnemyType() == EnemyType.Enemy)
            UpdateEnemyPath(obj);
        else
            UpdateAnimalPath(obj);
    }

    // 적은 플레이어 위치로 경로 설정
    private void UpdateEnemyPath(BaseEnemy obj)
    {
        Transform player = obj.GetPlayerTransform();
        if (player != null && obj.GetAgent().isOnNavMesh)
        {
            TrySetDestination(obj, player.position);
        }
    }

    // 동물은 주변 랜덤 위치로 경로 설정
    private void UpdateAnimalPath(BaseEnemy obj)
    {
        Vector3 center = obj.AnimalStats.WanderCenter.position; // Wander 반경 중심 위치
        Vector2 randomCircle = Random.insideUnitCircle * obj.AnimalStats.WanderRadius;
        Vector3 targetPos = center + new Vector3(randomCircle.x, 0, randomCircle.y);
        TrySetDestination(obj, targetPos);
    }
    
    // NavMesh에서 유요한 위치인지 체크 및 경로 설정
    private void TrySetDestination(BaseEnemy obj, Vector3 Target)
    {
        NavMeshAgent agent = obj.GetAgent();

        // 유효한 위치 찾기
        if (NavMesh.SamplePosition(Target, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(hit.position);
                return;
            }
        }
        // 실패 시 제자리에
        agent.SetDestination(obj.transform.position);
    }
}
