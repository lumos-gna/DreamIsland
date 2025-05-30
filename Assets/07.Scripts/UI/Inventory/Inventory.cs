using System;
using UnityEngine;
using UnityEngine.Events;

public class Inventory
{
    public ItemSlotData[] itemSlots;      // 일반 인벤토리
    public ItemSlotData[] handleSlots;    // 인벤토리 내 퀵슬롯

    public event UnityAction OnChangedInventory;


    public Inventory(int itemSlotCount, int handleSlotCount)
    {
        itemSlots = new ItemSlotData[itemSlotCount];
        handleSlots = new ItemSlotData[handleSlotCount];

        for (int i = 0; i < itemSlotCount; i++)
        {
            itemSlots[i] = new ItemSlotData();
        }

        for (int i = 0; i < handleSlotCount; i++)
        {
            handleSlots[i] = new ItemSlotData();
        }
    }

    public ItemData GetQuickSlotItem(int index)
    {
        if (index < 0 || index >= handleSlots.Length)
        {
            Debug.LogWarning($"잘못된 인덱스입니다. : {index}");
            return null;
        }

        var slot = handleSlots[index];
        
        if (slot.item == null)
        {
            Debug.Log($"{index + 1}번째 퀵슬롯 인덱스에 아이템이 없습니다.");
            return null;
        }

        return slot.item;
    }

    public void AddItem(ItemData data)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == data && itemSlots[i].quantity < data.MaxStackCount)
            {
                itemSlots[i].quantity++;
                
                OnChangedInventory?.Invoke();
                return;
            }
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == null)
            {
                itemSlots[i].item = data;
                itemSlots[i].quantity = 1;
                
                OnChangedInventory?.Invoke();
                return;
            }
        }

        Debug.LogWarning("인벤토리가 가득 찼습니다.");
    }

    public void DecreaseItem(ItemData item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == item)
            {
                itemSlots[i].quantity--;
                
                if (itemSlots[i].quantity <= 0)
                {
                    itemSlots[i].item = null;
                    itemSlots[i].quantity = 0;
                    
                    OnChangedInventory?.Invoke();
                }
                return;
            }
        }

        for (int i = 0; i < handleSlots.Length; i++)
        {
            if (handleSlots[i].item == item)
            {
                handleSlots[i].quantity--;
                
                if (handleSlots[i].quantity <= 0)
                {
                    handleSlots[i].item = null;
                    handleSlots[i].quantity = 0;
                    
                    OnChangedInventory?.Invoke();
                }
                return;
            }
        }

        // 디버깅용 코드
        Debug.LogWarning($"[InventoryModel] DecreaseItem 실패 : '{item?.DisplayName}'을 인벤토리에서 찾을 수 없습니다.");
    }
    
    public void ForceSync()
    {
        OnChangedInventory?.Invoke();
    }
}
