using DG.Tweening;
using UnityEngine;

public class Alter : MonoBehaviour, IInteractable
{
    [Header("포탈 및 이펙트")]
    public GameObject fireEffect;
    public GameObject portalPrefab;
    public Transform portalPosition;

    public Outline Outline { get; private set; }

    [Header("카메라 이동")]
    public Transform cameraContainer;
    public Transform moveTarget1;
    public Transform moveTarget2;
    public float moveDuration = 0.5f;
    public float lookDuration = 1.5f;
    public float finalMoveDuration = 1f;

    [Header("레터박스")]
    public RectTransform topLetterbox;
    public RectTransform bottomLetterbox;

    [SerializeField] private ItemData targetItemData;
    [SerializeField] private int targetItemCount;

    private Inventory _inventory;
    private GameObject _spawnedPortal;
    private GameObject _playerCamera;

    void Awake()
    {
        Outline = GetComponent<Outline>();
        _inventory = GameManager.Instance.Inventory;
    }

    void Start()
    {
        var player = GameObject.Find("Player");
        var child = player.transform.Find("CamerContainer");
        if (child != null)
            _playerCamera = child.gameObject;
    }

    public void OnInteract()
    {
        if (_inventory.FindSlot(slot => slot.item == targetItemData && slot.quantity >= targetItemCount) != null)
        {
            fireEffect.SetActive(true);
            GameManager.Instance.OnOffEquipCamera(false);

            // 포탈 생성
            if (_spawnedPortal == null)
            {
                _spawnedPortal = Instantiate(portalPrefab, portalPosition.position, portalPosition.rotation, portalPosition);
                _spawnedPortal.SetActive(false); // 애니메이션 시작 전 숨김

                // 포탈의 목표 지역 자동 지정
                var portalComponent = _spawnedPortal.GetComponent<Portal>();
                if (portalComponent != null)
                {
                    var currentRegion = FindObjectOfType<RegionManager>().currentRegion;
                    portalComponent.regionManager = FindObjectOfType<RegionManager>();
                    portalComponent.playerTransform = GameObject.FindWithTag("Player").transform;

                    switch (currentRegion)
                    {
                        case Region.Forest:
                            portalComponent.targetRegion = Region.Arctic;
                            break;
                        case Region.Arctic:
                            portalComponent.targetRegion = Region.Desert;
                            break;
                        case Region.Desert:
                            // 마지막 Desert에서는 EndingPortal 생성!
                            Destroy(_spawnedPortal); // 기존 포탈 삭제
                            _spawnedPortal = Instantiate(Resources.Load<GameObject>("EndingPortal"), portalPosition.position, portalPosition.rotation);
                            return;
                    }
                }
            }

            StartCameraEvent();
            GameManager.Instance.OnOffEquipCamera(true);
        }
    }

    private void StartCameraEvent()
    {
        if (cameraContainer == null || moveTarget1 == null || moveTarget2 == null || portalPosition == null || _playerCamera == null) return;
        if (topLetterbox == null || bottomLetterbox == null) return;

        GameManager.Instance.SetCursorLockState(false);
        GameManager.Instance.OnOffEquipCamera(false);

        cameraContainer.position = moveTarget1.position;
        Vector3 dir = (portalPosition.position - cameraContainer.position).normalized;
        cameraContainer.rotation = Quaternion.LookRotation(dir);

        Sequence letterboxSeq = DOTween.Sequence();
        letterboxSeq.Append(topLetterbox.DOAnchorPosY(-50, moveDuration).SetEase(Ease.InOutSine));
        letterboxSeq.Join(bottomLetterbox.DOAnchorPosY(50, moveDuration).SetEase(Ease.InOutSine));

        letterboxSeq.OnComplete(() =>
        {
            cameraContainer.DOMove(moveTarget2.position, moveDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    if (_spawnedPortal != null)
                    {
                        _spawnedPortal.SetActive(true);
                        _spawnedPortal.transform.localPosition = new Vector3(0, -10f, 0);
                        _spawnedPortal.transform.DOLocalMoveY(0f, finalMoveDuration)
                            .SetEase(Ease.OutSine)
                            .OnComplete(() =>
                            {
                                topLetterbox.anchoredPosition = new Vector2(0, 100);
                                bottomLetterbox.anchoredPosition = new Vector2(0, -100);

                                cameraContainer.DOMove(_playerCamera.transform.position, finalMoveDuration)
                                    .SetEase(Ease.InOutSine)
                                    .OnComplete(() =>
                                    {
                                        cameraContainer.position = _playerCamera.transform.position;
                                        cameraContainer.rotation = _playerCamera.transform.rotation;
                                        GameManager.Instance.SetCursorLockState(true);
                                        GameManager.Instance.OnOffEquipCamera(true);
                                    });
                            });
                    }
                });
        });
    }

    // 테스트 인터랙트
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnInteract();
        }
    }
}
