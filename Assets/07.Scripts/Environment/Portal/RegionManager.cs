using System;
using UnityEngine;

public enum Region { Forest, Desert, Arctic }

public class RegionManager : MonoBehaviour
{
    [Header("Map Roots")]
    public GameObject forestMap;
    public GameObject desertMap;
    public GameObject arcticMap;

    [Header("Spawn Points")]
    public Transform forestSpawnPoint;
    public Transform desertSpawnPoint;
    public Transform arcticSpawnPoint;

    [Header("Player")]
    public Transform playerTransform;

    [Header("Start Region")]
    public Region initialRegion = Region.Forest;

    public Region currentRegion { get; private set; }
    public event Action<Region> OnRegionChanged;

    void Start()
    {
        ChangeRegion(initialRegion, skipTeleport: true);
    }

    public void ChangeRegion(Region r, bool skipTeleport = false)
    {
        currentRegion = r;

        // 플레이어 텔레포트
        if (!skipTeleport)
            playerTransform.position = GetSpawnPoint(r).position;

        OnRegionChanged?.Invoke(r);
    }

    public Transform GetSpawnPoint(Region r)
    {
        return r switch
        {
            Region.Forest => forestSpawnPoint,
            Region.Desert => desertSpawnPoint,
            Region.Arctic => arcticSpawnPoint,
            _ => forestSpawnPoint
        };
    }
}
