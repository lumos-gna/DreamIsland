using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IState<BaseEnemy>
{
    public void Enter(BaseEnemy obj)
    {
        obj.StopFootParticle();
        Animator animator = obj.GetAnimator(); 

        if (animator != null && animator.HasState(0, Animator.StringToHash("Die")))
        {
            animator.CrossFade(Animator.StringToHash("Die"), 0f);
        }
        else
        {
            obj.Die(); // 애니메이션 없으면 즉시 처리
        }
    }

    public void Update(BaseEnemy obj)
    {

    }

    public void Exit(BaseEnemy obj)
    {
    }
}
