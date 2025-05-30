using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : BaseUI
{
    [Header("게이지 이미지")]
    [SerializeField] private Image barHP;
    [SerializeField] private Image barRed;
    [SerializeField] private Image barBlue;

    private void Awake()
    {
        UIType = UIType.HUD;  // 필드에 직접 할당
    }

    public override void Init()
    {
        SetHP(100f);
        SetRed(0f);
        SetBlue(0f);
    }

    public override void Enable() => gameObject.SetActive(true);
    public override void Disable() => gameObject.SetActive(false);

    public void SetHP(float value) => barHP.fillAmount = Mathf.Clamp01(value / 100f);
    public void SetRed(float value) => barRed.fillAmount = Mathf.Clamp01(value / 100f);
    public void SetBlue(float value) => barBlue.fillAmount = Mathf.Clamp01(value / 100f);
}
