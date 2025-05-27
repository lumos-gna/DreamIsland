using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerState state;
    private PlayerController _playerController;
    [SerializeField] private GameObject tempitem;
    [SerializeField] private Transform EquipParent;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void ChoiceItem(GameObject item)// 아이템을 고르는 함수?
    {
        Instantiate(item, EquipParent);
    }

    public void OnChoiceitemInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            ChoiceItem(tempitem);
        }
    }
}
