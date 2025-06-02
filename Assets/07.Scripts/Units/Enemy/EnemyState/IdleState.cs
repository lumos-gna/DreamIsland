using UnityEngine;

public class IdleState : IState<BaseEnemy>
{
    private float _timer;

    private float _idleTime; // 모든 적들이 동시에 움직이는걸 방지하기 위한 대기 시간
    private float _minIdleTime = 8f;
    private float _maxIdleTime = 15f;

    public void Enter(BaseEnemy obj)
    {
        obj.StopFootParticle();
        // 대기 시간 랜덤으로 설정
        _idleTime = Random.Range(_minIdleTime, _maxIdleTime);
        _timer = 0f;

        obj.GetAgent().isStopped = true;
        obj.GetAnimator()?.CrossFade("Idle", 0.1f);
    }

    public void Update(BaseEnemy obj)
    {
        _timer += Time.deltaTime;
        if (obj.PlayerInRange())
        {
            obj.GetFSM().ChangeState(obj.StateFactory.Get<FleeState>());
            return;
        }
        if (_timer >= _idleTime)
        {
            obj.GetFSM().ChangeState(obj.StateFactory.Get<MoveState>());
        }
    }

    public void Exit(BaseEnemy obj)
    {
        obj.GetAgent().isStopped = false;
    }
}
