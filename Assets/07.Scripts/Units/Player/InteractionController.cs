using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private float checkRate = 0.05f;
    [SerializeField] private float maxCheckDistance;
    [SerializeField] private TextMeshProUGUI promptText;

    private IInteractable _curInteractable;

    private GameObject _previousHitObj;
    private Camera _camera;

    private float _lastCheckTime;
    private bool _onInteract;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (_onInteract)
        {
            _curInteractable.OnInteract();

            _curInteractable = null;

            _onInteract = false;

            return;
        }

        if (Time.time - _lastCheckTime > checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));

            if (Physics.Raycast(ray, out RaycastHit hit, maxCheckDistance))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    if (_curInteractable != interactable)
                    {
                        if (_curInteractable != null)
                        {
                            _curInteractable.Outline.enabled = false;
                        }
                        else
                        {
                            interactable.Outline.enabled = true;
                        }

                        _curInteractable = interactable;
                    }
                    return;
                }
            }

            if (_curInteractable != null)
            {
                _curInteractable.Outline.enabled = false;
                _curInteractable = null;
            }
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && _curInteractable != null)
        {
            _onInteract = true;
        }
    }
}