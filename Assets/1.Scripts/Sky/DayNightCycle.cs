using UnityEngine;
using System;

public class DayNightCycle : MonoBehaviour
{
    [Header("References")]
    public Light SunLight;     
    public Light MoonLight;     
    public Material Sun;       
    public Material Moon;      

    [Header("Cycle Settings")]
    public float dayDuration = 120f;

    [Header("Light Intensity")]
    public float daySunIntensity = 1f;
    public float nightMoonIntensity = 0.3f;

    [Header("Temperature Settings")]
    // 최저 온도(밤)
    public float minTemperature = 10f;
    // 최고 온도(정오)
    public float maxTemperature = 25f;

    public static float CurrentTemperature { get; private set; }
    // 온도 빼가실때 "float currentTemp = DayNightCycle.CurrentTemperature;" 이걸로 빼가시면 됩니다.

    public static event Action OnCycleComplete;

    private float timer = 0f;

    void Start()
    {
        RenderSettings.skybox = Sun;
        SunLight.intensity = daySunIntensity;
        MoonLight.intensity = 0f;
        MoonLight.enabled = true;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > dayDuration)
        {
            timer -= dayDuration;
            OnCycleComplete?.Invoke();
        }

        float t = timer / dayDuration;
        float sunAngle = Mathf.Lerp(-90f, 270f, t);

        SunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        MoonLight.transform.rotation = Quaternion.Euler(sunAngle + 180f, 170f, 0f);

        bool isDay = sunAngle > 0f && sunAngle < 180f;

        SunLight.intensity = isDay ? daySunIntensity : 0f;
        MoonLight.intensity = isDay ? 0f : nightMoonIntensity;

        RenderSettings.skybox = isDay ? Sun : Moon;

        DynamicGI.UpdateEnvironment();

        // 온도 계산 (해각도 기준으로)
        float normalizedDay = isDay
            ? Mathf.InverseLerp(0f, 180f, sunAngle)
            : 0f;
        CurrentTemperature = Mathf.Lerp(minTemperature, maxTemperature, normalizedDay);


    }
}
