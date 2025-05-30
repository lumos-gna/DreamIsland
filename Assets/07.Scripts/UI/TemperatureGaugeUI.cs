using UnityEngine;
using UnityEngine.UI;  

[RequireComponent(typeof(Image))]
public class TemperatureGaugeUI : MonoBehaviour
{
    private Image _fill;
    private bool _isRed;
    private DayNightCycle _cycle;   

    public void Initialize(bool isRed)
    {
        _isRed = isRed;

        // 이 컴포넌트가 붙어있는 Image 확보
        _fill = GetComponent<Image>();
        _fill.type = Image.Type.Filled;
        _fill.fillMethod = Image.FillMethod.Horizontal;
        _fill.fillOrigin = (int)Image.OriginHorizontal.Left;

        // 씬에 존재하는 DayNightCycle 오브젝트 하나를 찾아서 참조
        _cycle = FindObjectOfType<DayNightCycle>();
        if (_cycle == null)
            Debug.LogError("씬에 DayNightCycle이 없습니다!");
    }

    void Update()
    {
        if (_cycle == null) return;

        // 현재 온도 (static 프로퍼티)
        float temp = DayNightCycle.CurrentTemperature;

        // 인스턴스 멤버로 바꿔서 사용
        float t = Mathf.InverseLerp(
            _cycle.RegionMinTemperature,
            _cycle.RegionMaxTemperature,
            temp
        );

        // isRed이면 t만큼 차오르고, 파랑쪽이면 (1-t)
        _fill.fillAmount = _isRed ? t : 1f - t;
    }
}
