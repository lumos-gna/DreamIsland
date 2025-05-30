using System;
using UnityEngine;

public class Inventory
{
    public ItemSlotData[] itemSlots;      // 일반 인벤토리
    public ItemSlotData[] handleSlots;    // 인벤토리 내 퀵슬롯

    public event Action<ItemDataSO> SelectedItem;

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

    public void SelectQuickSlotItem(int index)
    {
        if (index < 0 || index >= handleSlots.Length)
        {
            Debug.LogWarning($"잘못된 인덱스입니다. : {index}");
            return;
        }

        var slot = handleSlots[index];
        if (slot.item == null)
        {
            Debug.LogWarning($"{index + 1}번째 퀵슬롯 인덱스에 아이템이 없습니다.");
            return;
        }

        SelectedItem?.Invoke(slot.item);
    }

    public void AddItem(ItemDataSO data)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == data && itemSlots[i].quantity < data.MaxStackCount)
            {
                itemSlots[i].quantity++;
                return;
            }
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == null)
            {
                itemSlots[i].item = data;
                itemSlots[i].quantity = 1;
                return;
            }
        }

        Debug.LogWarning("인벤토리가 가득 찼습니다.");
    }

    public void DecreaseItem(ItemDataSO item)
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
                }
                return;
            }
        }

        // 디버깅용 코드
        Debug.LogWarning($"[InventoryModel] DecreaseItem 실패 : '{item?.DisplayName}'을 인벤토리에서 찾을 수 없습니다.");
    }
}
