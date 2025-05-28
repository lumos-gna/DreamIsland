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

    Camera mainCam;
    int defaultMask;

    void Start()
    {
        mainCam = Camera.main;
        playerTransform.position = GetSpawnPoint(initialRegion).position;
    }

    public void SetMainCameraRegion(Region r)
    {
        mainCam.cullingMask = -1;   // ¶Ç´Â mainCam.cullingMask = -1;
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