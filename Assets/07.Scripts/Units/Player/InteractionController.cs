using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    public IInteractable CurInteractable
    {
        get => _curInteractable;

        private set
        {
            if (value != _curInteractable)
            {
                if (value != null)
                {
                    value.Outline.enabled = true;
                }

                if (_curInteractable != null)
                {
                    _curInteractable.Outline.enabled = false;
                }
                
                _curInteractable = value;
            }
        }
    }


    [SerializeField] private float checkRate = 0.05f;  
    
    [SerializeField] private float maxCheckDistance;   
    
    [SerializeField] private TextMeshProUGUI promptText;
    
    
    private IInteractable _curInteractable;  
    
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
                    
                    CurInteractable = hit.collider.GetComponent<IInteractable>();
                }
            }
            else
            {
                CurInteractable = null;

                _previousHitObj = null;
            }
        }
    }



    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && CurInteractable != null)
        {
            CurInteractable.OnInteract();
            
            _previousHitObj = null;
            
            CurInteractable = null;
        }
    }
}