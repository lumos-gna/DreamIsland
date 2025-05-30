using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IState<BaseEnemy>
{
    public void Enter(BaseEnemy obj)
    {
        obj.GetAnimator()?.CrossFade("Die", 0f);
    }

    public void Update(BaseEnemy obj)
    {

    }

    public void Exit(BaseEnemy obj)
    {
    }
}
