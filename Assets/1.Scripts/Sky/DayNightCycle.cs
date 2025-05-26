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

    [Header("Start Time")]
    // 0: 자정, 0.25: 일출(낮 시작), 0.5: 정오, 0.75: 일몰
    [Range(0f, 1f)]
    public float startTimeOfDay = 0.25f; // 일출부터 시작

    [Header("Light Intensity")]
    public float daySunIntensity = 1f;
    public float nightMoonIntensity = 0.3f;

    [Header("Temperature Settings")]
    public float minTemperature = 10f;
    public float maxTemperature = 25f;
    public static float CurrentTemperature { get; private set; }
    // 온도 빼가실때 "float currentTemp = DayNightCycle.CurrentTemperature;" 이걸로 빼가시면 됩니다.

    public static event Action OnCycleComplete;
    private float timer;

    void Start()
    {
        timer = startTimeOfDay * dayDuration;

        ApplyCycle(timer);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > dayDuration)
        {
            timer -= dayDuration;
            OnCycleComplete?.Invoke();
        }

        ApplyCycle(timer);
    }

    void ApplyCycle(float currentTime)
    {
        float t = currentTime / dayDuration;                 
        float sunAngle = Mathf.Lerp(-90f, 270f, t);         

        // 회전
        SunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        MoonLight.transform.rotation = Quaternion.Euler(sunAngle + 180f, 170f, 0f);

        // 낮/밤 판정
        bool isDay = sunAngle > 0f && sunAngle < 180f;

        // 밝기
        SunLight.intensity = isDay ? daySunIntensity : 0f;
        MoonLight.intensity = isDay ? 0f : nightMoonIntensity;

        RenderSettings.skybox = isDay ? Sun : Moon;

        // 온도 계산
        float normalizedDay = isDay
            ? Mathf.InverseLerp(0f, 180f, sunAngle)
            : 0f;
        CurrentTemperature = Mathf.Lerp(minTemperature, maxTemperature, normalizedDay);

        DynamicGI.UpdateEnvironment();
    }
}
