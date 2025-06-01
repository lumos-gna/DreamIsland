using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private Animator _damageAnimator;
    [SerializeField] private GameObject _damageText;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _healthBarForegroundSprite;
    [SerializeField] private Image _helathBarSprite;
    [SerializeField] private float _speed = 3f;
    private Transform _target;
    private EnemyHealth _enemyHealth;
    private BaseEnemy _baseEnemy;
    private float _targetFillAmount = 1f;
    private bool _isDectect = false;
    public Transform GetPivot() => _pivot;
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

        if (_baseEnemy.PlayerInRange())
        {
            _isDectect = true;
        }

        _helathBarSprite.gameObject.SetActive(_isDectect);

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

    public void DamageText(int damage)
    {
        Vector3 pos = _pivot.position + new Vector3(0f, 2f, Random.Range(-0.2f, 0.2f));

        // 프리팹 생성
        GameObject go = Instantiate(_damageText, _canvas.transform);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.position = pos;

        // 텍스트 설정
        var tmp = go.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = damage.ToString();
        }

        // 애니메이션 실행
        Animator anim = go.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("isFloat");
        }

        // 일정 시간 후 파괴
        //Destroy(go, 1.2f);
    }
}