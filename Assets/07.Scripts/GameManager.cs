using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Inventory Inventory { get; private set; }

    public event Action OnInventoryChanged;

    private void Awake()
    {
        Inventory = new Inventory(itemSlotCount: 21, handleSlotCount: 7);
    }

    public void AddItem(ItemData data)
    {
        Inventory.AddItem(data);
        OnInventoryChanged?.Invoke();
    }

    public void DecreaseItem(ItemData data)
    {
        Inventory.DecreaseItem(data);
        OnInventoryChanged?.Invoke();
    }

    public void ForceSync()
    {
        OnInventoryChanged?.Invoke();
    }
}
