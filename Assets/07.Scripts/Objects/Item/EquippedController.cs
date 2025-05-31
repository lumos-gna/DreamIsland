using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquippedController : MonoBehaviour
{
    public enum InputState
    {
        Up,
        Down
    }
    
    [SerializeField] private Transform equipParent;
    
    public EquippedItem CurEquippedItem { get; set; }

    private InputState _inputState = InputState.Up;

    private Inventory _inventory;
    private ItemSlot _curSlot;

    private void Start()
    {
        _inventory = GameManager.Instance.Inventory;
    }

    private void Update()
    {
        if (CurEquippedItem != null)
        {
            if (CurEquippedItem.TryUse(_inputState))
            {
                if (_curSlot.quantity == 0)
                {
                    CurEquippedItem.UnEquip();
                    Destroy(CurEquippedItem.gameObject);
                }
            }
        }
    }


    public void OnLeftClickInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _inputState = InputState.Down;
                break;
            case InputActionPhase.Canceled:
                _inputState = InputState.Up;
                break;
        }
    }
    
    public void OnSelectItemInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (int.TryParse(context.control.name, out int index))
                {
                    _curSlot = _inventory.GetQuickSlotToIndex(index - 1);

                    if (_curSlot == null)
                    {
                        if (CurEquippedItem == null) return;
                        
                        CurEquippedItem.UnEquip();
                        Destroy(CurEquippedItem.gameObject);
                    }
                    else
                    {
                        var slotItem = _curSlot.item;
                        
                        if (CurEquippedItem != null)
                        {
                            if (CurEquippedItem.ItemData == slotItem) return;
                            
                            CurEquippedItem.UnEquip();
                            Destroy(CurEquippedItem.gameObject);
                        }
                        
                        CurEquippedItem = Instantiate(slotItem.EquippedPrefab, equipParent);
                        CurEquippedItem.Equip(gameObject, slotItem);
                    }
                }
                break;
        }
    }
}
