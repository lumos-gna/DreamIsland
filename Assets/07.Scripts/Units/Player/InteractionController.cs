using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private float checkRate = 0.05f;  
    
    [SerializeField] private float maxCheckDistance;   
    
    [SerializeField] private TextMeshProUGUI promptText;
    
    
    private IInteractable _curInteractable;  
    private IInteractable _previousInteractable;  
    
    private GameObject _previousHitObj; 

    private Camera _camera;
    
    private float _lastCheckTime;       

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if(Time.time - _lastCheckTime > checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));

            if(Physics.Raycast(ray, out RaycastHit hit, maxCheckDistance))
            {
                if (hit.collider.gameObject != _previousHitObj)
                {
                    _previousHitObj =  hit.collider.gameObject;
                    
                    _curInteractable = hit.collider.GetComponent<IInteractable>();
                }
            }
            else
            {
                _curInteractable = null;

                _previousHitObj = null;
            }
        }

        if (_previousInteractable != _curInteractable)
        {
            if (_previousInteractable != null)
            {
                _previousInteractable.Outline.enabled = false;
            }

            if (_curInteractable != null)
            {
                _curInteractable.Outline.enabled = true;
            }

            _previousInteractable = _curInteractable;
        }
    }



    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && _curInteractable != null)
        {
            _curInteractable.OnInteract();
            
            _curInteractable = null;

            _previousHitObj = null;
        }
    }
}