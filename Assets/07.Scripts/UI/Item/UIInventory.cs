using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : BaseUI
{
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform handleSlotPanel;   // 인벤토리 퀵슬롯 부모
    public TextMeshProUGUI onMouseItemName;
    public TextMeshProUGUI onMouseItemDescription;
    public GameObject summaryBox;

    public ItemSlot[] slots;
    public HandleSlot[] handleSlots;    // 인벤토리 내 퀵슬롯

    private InventoryModel model;
    private QuickSlotUI quickSlotUI;

    // 해당 부분 PlayerController.cs에 인벤토리 로직 추가되면 Test제외하고 사용하세요.
    private TestPlayerController controller;
    //private PlayerController controller;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    public override void Init()
    {
        inventoryWindow.SetActive(false);
        summaryBox.SetActive(false);

        slots = new ItemSlot[slotPanel.childCount];
        handleSlots = new HandleSlot[handleSlotPanel.childCount];

        // 보관 아이템 슬롯
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].ClearSlot();
        }

        // 핸들 아이템 슬롯
        for (int i = 0; i < handleSlots.Length; i++)
        {
            handleSlots[i] = handleSlotPanel.GetChild(i).GetComponent<HandleSlot>();
            handleSlots[i].index = i;
            handleSlots[i].inventory = this;
            handleSlots[i].ClearSlot();
        }

        model = new InventoryModel(slots.Length, handleSlots.Length);
        quickSlotUI = UIManager.Instance.Create<QuickSlotUI>() as QuickSlotUI;
        quickSlotUI.SetQuickSlotsFromHandleSlots(handleSlots);

        CharacterManager.Instance.Player.addItem += AddItem;

        // 인벤토리에서 아이콘에 커서를 갖다 대기 전 나올 아이템의 정보를 클리어
        ClearSelectedItemWindow();
    }

    public override void Enable()
    {
        inventoryWindow.SetActive(true);

        UIManager.Instance.Get<QuickSlotUI>()?.Disable();
    }

    public override void Disable()
    {
        inventoryWindow.SetActive(false);

        UIManager.Instance.Get<QuickSlotUI>()?.Enable();
        quickSlotUI.SetQuickSlotsFromHandleSlots(handleSlots);
    }

    // 인벤토리 아이템 슬롯에 마우스를 올렸을 때 아이템의 설명을 보이기 위한 메서드
    public void MouseOnInventoryItem(int index)
    {
        var slot = slots[index];
        if (slot.item == null) return;
        onMouseItemName.text = slot.item.displayName;
        onMouseItemDescription.text = slot.item.description;
    }

    public void ClearSelectedItemWindow()
    {
        onMouseItemName.text = string.Empty;
        onMouseItemDescription.text = string.Empty;
    }

    private void Update()
    {
        if (!summaryBox.activeSelf) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(summaryBox.transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out pos);
        summaryBox.transform.localPosition = pos;
    }

    public void AddItem(ItemData data)
    {
        var player = CharacterManager.Instance.Player;
        if (player.itemData == null) return;

        model.AddItem(data);
        UpdateUI();
        player.itemData = null;
    }

    //public bool IsOpenInventory()
    //{
    //    return inventoryWindow.activeInHierarchy;
    //}

    //private void AddItem()
    //{
    //    ItemData data = CharacterManager.Instance.Player.itemData;   // player에 맞게 수정 필요

    //    if (data.canStack)
    //    {
    //        ItemSlot slot = GetItemStack(data);

    //        if (slot != null)
    //        {
    //            slot.quantity++;
    //            UpdateUI();
    //            CharacterManager.Instance.Player.itemData = null;   // player에 맞게 수정 필요
    //            return;
    //        }
    //    }

    //    ItemSlot emptySlot = GetEmptySlot();

    //    if (emptySlot != null)
    //    {
    //        emptySlot.item = data;
    //        emptySlot.quantity = 1;
    //        UpdateUI();
    //        CharacterManager.Instance.Player.itemData = null;   // player에 맞게 수정 필요
    //        return;
    //    }

    //    //ThrowItem(data);
    //    CharacterManager.Instance.Player.itemData = null;   // player에 맞게 수정 필요
    //}

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var data = model.itemSlots[i];
            if (data.item != null)
            {
                slots[i].item = data.item;
                slots[i].quantity = data.quantity;
                slots[i].SetSlot();
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        for (int i = 0; i < handleSlots.Length; i++)
        {
            var data = model.handleSlots[i];
            if (data.item != null)
            {
                handleSlots[i].item = data.item;
                handleSlots[i].quantity = data.quantity;
                handleSlots[i].SetSlot();
            }
            else
            {
                handleSlots[i].ClearSlot();
            }
        }
    }

    //public ItemSlot GetItemStack(ItemData data)
    //{
    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (slots[i].item == data && slots[i].quantity < data.maxStackCount)
    //        {
    //            return slots[i];
    //        }
    //    }

    //    return null;
    //}

    //private ItemSlot GetEmptySlot()
    //{
    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (slots[i].item == null)
    //        {
    //            return slots[i];
    //        }
    //    }

    //    return null;
    //}

    // 아이템 버릴 때 사용할 메서드
    //private void ThrowItem(ItemData data)
    //{
    //    Instantiate(data.dropItemPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    //}
}
