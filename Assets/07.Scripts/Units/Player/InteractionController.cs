using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private float checkRate = 0.05f;  
    
    [SerializeField] private float maxCheckDistance;   
    
    [SerializeField] private TextMeshProUGUI promptText;
    
    
    private IInteractable _curInteractable;  
    
    private GameObject _curInteractGameObject; 

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
                if(hit.collider.gameObject != _curInteractGameObject)
                {
                    _curInteractGameObject = hit.collider.gameObject;
                    
                    _curInteractable = hit.collider.GetComponent<IInteractable>();
                    
                    //SetPromptText();
                }
            }
            else
            {
                _curInteractGameObject = null;
                
                _curInteractable = null;
                
                //promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        
        promptText.text = _curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && _curInteractable != null)
        {
            _curInteractable.OnInteract();
            
            _curInteractGameObject = null;
            
            _curInteractable = null;
            
            //promptText.gameObject.SetActive(false);
        }
    }
}