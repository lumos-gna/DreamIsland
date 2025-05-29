using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class InventoryModel
{
    public ItemSlotData[] itemSlots;      // 일반 인벤토리
    public ItemSlotData[] handleSlots;    // 인벤토리 내 퀵슬롯

    public InventoryModel(int itemSlotCount, int handleSlotCount)
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

    public void AddItem(ItemData data)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == data && itemSlots[i].quantity < data.maxStackCount)
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
}
