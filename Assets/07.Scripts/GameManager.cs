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
        UIManager.Instance.Create<InventoryUI>();
        UIManager.Instance.Create<QuickSlotUI>();
        UIManager.Instance.Create<AimUI>();
        UIManager.Instance.Create<ConditionUI>();
    }
    
    public void ToggleCursor(bool isLock)
    {
        IsLockedCursor = isLock;
        Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
