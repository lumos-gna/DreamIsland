using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
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

    // 낮/밤 여부를 나타내는 static 프로퍼티
    public static bool IsDay { get; private set; }

    [Header("Audio Settings")]
    public int nightBgmIndex = 3;            
    public int forestDayBgmIndex = 0;         
    public int desertDayBgmIndex = 2;           
    public int arcticDayBgmIndex = 1;           

    [Header("Events")]
    public CycleEvent OnCycleComplete;

    private float timer;
    private bool _wasDay;

    //null 체크
    private RegionManager _regionManager;

    void Awake()
    {
        // RegionManager를 찾아서 이벤트 등록
        _regionManager = FindObjectOfType<RegionManager>();
        if (_regionManager != null)
            _regionManager.OnRegionChanged += OnRegionChanged;

        // null이면 새로 생성
        if (OnCycleComplete == null)
            OnCycleComplete = new CycleEvent();

        //  nightBgmIndex = 3
        nightBgmIndex = 3;
    }

    void Start()
    {
        // 시작 시각 설정
        timer = startTimeOfDay * dayDuration;
        ApplyCycle(timer);

        _wasDay = IsDay;
        PlayCurrentBGM();
    }

    void Update()
    {
        // 시간 증가
        timer += Time.deltaTime;
        if (timer > dayDuration)
        {
            timer -= dayDuration;
            // 사이클이 완료될 때마다 이벤트 발동
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

        // 낮→밤 혹은 밤→낮으로 전환될 때 BGM 전환
        if (isDay != _wasDay)
        {
            _wasDay = isDay;
            PlayCurrentBGM();
        }

        // 태양/달 조명 방향 및 밝기 설정
        SunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        MoonLight.transform.rotation = Quaternion.Euler(sunAngle + 180f, 170f, 0f);
        SunLight.intensity = isDay ? daySunIntensity : 0f;
        MoonLight.intensity = isDay ? 0f : nightMoonIntensity;
        RenderSettings.skybox = isDay ? Sun : Moon;

        // null 체크 후 currentRegion 사용
        float regionMin, regionMax;
        if (_regionManager != null)
        {
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
        }
        else
        {
            // RegionManager가 없으면 숲(Forest) 설정 값 사용
            regionMin = forestMinTemp;
            regionMax = forestMaxTemp;
        }

        // 낮과 밤 온도 산출 로직
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
        // 지역이 바뀔 때, 낮인 상태라면 즉시 해당 지역 낮 BGM으로 전환
        if (IsDay)
            PlayDayBGMForRegion(newRegion);
    }

    private void PlayCurrentBGM()
    {
        int idx = IsDay
            ? (_regionManager != null
               ? _regionManager.currentRegion switch
               {
                   Region.Forest => forestDayBgmIndex,
                   Region.Desert => desertDayBgmIndex,
                   Region.Arctic => arcticDayBgmIndex,
                   _ => forestDayBgmIndex
               }
               : forestDayBgmIndex)  // _regionManager가 null이면 기본 숲 음악
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

    // (기존 코드와 동일) 현재 지역에 따른 최소/최대 온도를 구하는 프로퍼티
    public float RegionMinTemperature
    {
        get
        {
            return _regionManager != null
                ? _regionManager.currentRegion switch
                {
                    Region.Desert => desertMinTemp,
                    Region.Arctic => arcticMinTemp,
                    _ => forestMinTemp
                }
                : forestMinTemp;
        }
    }

    public float RegionMaxTemperature
    {
        get
        {
            return _regionManager != null
                ? _regionManager.currentRegion switch
                {
                    Region.Desert => desertMaxTemp,
                    Region.Arctic => arcticMaxTemp,
                    _ => forestMaxTemp
                }
                : forestMaxTemp;
        }
    }
}
