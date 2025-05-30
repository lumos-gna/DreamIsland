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

    // 낮/밤 상태
    public static bool IsDay { get; private set; }

    [Header("Audio Settings")]
    public int nightBgmIndex = 3;                // 공통 밤 BGM 인덱스
    public int forestDayBgmIndex = 0;            // 숲 낮 BGM 인덱스
    public int desertDayBgmIndex = 2;            // 사막 낮 BGM 인덱스
    public int arcticDayBgmIndex = 1;            // 북극 낮 BGM 인덱스

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

        // 광원/스카이박스 설정
        SunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        MoonLight.transform.rotation = Quaternion.Euler(sunAngle + 180f, 170f, 0f);
        SunLight.intensity = isDay ? daySunIntensity : 0f;
        MoonLight.intensity = isDay ? 0f : nightMoonIntensity;
        RenderSettings.skybox = isDay ? Sun : Moon;

        // 온도 변화 - 낮에는 곡선, 밤에는 잔열 보정
        float normalizedDay = Mathf.InverseLerp(0f, 180f, Mathf.Clamp(sunAngle, 0f, 180f));
        float dayTempCurve = Mathf.Sin(normalizedDay * Mathf.PI); // 0~1~0 Sine 곡선

        // 밤에도 온도 자연스럽게 식게 잔열 처리
        float nightBlendSpeed = 0.3f; // 0~1, 값이 작을수록 밤에 서서히 식음

        if (isDay)
        {
            // 낮에는 곡선 따라 상승
            CurrentTemperature = Mathf.Lerp(minTemperature, maxTemperature, dayTempCurve);
        }
        else
        {
            // 밤에는 온도가 곧바로 떨어지지 않고, 천천히 min 쪽
            // 이전 프레임 값을 유지하면서 점진적으로 minTemperature로 감소
            CurrentTemperature = Mathf.Lerp(CurrentTemperature, minTemperature, Time.deltaTime * nightBlendSpeed);
        }

        DynamicGI.UpdateEnvironment();
    }


    private void OnRegionChanged(Region newRegion)
    {
        // 지역 이동 시에도 낮 상태라면 즉시 해당 지역 낮 BGM으로 전환
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