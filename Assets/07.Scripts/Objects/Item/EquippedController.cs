using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquippedController : MonoBehaviour
{
    public EquippedItem CurEquippedItem { get; private set; }
    public ItemSlot CurSlot { get; private set; }
    public Inventory Inventory { get; private set; }

    public bool IsInputDown { get; private set; }
    public bool IsInputUp { get; private set; }

    [SerializeField] private Transform equipParent;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        Inventory = _gameManager.Inventory;
    }

    private void Update()
    {
        if (!_gameManager.IsLockedCursor || CurEquippedItem == null)
            return;

        CurEquippedItem.Use();
    }

    private void LateUpdate()
    {
        IsInputDown = false;
        IsInputUp = false;
    }

    public void OnLeftClickInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                IsInputDown = true;
                break;
            case InputActionPhase.Canceled:
                IsInputUp = true;
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
                    CurSlot = Inventory.GetQuickSlotToIndex(index - 1);

                    var quickSlotUI = UIManager.Instance.Get<QuickSlotUI>();
                    quickSlotUI.HighlightSlot(index - 1);
                    if (CurSlot == null)
                    {
                        if (CurEquippedItem == null)
                            return;

                        CurEquippedItem.UnEquip();
                    }
                    else
                    {
                        var slotItem = CurSlot.item;

                        if (CurEquippedItem != null)
                        {
                            if (CurEquippedItem.ItemData == slotItem)
                                return;

                            CurEquippedItem.UnEquip();
                        }

                        if (slotItem != null)
                        {
                            if (!CurSlot.item.IsEquippalbe)
                                return;

                            CurEquippedItem = Instantiate(slotItem.EquippedPrefab, equipParent);
                            CurEquippedItem.Equip(this, slotItem);
                        }
                    }
                }
                break;
        }
    }
}
