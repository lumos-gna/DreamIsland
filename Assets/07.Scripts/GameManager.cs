using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsLockedCursor { get; private set; }

    public Inventory Inventory { get; private set; }

    private void Awake()
    {
        Inventory = new Inventory(itemSlotCount: 21, handleSlotCount: 7);
    }

    private void Start()
    {
        UIManager.Instance.Enable<QuickSlotUI>();
        UIManager.Instance.Enable<AimUI>();
        UIManager.Instance.Enable<ConditionUI>();
        
        UIManager.Instance.Disable<InventoryUI>();
        UIManager.Instance.Disable<CraftingUI>();
    }
    
    public void ToggleCursor(bool isLock)
    {
        IsLockedCursor = isLock;
        Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
