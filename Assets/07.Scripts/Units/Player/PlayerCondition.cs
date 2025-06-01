using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float water = 100f;
    [SerializeField] private float stamina = 100f;

    [SerializeField] private float redTemperature = 0f;   // 과열 게이지
    [SerializeField] private float blueTemperature = 0f;  // 저체온 게이지

    public float Health => health;
    public float Water => water;
    public float Stamina => stamina;
    public float RedTemperature => redTemperature;
    public float BlueTemperature => blueTemperature;

    private float waterDecreaseperFrame = 0.001f;
    private float thirstyDecreaseHealth = 0.1f;
    private float StaminaDecreasePerFrame = 0.001f;

    private float minf = 0f;
    private float maxf = 100f;

    private void Update()
    {
        // 목마름 자동 감소
        WaterChange(-waterDecreaseperFrame);
        if (water == minf)
        {
            HealthChange(-thirstyDecreaseHealth);
        }

        // 스태미너 자동 감소
        StaminaChange(-StaminaDecreasePerFrame);
        if (stamina <= 0f)
        {
            HealthChange(-5f * Time.deltaTime); // 스태미너가 0이 되면 HP 감소
        }

        float envTemp = DayNightCycle.CurrentTemperature;

        // 온도 22도 이상이면 빨간 게이지 상승
        if (envTemp > 22f)
            RedTempChange((envTemp - 22f) * Time.deltaTime);
        else
            RedTempChange(-10f * Time.deltaTime); // 자연회복

        // 온도 13도 이하이면 파란 게이지 상승
        if (envTemp < 13f)
            BlueTempChange((13f - envTemp) * Time.deltaTime);
        else
            BlueTempChange(-10f * Time.deltaTime); // 자연회복

        redTemperature = Mathf.Clamp(redTemperature, minf, maxf);
        blueTemperature = Mathf.Clamp(blueTemperature, minf, maxf);

        // 파란 게이지 100%면 HP 감소
        if (blueTemperature >= 100f)
            HealthChange(-10f * Time.deltaTime);

        // 빨간 게이지 100%면 Water 감소, Water가 0이 되면 HP도 감소
        if (redTemperature >= 100f)
            WaterChange(-10f * Time.deltaTime);

        if (water <= 0f)
            HealthChange(-5f * Time.deltaTime);
        
        if(health <= 0f) // 체력이 0이되면 게임 오버
        {
            GameManager.Instance.GameOver();
        }
    }

    public void HealthChange(float change)
    {
        if(change < 0f)
        {
            UIManager.Instance.Get<FlashUI>()?.PlayFlash();
        }
        health = Mathf.Clamp(health + change, minf, maxf);
    }
    public void WaterChange(float change) => water = Mathf.Clamp(water + change, minf, maxf);
    public void RedTempChange(float change) => redTemperature = Mathf.Clamp(redTemperature + change, minf, maxf);
    public void BlueTempChange(float change) => blueTemperature = Mathf.Clamp(blueTemperature + change, minf, maxf);
    public void StaminaChange(float change) => stamina = Mathf.Clamp(stamina + change, minf, maxf);
    
}