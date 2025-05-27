using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<BaseEnemy>
{
    
    public void Enter(BaseEnemy obj)
    {
        // obj.GetAnimator()?.CrossFade("Attack", 0f);
    }

    public void Exit(BaseEnemy obj)
    {

    }

    public void Update(BaseEnemy obj)
    {
  
    }
}
