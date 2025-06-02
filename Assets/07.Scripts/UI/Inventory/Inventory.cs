using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory
{
    public ItemSlot[] itemSlots;      // 일반 인벤토리
    public ItemSlot[] handleSlots;    // 인벤토리 내 퀵슬롯
    public event UnityAction OnChangedInventory;

    public bool IsFull => itemSlots.FirstOrDefault((slot) => slot.item == null) == null;

    public Inventory(int itemSlotCount, int handleSlotCount)
    {
        itemSlots = new ItemSlot[itemSlotCount];
        handleSlots = new ItemSlot[handleSlotCount];

        for (int i = 0; i < itemSlotCount; i++)
        {
            itemSlots[i] = new ItemSlot();
        }

        for (int i = 0; i < handleSlotCount; i++)
        {
            handleSlots[i] = new ItemSlot();
        }
    }

    public ItemSlot GetQuickSlotToIndex(int index)
    {
        if (index < 0 || index >= handleSlots.Length)
        {
            Debug.LogWarning($"잘못된 인덱스입니다. : {index}");
            return null;
        }

        return handleSlots[index];
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
                }

                OnChangedInventory?.Invoke();

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

                OnChangedInventory?.Invoke();

                return;
            }
        }

        // 디버깅용 코드
        Debug.LogWarning($"[InventoryModel] DecreaseItem 실패 : '{item?.DisplayName}'을 인벤토리에서 찾을 수 없습니다.");
    }

    //찾고 싶은 슬롯의 조건을 람다로 넣어주면 됩니다
    public ItemSlot FindSlot(Func<ItemSlot, bool> slotCondition)
    {
        ItemSlot targetSlot = null;

        targetSlot = itemSlots.FirstOrDefault(slotCondition);

        if (targetSlot == null)
        {
            targetSlot = handleSlots.FirstOrDefault(slotCondition);
        }

        return targetSlot;
    }

    public void ForceSync()
    {
        OnChangedInventory?.Invoke();
    }
}
