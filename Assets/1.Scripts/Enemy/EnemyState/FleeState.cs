using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class FleeState : IState<BaseEnemy>
{
    private float _time = 0f;
    private bool _hasTarget;

    public void Enter(BaseEnemy obj)
    {
        obj.GetAnimator()?.CrossFade("Run", 0.1f);
    }
    public void Update(BaseEnemy obj)
    {
        _time += Time.deltaTime;

        Vector2 enemyPos = obj.transform.position;
        Vector2 playerPos = obj.GetPlayerTransform().position;

        if (Vector2.Distance(enemyPos, playerPos) > obj.AnimalStats.FleeDistance + 0.5f)
        {
            obj.GetFSM().ChangeState(obj.StateFactory.Get<IdleState>());
            return;
        }
    }
    public void Exit(BaseEnemy obj)
    {
        throw new System.NotImplementedException();
    }
}
