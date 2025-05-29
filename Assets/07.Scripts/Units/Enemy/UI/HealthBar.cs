using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private Image _healthBarForegroundSprite;
    [SerializeField] private Image _helathBarSprite;
    [SerializeField] private Vector3 _offset = new Vector3(0, -1f, 0);
    [SerializeField] private float _speed = 3f;
    private Transform _target;
    private EnemyHealth _enemyHealth;
    private BaseEnemy _baseEnemy;
    private float _targetFillAmount = 1f;
    private void Awake()
    {
        _target = transform.root;

        _enemyHealth = GetComponentInParent<EnemyHealth>();
        _baseEnemy = GetComponentInParent<BaseEnemy>();
        if (_enemyHealth != null)
        {
            _enemyHealth.OnHealthChanged += UpdateHealthBar;
        }
        transform.forward = Camera.main.transform.forward;
        // 처음에 비활성화;
        _helathBarSprite.gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        if (_target == null) return;

        // 플레이어가 범위 안에 있으면 보이게하기
        if (!_baseEnemy.PlayerInRange())
        {
            _helathBarSprite.gameObject.SetActive(false);
            return;
        }

        // 플레이어가 범위 밖에 있으면 안보이게하기
        _helathBarSprite.gameObject.SetActive(true);

        // 위치 맞추기
        transform.position = _pivot != null ? _pivot.position : _target.position;
        // 회전 따라가기 
        transform.forward = Camera.main.transform.forward;

        _healthBarForegroundSprite.fillAmount = Mathf.MoveTowards(_healthBarForegroundSprite.fillAmount, _targetFillAmount, Time.deltaTime * _speed);
    }


    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _targetFillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }
}
