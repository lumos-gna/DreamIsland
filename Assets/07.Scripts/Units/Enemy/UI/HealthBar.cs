using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarSprite;
    [SerializeField] private Vector3 _offset = new Vector3(0, -1f, 0);
    [SerializeField] private float _speed = 3f;
    private Transform _target;
    private EnemyHealth _enemyHealth;
    private Collider _collider;
    private float _targetFillAmount = 1f;
    private void Awake()
    {
        _target = transform.root;

        _enemyHealth = GetComponentInParent<EnemyHealth>();
        _collider = GetComponentInParent<Collider>();
        if (_enemyHealth != null)
        {
            _enemyHealth.OnHealthChanged += UpdateHealthBar;
        }
    }
    void LateUpdate()
    {
        if (_target == null) return;

        // 위치 맞추기
        transform.position = _target.transform.position + _offset;
        // 회전 따라가기 
        transform.rotation = _target.rotation;

        _healthBarSprite.fillAmount = Mathf.MoveTowards(_healthBarSprite.fillAmount, _targetFillAmount, Time.deltaTime * _speed);
    }


    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _targetFillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }
}
