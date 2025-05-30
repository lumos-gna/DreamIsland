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

            quickSlots[i].ClearSlot();
        }

        GameManager.Instance.OnInventoryChanged += UpdateUIFromData;

        // 시작 시 현재 상태 반영
        UpdateUIFromData();

        gameObject.SetActive(true);
    }

    public override void Enable() => gameObject.SetActive(true);
    public override void Disable() => gameObject.SetActive(false);

    private void UpdateUIFromData()
    {
        var handleSlotData = GameManager.Instance.Inventory.handleSlots;

        for (int i = 0; i < quickSlots.Length && i < handleSlotData.Length; i++)
        {
            quickSlots[i].item = handleSlotData[i].item;
            quickSlots[i].quantity = handleSlotData[i].quantity;
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
