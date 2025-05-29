using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : BaseUI
{
    public HandleSlot[] quickSlots;
    public Transform quickSlotPanel;

    public override void Init()
    {
        quickSlots = new HandleSlot[quickSlotPanel.childCount];

        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = quickSlotPanel.GetChild(i).GetComponent<HandleSlot>();
            quickSlots[i].index = i;
            quickSlots[i].inventory = UIManager.Instance.Get<UIInventory>() as UIInventory;
            quickSlots[i].ClearSlot();
        }

        gameObject.SetActive(true);
    }

    public override void Enable() => gameObject.SetActive(true);
    public override void Disable() => gameObject.SetActive(false);

    public void SetQuickSlotsFromHandleSlots(HandleSlot[] handleSlots)
    {
        for (int i = 0; i < quickSlots.Length && i < handleSlots.Length; i++)
        {
            quickSlots[i].item = handleSlots[i].item;
            quickSlots[i].quantity = handleSlots[i].quantity;
            quickSlots[i].SetSlot();
        }
    }

    public void SyncToItemEquip()
    {
        var equip = PlayerManager.Instance._Player.GetComponent<ItemEquip>();

        for (int i = 0; i < quickSlots.Length; i++)
        {
            equip.SetSlotItem(i, quickSlots[i].item);
        }
    }
}
