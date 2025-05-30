using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player _player;

    [Header("Move")]
    [SerializeField] private float moveSpeed;
    private Vector2 curMovement;
    [SerializeField] private float jump;
    [SerializeField] LayerMask groundLayerMask;

    [Header("Look")]
    [SerializeField] Transform cameraContainer;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float lookSensitivity;
    private float camcurXrot;

    private Vector2 mouseDelta;
    private CapsuleCollider capsuleCollider;

    private Rigidbody _rigidbody;

    [Header("Footstep Settings")]            // 효과음용 추가
    [SerializeField] private int playerSound;
    [SerializeField] private int playerJumpSound;
    [SerializeField] private float playerSoundInterval = 0.5f; // 걸음 소리 간격
    private float footstepTimer = 0f;

    public Player _Player
    {
        get
        {
            return _player;
        }
        set
        {
            _player = value;
        }
    }


    private GameManager _gameManager;

  
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        _gameManager = GameManager.Instance;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if(_gameManager.IsLockedCursor)
        {
            CameraLook();
        }
    }
    private void Move() // 움직이는 함수
    {
        Vector3 dir = transform.forward * curMovement.y + transform.right * curMovement.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

        // 발걸음 효과음 재생
        Vector3 horizontalVel = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        if (horizontalVel.magnitude > 0.1f)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= playerSoundInterval)
            {
                playerSound = 15;
                AudioManager.Instance.PlaySFXAtPoint(
                    playerSound,
                    transform.position
                );
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = playerSoundInterval; // 이동 멈추면 다음 걸음 준비
        }
    }

    private void CameraLook() // 카메라 움직임
    {
        camcurXrot += mouseDelta.y * lookSensitivity;
        camcurXrot = Mathf.Clamp(camcurXrot, minX, maxX);
        cameraContainer.localEulerAngles = new Vector3(-camcurXrot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovement = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovement = Vector2.zero;
        }
    }

    public void OnjumpInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && CanJump())
        {
            // 효과음 재생
            playerJumpSound = 14;
            AudioManager.Instance.PlaySFXAtPoint(
                playerJumpSound,
                transform.position
            );

            _rigidbody.AddForce(Vector2.up * jump, ForceMode.Impulse);
        }
    }

    private bool CanJump() // 점프 체크
    {
        Vector3 capsuleBottom = transform.position + capsuleCollider.center - Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);
        float checkradius = 0.5f;
        return Physics.CheckSphere(capsuleBottom, checkradius, groundLayerMask);
    }

    public void OnInventory(InputAction.CallbackContext context) // 인벤토리
    {
        if (context.phase == InputActionPhase.Started)
        {
            var uiManager = UIManager.Instance;

            bool enabledInventory = uiManager.IsUIEnabled<InventoryUI>();
            
            GameManager.Instance.ToggleCursor(enabledInventory);

            if (enabledInventory)
            {
                uiManager.Disable<InventoryUI>();
            }
            else
            {
                uiManager.Enable<InventoryUI>();
            }
        }
    }

    public void OnCraftingInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            var uiManager = UIManager.Instance;

            bool enabledCrafting = uiManager.IsUIEnabled<CraftingUI>();
            
            GameManager.Instance.ToggleCursor(enabledCrafting);

            if (enabledCrafting)
            {
                uiManager.Disable<CraftingUI>();
            }
            else
            {
                uiManager.Enable<CraftingUI>();
            }
        }
    }

   
    
    public void LookAtFairy()
    {
        if (QuestManager.Instance.npcManager.model.transform == null) return;

        Transform target = QuestManager.Instance.npcManager.model.transform;

        // =============== 1. 플레이어 몸통 Y축만 회전 ===============
        Vector3 flatDirection = target.position - transform.position;
        flatDirection.y = 0f; // 수평 방향만 고려

        if (flatDirection.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }

        // =============== 2. 카메라는 상하 각도 조절 ===============
        Vector3 directionToTarget = target.position - cameraContainer.position;
        Quaternion cameraRotation = Quaternion.LookRotation(directionToTarget);

        Vector3 camAngles = cameraRotation.eulerAngles;
        camcurXrot = -camAngles.x; // 카메라 pitch 업데이트
        cameraContainer.localEulerAngles = new Vector3(camcurXrot, 0, 0);

        // =============== 3. 대화 시작 ===============
        QuestManager.Instance.npcManager.TalkWithFairy();
        // if (QuestManager.Instance.npcManager.model.transform == null) return;
        //
        // Vector3 targetPosition = QuestManager.Instance.npcManager.model.transform.position;
        //
        // //targetPosition.y = transform.position.y;
        //
        // Vector3 direction = targetPosition - transform.position;
        //
        // if (direction.sqrMagnitude > 0.001f) 
        // {
        //     Quaternion lookRotation = Quaternion.LookRotation(direction);
        //     transform.rotation = lookRotation;
        // }
        //
        // QuestManager.Instance.npcManager.TalkWithFairy();

    }
}
