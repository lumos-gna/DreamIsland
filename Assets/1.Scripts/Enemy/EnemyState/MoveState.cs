using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : IState<BaseEnemy>
{
    private float _pathUpdateInterval = 0.5f;
    private float _pathUpdateTimer = 0f;
    public void Enter(BaseEnemy obj)
    {
        //obj.GetAnimator()?.CrossFade("Move", 0f);
        obj.GetAgent().isStopped = false;
        obj.GetAgent().speed = obj.Stats.WalkSpeed;

        // 플레이어한테 목적지 설정
        UpdatePath(obj);
    }

    public void Update(BaseEnemy obj)
    {
        // 플레이어가 범위 안에 있으면 공격 상태로 전환
        if(obj.PlayerInRange())
        {
            obj.GetFSM().ChangeState(obj.StateFactory.Get<AttackState>());
            return;
        }

        _pathUpdateTimer += Time.deltaTime;
        if (_pathUpdateTimer >= _pathUpdateInterval)
        {
            UpdatePath(obj);
            _pathUpdateTimer = 0f;
        }

        // 애니메이션 제어 예시
        if (obj.GetAgent().velocity.magnitude > 0.1f)
        {
            obj.GetAnimator()?.SetBool("IsMoving", true);
        }
        else
        {
            obj.GetAnimator()?.SetBool("IsMoving", false);
        }
    }

    public void Exit(BaseEnemy obj)
    {
        obj.GetAgent().isStopped = true;
        obj.GetAnimator()?.SetBool("IsMoving", false);
    }

    private void UpdatePath(BaseEnemy obj)
    {
        Transform player = obj.GetPlayerTransform();
        NavMeshAgent agent = obj.GetAgent();
        if (player != null && obj.GetAgent().isOnNavMesh)
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(player.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                Debug.LogWarning($"{obj.name} :: 플레이어로의 유효한 경로가 없습니다.");
            }
        }
    }
}
