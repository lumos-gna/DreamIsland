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
                //후에 차징 (활) 처리를 위해 업데이트처리
                //인벤토리 슬롯 개수 갱신?
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
                    var itemData = _inventory.GetQuickSlotItem(index - 1);

                    if (itemData == null)
                    {
                        if (CurEquippedItem == null) return;
                        
                        CurEquippedItem.UnEquip();
                        Destroy(CurEquippedItem.gameObject);
                    }
                    else
                    {
                        if (CurEquippedItem != null)
                        {
                            if (CurEquippedItem.ItemData == itemData) return;
                            
                            CurEquippedItem.UnEquip();
                            Destroy(CurEquippedItem.gameObject);
                        }
                        
                        CurEquippedItem = Instantiate(itemData.EquipItemPrefab, equipParent);
                        CurEquippedItem.Equip(gameObject, itemData);
                    }
                }
                break;
        }
    }
}
