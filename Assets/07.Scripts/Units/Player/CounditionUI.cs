using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : BaseUI
{
    [Header("References")]
    [Tooltip("PlayerCondition 컴포넌트가 붙은 오브젝트")]
    [SerializeField] private PlayerCondition _playerCondition;

    [Header("UI Prefabs (Resources/UI 폴더에 있어야 합니다)")]
    [Tooltip("체력바 프리팹")]
    [SerializeField] private GameObject _healthBarPrefab;
    [Tooltip("온도 빨강 게이지 프리팹")]
    [SerializeField] private GameObject _redGaugePrefab;
    [Tooltip("온도 파랑 게이지 프리팹")]
    [SerializeField] private GameObject _blueGaugePrefab;

    // 인스턴스화된 실제 UI
    private HealthBarUI _healthBarUI;
    private TemperatureGaugeUI _redGaugeUI;
    private TemperatureGaugeUI _blueGaugeUI;

    // BaseUI 에서 상속된 필드
    // 이 UI는 HUD 타입입니다.
    private void Reset() => UIType = UIType.HUD;

    public override void Init()
    {
        // 1) PlayerCondition 자동 할당
        if (_playerCondition == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null)
                _playerCondition = p.GetComponent<PlayerCondition>();
            if (_playerCondition == null)
                Debug.LogError("ConditionUI ▶ PlayerCondition 을 찾지 못했습니다!");
        }

        // 2) 이 스크립트가 붙은 GameObject 는
        //    UIManager.Create<ConditionUI>() 시 Canvas 밑에 생성됩니다.
        var parent = (transform as RectTransform);

        // 3) 체력바 인스턴스화
        var hb = Instantiate(_healthBarPrefab, parent, false);
        _healthBarUI = hb.GetComponent<HealthBarUI>();
        // HealthBarUI 에 PlayerCondition 을 넣어 주는 메서드
        _healthBarUI.Initialize(_playerCondition);
        // RectTransform 세팅 (오른쪽 아래)
        var hbRT = hb.GetComponent<RectTransform>();
        hbRT.anchorMin = hbRT.anchorMax = new Vector2(1, 0);
        hbRT.pivot = new Vector2(1, 0);
        hbRT.anchoredPosition = new Vector2(-10, 10);

        // 4) 빨강 게이지
        var rg = Instantiate(_redGaugePrefab, parent, false);
        _redGaugeUI = rg.GetComponent<TemperatureGaugeUI>();
        _redGaugeUI.Initialize(isRed: true);
        var rgRT = rg.GetComponent<RectTransform>();
        rgRT.anchorMin = rgRT.anchorMax = new Vector2(1, 0);
        rgRT.pivot = new Vector2(1, 0);
        rgRT.anchoredPosition = new Vector2(-10, 40);

        // 5) 파랑 게이지
        var bg = Instantiate(_blueGaugePrefab, parent, false);
        _blueGaugeUI = bg.GetComponent<TemperatureGaugeUI>();
        _blueGaugeUI.Initialize(isRed: false);
        var bgRT = bg.GetComponent<RectTransform>();
        bgRT.anchorMin = bgRT.anchorMax = new Vector2(1, 0);
        bgRT.pivot = new Vector2(1, 0);
        bgRT.anchoredPosition = new Vector2(-10, 70);
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }
}
