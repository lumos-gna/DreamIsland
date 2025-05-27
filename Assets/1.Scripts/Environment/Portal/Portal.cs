using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    [Header("References")]
    public RegionManager regionManager;
    public Region targetRegion;

    [Header("Preview")]
    public Camera previewCamera;
    public Renderer screenRenderer;

    [Header("Teleport")]
    public float cooldown = 2f;
    private bool canTeleport = true;

    [Header("Player")]
    public Transform playerTransform;

    void Awake()
    {
        // 1) PreviewCamera 자동 할당
        if (previewCamera == null)
        {
            var camTransform = transform.Find("PreviewCamera");
            if (camTransform != null)
                previewCamera = camTransform.GetComponent<Camera>();

        }

        // 2) ScreenRenderer 자동 할당
        if (screenRenderer == null)
        {
            var screenTransform = transform.Find("Screen");
            if (screenTransform != null)
                screenRenderer = screenTransform.GetComponent<Renderer>();

        }

        // 3) PlayerTransform 자동 할당 (태그가 Player인 오브젝트)
        if (playerTransform == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canTeleport) return;
        if (!other.CompareTag("Player")) return;

        StartCoroutine(TeleportCoroutine());
    }

    IEnumerator TeleportCoroutine()
    {
        canTeleport = false;

        // 1) 메인 카메라에 보일 레이어만 바꿔준다
        regionManager.SetMainCameraRegion(targetRegion);

        // 2) 플레이어 위치 이동
        playerTransform.position = regionManager.GetSpawnPoint(targetRegion).position;

        // (선택) 페이드 인 등 이펙트
        yield return new WaitForSeconds(cooldown);
        canTeleport = true;
    }
}