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
    public float forestMinTemp = 18f;
    public float forestMaxTemp = 24f;
    public float desertMinTemp = 20f;
    public float desertMaxTemp = 40f;
    public float arcticMinTemp = -10f;
    public float arcticMaxTemp = 15f;
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

        // ±§ø¯/Ω∫ƒ´¿Ãπ⁄Ω∫ º≥¡§
        SunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        MoonLight.transform.rotation = Quaternion.Euler(sunAngle + 180f, 170f, 0f);
        SunLight.intensity = isDay ? daySunIntensity : 0f;
        MoonLight.intensity = isDay ? 0f : nightMoonIntensity;
        RenderSettings.skybox = isDay ? Sun : Moon;

        float regionMin, regionMax;
        switch (_regionManager.currentRegion)
        {
            case Region.Desert:
                regionMin = desertMinTemp;
                regionMax = desertMaxTemp;
                break;
            case Region.Arctic:
                regionMin = arcticMinTemp;
                regionMax = arcticMaxTemp;
                break;
            case Region.Forest:
            default:
                regionMin = forestMinTemp;
                regionMax = forestMaxTemp;
                break;
        }

        // ≥∑ø°¥¬ Sine ∞Óº±, π„ø°¥¬ ¿‹ø≠ ∑Œ¡˜ ±◊¥Î∑Œ
        float normalizedDay = Mathf.InverseLerp(0f, 180f, Mathf.Clamp(sunAngle, 0f, 180f));
        float dayTempCurve = Mathf.Sin(normalizedDay * Mathf.PI);

        float nightBlendSpeed = 0.3f;
        if (isDay)
        {
            CurrentTemperature = Mathf.Lerp(regionMin, regionMax, dayTempCurve);
        }
        else
        {
            CurrentTemperature = Mathf.Lerp(CurrentTemperature, regionMin, Time.deltaTime * nightBlendSpeed);
        }

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

    public float RegionMinTemperature
    {
        get
        {
            return _regionManager.currentRegion switch
            {
                Region.Desert => desertMinTemp,
                Region.Arctic => arcticMinTemp,
                _ => forestMinTemp,
            };
        }
    }
    public float RegionMaxTemperature
    {
        get
        {
            return _regionManager.currentRegion switch
            {
                Region.Desert => desertMaxTemp,
                Region.Arctic => arcticMaxTemp,
                _ => forestMaxTemp,
            };
        }
    }


}