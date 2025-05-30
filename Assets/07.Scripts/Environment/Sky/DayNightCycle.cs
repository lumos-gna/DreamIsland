using UnityEngine;
using System;
using UnityEngine.Events;

public class CycleEvent : UnityEvent { }
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
    [Range(0f, 1f)]
    public float startTimeOfDay = 0.25f;

    [Header("Light Intensity")]
    public float daySunIntensity = 1f;
    public float nightMoonIntensity = 0.3f;

    [Header("Temperature Settings")]
    public float minTemperature = 10f;
    public float maxTemperature = 25f;
    public static float CurrentTemperature { get; private set; }

    // ≥∑/π„ ªÛ≈¬
    public static bool IsDay { get; private set; }

    [Header("Audio Settings")]
    public int nightBgmIndex = 3;                // ∞¯≈Î π„ BGM ¿Œµ¶Ω∫
    public int forestDayBgmIndex = 0;            // Ω£ ≥∑ BGM ¿Œµ¶Ω∫
    public int desertDayBgmIndex = 2;            // ªÁ∏∑ ≥∑ BGM ¿Œµ¶Ω∫
    public int arcticDayBgmIndex = 1;            // ∫œ±ÿ ≥∑ BGM ¿Œµ¶Ω∫

    [Header("Events")]
    public CycleEvent OnCycleComplete;

    private float timer;
    private bool _wasDay;
    private RegionManager _regionManager;

    void Awake()
    {
        _regionManager = FindObjectOfType<RegionManager>();
        if (_regionManager != null)
            _regionManager.OnRegionChanged += OnRegionChanged;

        nightBgmIndex = 3;
    }

    void Start()
    {
        timer = startTimeOfDay * dayDuration;
        ApplyCycle(timer);

        _wasDay = IsDay;
        PlayCurrentBGM();
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

        bool isDay = sunAngle >= 0f && sunAngle <= 180f;
        IsDay = isDay;

        if (isDay != _wasDay)
        {
            _wasDay = isDay;
            PlayCurrentBGM();
        }

        // ∫˚/Ω∫ƒ´¿Ãπ⁄Ω∫/ø¬µµ ±‚¡∏ ∑Œ¡˜
        SunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        MoonLight.transform.rotation = Quaternion.Euler(sunAngle + 180f, 170f, 0f);
        SunLight.intensity = isDay ? daySunIntensity : 0f;
        MoonLight.intensity = isDay ? 0f : nightMoonIntensity;
        RenderSettings.skybox = isDay ? Sun : Moon;

        float normalizedDay = isDay
            ? Mathf.InverseLerp(0f, 180f, sunAngle)
            : 0f;
        CurrentTemperature = Mathf.Lerp(minTemperature, maxTemperature, normalizedDay);
        DynamicGI.UpdateEnvironment();
    }

    private void OnRegionChanged(Region newRegion)
    {
        // ¡ˆø™ ¿Ãµø Ω√ø°µµ ≥∑ ªÛ≈¬∂Û∏È ¡ÔΩ√ «ÿ¥Á ¡ˆø™ ≥∑ BGM¿∏∑Œ ¿¸»Ø
        if (IsDay)
            PlayDayBGMForRegion(newRegion);
    }

    private void PlayCurrentBGM()
    {
        int idx = IsDay
        ? (_regionManager.currentRegion switch
        {
            Region.Forest => forestDayBgmIndex,
            Region.Desert => desertDayBgmIndex,
            Region.Arctic => arcticDayBgmIndex,
            _ => forestDayBgmIndex
        })
        : nightBgmIndex;

        Debug.Log($"[Audio] {(IsDay ? "Day" : "Night")} playing BGM index {idx}");
        AudioManager.PlayBackgroundMusic(idx, true);
    }

    private void PlayDayBGMForRegion(Region region)
    {
        int idx = region switch
        {
            Region.Forest => forestDayBgmIndex,
            Region.Desert => desertDayBgmIndex,
            Region.Arctic => arcticDayBgmIndex,
            _ => forestDayBgmIndex
        };
        AudioManager.PlayBackgroundMusic(idx, true);
    }

    void OnDestroy()
    {
        if (_regionManager != null)
            _regionManager.OnRegionChanged -= OnRegionChanged;
    }
}