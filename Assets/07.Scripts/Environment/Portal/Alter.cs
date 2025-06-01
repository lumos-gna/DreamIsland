

using DG.Tweening;
using UnityEngine;

public class Alter : MonoBehaviour, IInteractable
{
    public Outline Outline { get; private set; }

    public GameObject fireEffect;
    public GameObject portalPosition;
    public GameObject portal;

    [Header("카메라 이동")]
    public Transform cameraContainer;  // 이동 및 회전할 카메라 오브젝트
    public Transform moveTarget1;       // 이동할 위치
    public Transform moveTarget2;    
    public float moveDuration = 0.5f;
    public float lookDuration = 1.5f;
    public float finalMoveDuration = 1f;
    
    [Header("레터 박스")]
    public RectTransform topLetterbox;    // 위쪽 레터박스 RectTransform
    public RectTransform bottomLetterbox; // 아래쪽 레터박스 RectTransform
    
    [SerializeField] private ItemData targetItemData;
    [SerializeField] private int targetItemCount;


    private Inventory _inventory;
    private GameObject _childObject;
    private GameObject _spawnedPortal;
    

    private void Awake()
    {
        Outline = GetComponent<Outline>();

        _inventory = GameManager.Instance.Inventory;
    }

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        Transform child = player.transform.Find("CamerContainer"); // 자식 오브젝트 이름
        if (child != null)
        {
            _childObject = child.gameObject;
        }
    }

    public void OnInteract()
    {
        // 해당 아이템이 슬롯에 목표만큼 있는지 
        if (_inventory.FindSlot((slot) => slot.item == targetItemData && slot.quantity >= targetItemCount) != null)
        {
            fireEffect.SetActive(true);

            GameObject newPortal = Instantiate(portal, portalPosition.transform.position, portalPosition.transform.rotation);
            newPortal.transform.SetParent(portalPosition.transform);

            Vector3 pos = newPortal.transform.localPosition;
            pos.y = -10f;
            newPortal.transform.localPosition = pos;

            _spawnedPortal = newPortal;  // 생성한 포탈 저장

            StartCameraEvent();
        }
    }
    
    public void StartCameraEvent()
{
    if (cameraContainer == null || moveTarget1 == null || moveTarget2 == null || portalPosition == null || _childObject == null) return;
    if (topLetterbox == null || bottomLetterbox == null) return;

    // 마우스 조작 비활성화
    GameManager.Instance.SetCursorLockState(false); // 커서 보이게 (IsLockedCursor = false)

    // 1단계: moveTarget1 위치로 즉시 이동
    cameraContainer.position = moveTarget1.position;

    // 2단계: 즉시 회전
    Vector3 dir = (portalPosition.transform.position - cameraContainer.position).normalized;
    Quaternion targetRot = Quaternion.LookRotation(dir);
    cameraContainer.rotation = targetRot;

    // 2.5단계: 레터박스 등장
    Sequence letterboxSeq = DOTween.Sequence();
    letterboxSeq.Append(topLetterbox.DOAnchorPosY(-50, moveDuration).SetEase(Ease.InOutSine));
    letterboxSeq.Join(bottomLetterbox.DOAnchorPosY(50, moveDuration).SetEase(Ease.InOutSine));

    // 3단계: 카메라 이동
    letterboxSeq.OnComplete(() =>
    {
        cameraContainer.DOMove(moveTarget2.position, moveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                // 3.5단계: 포탈 y=-7 → y=0 으로 등장 애니메이션
                _spawnedPortal .transform.DOLocalMoveY(0f, finalMoveDuration)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() =>
                    {
                        // 레터박스 즉시 숨김
                        topLetterbox.anchoredPosition = new Vector2(0, 100);
                        bottomLetterbox.anchoredPosition = new Vector2(-0, -100);

                        // 4단계: 마지막 이동
                        cameraContainer.DOMove(_childObject.transform.position, finalMoveDuration)
                            .SetEase(Ease.InOutSine)
                            .OnComplete(() =>
                            {
                                // 정확한 위치와 회전 보정
                                cameraContainer.position = _childObject.transform.position;
                                cameraContainer.rotation = _childObject.transform.rotation;

                                // 마우스 조작 다시 활성화
                                GameManager.Instance.SetCursorLockState(true); // 커서 감추고, IsLockedCursor = true
                            });
                    });
            });
    });
}


    
    //테스트용, 나중에 삭제
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnInteract();
            
            
            
            
            
        }
    }
}
