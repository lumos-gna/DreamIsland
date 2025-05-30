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
    [SerializeField] private GameObject _attackParticle;
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
        // 파티클 실행
        if (_attackParticle != null)
        {
            GameObject particle = Instantiate(_attackParticle, transform.position, Quaternion.identity);
        }

    }

    private void HandleDie()
    {
        Debug.Log("죽음");
        _baseEnemy.GetFSM().ChangeState(_baseEnemy.StateFactory.Get<DieState>());
        _helathBarSprite.gameObject.SetActive(false);
    }

}
