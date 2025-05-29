using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    private float _maxHealth;
    private float _currentHealth;

    private BaseEnemy _baseEnemy;
    private SpriteRenderer _spriteRenderer;

    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        _baseEnemy = GetComponent<BaseEnemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // 초기 체력 설정
        if (_baseEnemy != null)
        {
            _maxHealth = _baseEnemy.Stats.Health;
            _currentHealth = _maxHealth;
        }

        // 체력바 초기화
        _healthBar?.UpdateHealthBar(_maxHealth, _currentHealth);
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;

        OnHealthChanged?.Invoke(_maxHealth, _currentHealth); // 데미지 변경 이벤트 호출
        StartCoroutine(HitColor()); // 피격효과

        // 피격 소리
        // enemyAudio.start();

        // 파티클
        // hitparticles.transform.position = hitpoint;
        // hitpaticles.play()
        _healthBar.DamageText(damage);

        if (_currentHealth <= 0)
        {
            _baseEnemy.Die();
            _baseEnemy.DropItem(); // 드롭 처리
            _baseEnemy.GetFSM().ChangeState(_baseEnemy.StateFactory.Get<DieState>());
        }
    }
    // 피격 효과
    private IEnumerator HitColor()
    {
        if (_spriteRenderer == null) yield break;

        Color original = _spriteRenderer.color;
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = original;
    }
}
