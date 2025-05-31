using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IState<BaseEnemy>
{
    private float _timer = 0;
    private float _coolTime = 0;
    public void Enter(BaseEnemy obj)
    {
        obj.StopFootParticle();
        _coolTime = obj.AttackEnemyStats.CoolTime;
        _timer = _coolTime; // 바로 공격
    }

    public void Update(BaseEnemy obj)
    {
        float dist = Vector3.Distance(obj.transform.position, obj.GetPlayer().transform.position);
        _timer += Time.deltaTime;

        if (!obj.PlayerInRange())
        {
            obj.GetFSM().ChangeState(obj.StateFactory.Get<MoveState>());
            return;
        }

        // 쿨타임 기다리기
        if (_timer < _coolTime)
            return;

        _timer = 0f; // 쿨타임 도달했으면 타이머 리셋

        // 공격 실행
        switch (obj.AttackEnemyStats.AttackEnemyType)
        {
            case AttackEnemyType.Melee:
                HandleMeleeAttack(obj);
                break;

            case AttackEnemyType.Ranged:
                if (dist <= obj.Stats.DetectDistance / 2) // 플레이어가 절반 정도 가까이 왔을때 근접 공격으로
                {
                    HandleMeleeAttack(obj);
                }
                else
                    HandleRangedAttack(obj);
                break;
        }
    }

    public void Exit(BaseEnemy obj)
    {

    }

    private void HandleMeleeAttack(BaseEnemy obj)
    {
        obj.GetAnimator()?.CrossFade("Melee", 0.1f);
        AudioManager.Instance.PlaySFXAtPoint(11, obj.transform.position); // 적 근접 공격소리
    }

    private void HandleRangedAttack(BaseEnemy obj)
    {
        obj.GetAnimator()?.CrossFade("Ranged", 0.1f);
        AudioManager.Instance.PlaySFXAtPoint(6, obj.transform.position);
    }
}
