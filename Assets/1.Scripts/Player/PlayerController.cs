//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerController : MonoBehaviour
//{
//    private Player _player;

//    [Header("Move")]
//    [SerializeField] private float moveSpeed;
//    private Vector2 curMovement;
//    [SerializeField] private float jump;
//    [SerializeField] LayerMask groundLayerMask;

//    [Header("Look")]
//    [SerializeField] Transform cameraContainer;
//    [SerializeField] private float minX;
//    [SerializeField] private float maxX;
//    [SerializeField] private float lookSensitivity;
//    private float camcurXrot;

//    private bool canlook = true;
//    private bool canjump = true;
//    private Vector2 mouseDelta;

//    private Rigidbody _rigidbody;

//    public Player _Player
//    {
//        get
//        {
//            return _player;
//        }
//        set
//        {
//            _player = value;
//        }
//    }
//    void Start()
//    {
//        _rigidbody = GetComponent<Rigidbody>();
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    private void FixedUpdate()
//    {
//        Move();
//    }

//    private void LateUpdate()
//    {
//        if(canlook)
//        {
//            CameraLook();
//        }
//    }


//    private void Move()
//    {
//        Vector3 dir = transform.forward * curMovement.y + transform.right * curMovement.x;
//        dir *= moveSpeed;
//        dir.y = _rigidbody.velocity.y;

//        _rigidbody.velocity = dir;
//    }

//    private void CameraLook()
//    {
//        camcurXrot += mouseDelta.y * lookSensitivity;
//        camcurXrot = Mathf.Clamp(camcurXrot, minX, maxX);
//        cameraContainer.localEulerAngles = new Vector3(-camcurXrot, 0, 0);
//        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
//    }

//    public void OnLookInput(InputAction.CallbackContext context)
//    {
//        mouseDelta = context.ReadValue<Vector2>();
//    }

//    public void OnMoveInput(InputAction.CallbackContext context)
//    {
//        if(context.phase == InputActionPhase.Performed)
//        {
//            curMovement = context.ReadValue<Vector2>();
//        }
//        else if(context.phase == InputActionPhase.Canceled)
//        {
//            curMovement = Vector2.zero;
//        }
//    }

//    public void OnjumpInput(InputAction.CallbackContext context)
//    {
//        if(context.phase == InputActionPhase.Started && canjump)
//        {
//            _rigidbody.AddForce(Vector2.up * jump, ForceMode.Impulse);
//        }
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        Debug.Log("���� ����");
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            Debug.Log("���� ����");
//            canjump = true;
//        }
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            canjump = false;
//            Debug.Log("���� �Ұ�");
//        }
//    }

//    public void ChangeCursorState(bool ispopon)
//    {
//        Cursor.lockState = ispopon ? CursorLockMode.None : CursorLockMode.Locked;
//        canlook = !ispopon;
//    }
//}
