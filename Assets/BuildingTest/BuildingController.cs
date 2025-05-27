using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    [SerializeField] private BuildingObject buildingObjectHPrefab;
    [SerializeField] private BuildingObject buildingObjectVPrefab;
    
    [SerializeField] private LayerMask buildableMask;

    [SerializeField] private float rayDistance;

    
    private BuildingObject _curBuildingObject;
    
    
    private bool _isBuildable;
    
    
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    public float maxYAngle = 80f;

    private float yaw = 0f;
    private float pitch = 0f;
    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TryBuild();
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Enable();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            EnableVertical();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 euler = _curBuildingObject.transform.eulerAngles;
            euler.y += 45f;
            _curBuildingObject.transform.eulerAngles = euler;
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
        _curBuildingObject = Instantiate(buildingObjectHPrefab);
        
        _curBuildingObject.InitToBuilding();
    }
    
    public void EnableVertical()
    {
        _curBuildingObject = Instantiate(buildingObjectVPrefab);
        
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

            TrySnap(hit);

            _isBuildable = true;
        }
     
        _curBuildingObject.UpdateToBuildingState(_isBuildable);
    }


    bool TrySnap(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out BuildingObject targetObject))
        {
            if (targetObject.IsSnappable() &&_curBuildingObject.IsSnappable() )
            {
                BuildingSnapPoint targetSnapPoint = targetObject.GetClosestSnapPointToHit(hit.point);

                //BuildingSnapPoint curSnapPoint = _curBuildingObject.GetClosestSnapPointToHit(targetObject.transform.position);
                
                BuildingSnapPoint curSnapPoint = _curBuildingObject.GetClosestSnapPointToSnapPoint(targetSnapPoint);

                if (curSnapPoint != null)
                {
                    Vector3 offset = targetSnapPoint.transform.position - curSnapPoint.transform.position;
                    
                    _curBuildingObject.transform.position += offset;
                    
                    Debug.Log(targetSnapPoint.gameObject.name);
                    //Debug.Log(curSnapPoint.gameObject.name);
                    
                    return true;
                }
            }
        }

        return false;
    }
    
}
