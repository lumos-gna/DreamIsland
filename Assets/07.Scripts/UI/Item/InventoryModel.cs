using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class InventoryModel
{
    public ItemSlotData[] itemSlots;        // 일반 인벤토리
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
        // HandleSlot에 먼저 스택할 수 있는 슬롯이 있는지 확인
        foreach (var slot in handleSlots)
        {
            if (slot.item == data && slot.quantity < data.maxStackCount)
            {
                slot.quantity++;
                return;
            }
        }

        // ItemSlot에도 스택할 수 있는 슬롯이 있는지 확인
        foreach (var slot in itemSlots)
        {
            if (slot.item == data && slot.quantity < data.maxStackCount)
            {
                slot.quantity++;
                return;
            }
        }

        // 빈 슬롯을 찾고 아이템 추가
        foreach (var slot in itemSlots)
        {
            if (slot.item == null)
            {
                slot.item = data;
                slot.quantity = 1;
                return;
            }
        }

        Debug.LogWarning("인벤토리가 가득 찼습니다.");
    }
}
