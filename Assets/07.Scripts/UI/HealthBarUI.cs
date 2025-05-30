using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBarUI : MonoBehaviour
{
    private PlayerCondition _pc;
    private Image _fill;

    public void Initialize(PlayerCondition pc)
    {
        _pc = pc;
        _fill = GetComponent<Image>();
        _fill.type = Image.Type.Filled;
        _fill.fillMethod = Image.FillMethod.Horizontal;
        _fill.fillOrigin = (int)Image.OriginHorizontal.Left;
    }

    void Update()
    {
        if (_pc == null) return;
        float ratio = Mathf.Clamp01(_pc.Health / 100f);
        _fill.fillAmount = ratio;
    }
}
