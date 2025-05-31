using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject _helathBarSprite;
    [SerializeField] private ConditionHandler _conditionHandler;
    [SerializeField] private ParticleSystem _damageParticle;
    private BaseEnemy _baseEnemy;
    
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        
        _baseEnemy = GetComponent<BaseEnemy>();
        _conditionHandler = GetComponent<ConditionHandler>();
    }

    private void Start()
    {
        // 초기 체력 UI 설정
        if (_healthBar != null && _conditionHandler != null)
        {
            _healthBar.UpdateHealthBar(_conditionHandler.Maxhealth, _conditionHandler.CurHealth);
            _helathBarSprite.gameObject.SetActive(false);
        }

        // 이벤트 등록
        if (_conditionHandler != null)
        {
            _conditionHandler.OnTakeDamage += HandleTakeDamage;
            _conditionHandler.OnDie += HandleDie;
        }
    }

    public void ApplyDamage(float damage)
    {
        _conditionHandler?.TakeDamage(damage);
    }

    private void HandleTakeDamage()
    {
        // 체력바 표시
        if (_healthBar != null)
        {
            _helathBarSprite.gameObject.SetActive(true);
            _healthBar.UpdateHealthBar(_conditionHandler.Maxhealth, _conditionHandler.CurHealth);
            _healthBar.DamageText(2); // 여기에 플레이어 데미지 넣기
        }
        Animator anim = _baseEnemy.GetAnimator();

        if (anim != null && HasParameter(anim, "isDamage"))
        {
            anim.SetTrigger("isDamage");
        }

        // 파티클 실행
        if (_damageParticle != null)
        {
            _damageParticle.Play();
        }

    }
    // 애니메이션 있는지 체크
    private bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
    private void HandleDie()
    {
        _baseEnemy.GetFSM().ChangeState(_baseEnemy.StateFactory.Get<DieState>());
        _helathBarSprite.gameObject.SetActive(false);
    }

}
