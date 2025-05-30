using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Inventory Inventory { get; private set; }

    public event Action OnInventoryChanged;

    public event Action<ItemDataSO> SelectedItem;

    private void Awake()
    {
        Inventory = new Inventory(itemSlotCount: 21, handleSlotCount: 7);

        // 인벤토리에서 발생한 이벤트를 GameManager가 관리
        Inventory.SelectedItem += (item) => { SelectedItem?.Invoke(item); };
    }

    public void AddItem(ItemDataSO data)
    {
        Inventory.AddItem(data);
        OnInventoryChanged?.Invoke();
    }

    public void DecreaseItem(ItemDataSO data)
    {
        Inventory.DecreaseItem(data);
        OnInventoryChanged?.Invoke();
    }

    public void ForceSync()
    {
        OnInventoryChanged?.Invoke();
    }

    public void SelectQuickSlotItem(int index)
    {
        Inventory.SelectQuickSlotItem(index);
    }
}
