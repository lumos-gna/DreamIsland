using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private GameObject _damageText;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _healthBarForegroundSprite;
    [SerializeField] private Image _helathBarSprite;
    [SerializeField] private float _speed = 3f;

    private Transform _player;
    private Transform _target;
    private DestructibleObject _destructibleObject;
    private BaseEnemy _baseEnemy;
    
    private float _targetFillAmount = 1f;
    private bool _isDectect = false;

    public void SetIsDectect(bool isDect)
    {
        _isDectect = isDect;
    }
    private void Awake()
    {
        _target = transform.root;
        _destructibleObject = GetComponentInParent<DestructibleObject>();
        _baseEnemy = GetComponentInParent<BaseEnemy>();

        if (_destructibleObject != null)
        {
            _destructibleObject.OnHealthChanged += UpdateHealthBar;
        }

        PlayerController player = FindAnyObjectByType<PlayerController>();

        if (player == null)
        {
            Debug.LogError("플레이어가 없다~~");
            return;
        }

        _player = player.transform;

        transform.forward = Camera.main.transform.forward;

        // 처음에 비활성화;
        _helathBarSprite.gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        if (_target == null) return;

        if (_baseEnemy != null && _baseEnemy.PlayerInRange())
        {
            _isDectect = true;
        }
        else
        {
            if (_player != null)
            {
                float detectRange = 5f;
                float distance = Vector3.Distance(_target.position, _player.position);
                _isDectect = distance <= detectRange;
            }
        }

        _helathBarSprite.gameObject.SetActive(_isDectect);

        // 위치 맞추기
        transform.position = _pivot != null ? _pivot.position : _target.position;
        // 회전 따라가기 
        Vector3 dir = (transform.position - Camera.main.transform.position).normalized;
        transform.forward = dir;

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