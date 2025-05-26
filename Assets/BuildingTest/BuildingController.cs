using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    [SerializeField] private BuildingObject buildingObjectPrefab;
    
    [SerializeField] private LayerMask buildableMask;

    [SerializeField] private float rayDistance;

    
    private BuildingObject _curBuildingObject;
    
    
    private bool _isSnap;
    private bool _isBuildable;
    
    
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    public float maxYAngle = 80f;

    private float yaw = 0f;
    private float pitch = 0f;
    

    private void Start()
    {
        Enable();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            _isSnap = true;
        }
        else
        {
            _isSnap = false;
        }
        
        if (Input.GetMouseButton(0))
        {
            TryBuild();
        }
       
        
        UpdateBuildingObject();

        
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxYAngle, maxYAngle);

        targetCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");  

        Vector3 move = targetCamera.transform.right * moveX + targetCamera.transform.forward * moveZ;
        targetCamera.transform.position += move * moveSpeed * Time.deltaTime;
    }


    public void Enable()
    {
        _curBuildingObject = Instantiate(buildingObjectPrefab);
        
        _curBuildingObject.InitToBuilding();
    }
    
    
    public void Disable() =>  Destroy(_curBuildingObject.gameObject);
    

    public bool TryBuild()
    {
        if (_isBuildable && _curBuildingObject != null)
        {
            _curBuildingObject.Built();

            _curBuildingObject = null;

            return true;
        }

        return false;
    }


    public void UpdateBuildingObject()
    {
        if (_curBuildingObject == null)
        {
            return;
        }


        _isBuildable = false;
        
        
        Ray ray = targetCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, buildableMask))
        {
            _curBuildingObject.transform.position = hit.point;

            if (_isSnap)
            {
                Snap(hit);
            }

            _isBuildable = true;
        }
     
        _curBuildingObject.UpdateToBuildingState(_isBuildable);
    }


    void Snap(RaycastHit hit)
    {
        if (_curBuildingObject.IsSnappable())
        {
            if (hit.collider.TryGetComponent(out BuildingObject targetObject))
            {
                if (targetObject.IsSnappable())
                {
                    Vector3 targetPoint = targetObject.GetCloseSnapPoint(hit.point);
                        
                    Vector3 curObjectPoint = _curBuildingObject.GetCloseSnapPoint(targetObject.transform.position);

                    _curBuildingObject.transform.position += targetPoint - curObjectPoint;
                }
            }
        }
    }
    
}
