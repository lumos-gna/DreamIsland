using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player _player;

    [Header("Attack Settings 테스트")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask destructibleLayer;

    [Header("Layer Masks")]
    [Tooltip("환경 파괴 가능 오브젝트 레이어만 포함")]
    [SerializeField] private LayerMask environmentLayer;
    [Tooltip("적 유닛 레이어만 포함")]
    [SerializeField] private LayerMask enemyLayer;


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

    private bool canlook = true;
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

    public bool Canlook
    {
        get { return canlook; }
    }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if(canlook)
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

    // 테스트용 매서드
    public void OnHit(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;

        // 화면 중앙에서 Raycast
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
        {
            int hitLayer = hit.collider.gameObject.layer;

            // 1) 환경 파괴 오브젝트
            if (((1 << hitLayer) & environmentLayer) != 0)
            {
                var destructible = hit.collider.GetComponentInParent<DestructibleObject>();
                if (destructible != null)
                {
                    destructible.ObjectTakeDamage(attackDamage);
                    Debug.Log($"Hit ENV {destructible.name}: –{attackDamage} HP");
                }
            }
            // 2) 적 유닛
            else if (((1 << hitLayer) & enemyLayer) != 0)
            {
                var condition = hit.collider.GetComponentInParent<ConditionHandler>();
                if (condition != null)
                {
                    condition.TakeDamage(attackDamage);
                    Debug.Log($"Hit ENEMY {condition.name}: –{attackDamage} HP");
                }
            }
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

            if (uiManager.IsUIEnabled<InventoryUI>())
            {
                uiManager.Disable<InventoryUI>();
                ChangeCursorState(false);
            }
            else
            {
                uiManager.Enable<InventoryUI>();
                ChangeCursorState(true);
            }
        }
    }

    public void ChangeCursorState(bool ispopon) // 커서 상태 변경(인벤토리 열었을때?)
    {
        Cursor.lockState = ispopon ? CursorLockMode.None : CursorLockMode.Locked;
        canlook = !ispopon;
        if(canlook)
        {
            UIManager.Instance.Enable<AimUI>();
        }
        else
        {
            UIManager.Instance.Disable<AimUI>();
        }
    }
    
    public void LookAtFairy()
    {
        if (QuestManager.Instance.npcManager.model.transform == null) return;

        Vector3 direction = QuestManager.Instance.npcManager.model.transform.position - transform.position;
    
        if (direction.sqrMagnitude > 0.001f) 
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }

        QuestManager.Instance.npcManager.TalkWithFairy();
    }
}
