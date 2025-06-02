using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TemperatureGaugeUI : MonoBehaviour
{
    private Image _fill;
    private DayNightCycle _cycle;
    
    private bool _isRed;

    public void Initialize(bool isRed)
    {
        _isRed = isRed;

        // �� ������Ʈ�� �پ��ִ� Image Ȯ��
        _fill = GetComponent<Image>();
        _fill.type = Image.Type.Filled;
        _fill.fillMethod = Image.FillMethod.Horizontal;
        _fill.fillOrigin = (int)Image.OriginHorizontal.Left;

        // ���� �����ϴ� DayNightCycle ������Ʈ �ϳ��� ã�Ƽ� ����
        _cycle = FindObjectOfType<DayNightCycle>();
        if (_cycle == null)
            Debug.LogError("���� DayNightCycle�� �����ϴ�!");
    }

    void Update()
    {
        if (_cycle == null) return;

        // ���� �µ� (static ������Ƽ)
        float temp = DayNightCycle.CurrentTemperature;

        // �ν��Ͻ� ����� �ٲ㼭 ���
        float t = Mathf.InverseLerp(
            _cycle.RegionMinTemperature,
            _cycle.RegionMaxTemperature,
            temp
        );

        // isRed�̸� t��ŭ ��������, �Ķ����̸� (1-t)
        _fill.fillAmount = _isRed ? t : 1f - t;
    }
}
