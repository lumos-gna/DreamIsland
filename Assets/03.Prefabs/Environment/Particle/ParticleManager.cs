using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Header("RegionManager 참조 (없으면 자동 할당)")]
    public RegionManager regionManager;

    [Header("각 Region별 파티클 오브젝트")]
    public GameObject forestParticles;
    public GameObject desertParticles;
    public GameObject arcticParticles;

    void Awake()
    {
        if (regionManager == null)
            regionManager = FindObjectOfType<RegionManager>();

        // 이벤트 구독
        if (regionManager != null)
            regionManager.OnRegionChanged += HandleRegionChanged;
    }

    void Start()
    {
        // 시작 시 initialRegion에 맞춰 파티클 설정
        if (regionManager != null)
            HandleRegionChanged(regionManager.currentRegion);
    }

    void HandleRegionChanged(Region r)
    {
        if (forestParticles != null) forestParticles.SetActive(r == Region.Forest);
        if (desertParticles != null) desertParticles.SetActive(r == Region.Desert);
        if (arcticParticles != null) arcticParticles.SetActive(r == Region.Arctic);
    }

    void OnDestroy()
    {
        if (regionManager != null)
            regionManager.OnRegionChanged -= HandleRegionChanged;
    }
}
