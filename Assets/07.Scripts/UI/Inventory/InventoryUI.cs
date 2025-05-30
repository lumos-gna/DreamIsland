using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : BaseUI
{
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform handleSlotPanel;   // 인벤토리 퀵슬롯 부모
    public TextMeshProUGUI onMouseItemName;
    public TextMeshProUGUI onMouseItemDescription;
    public GameObject summaryBox;

    public ItemSlot[] slots;
    public HandleSlot[] handleSlots;    // 인벤토리 내 퀵슬롯

    private Inventory model;
    private QuickSlotUI quickSlotUI;

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
            slots[i].ClearSlot();
        }

        // 핸들 아이템 슬롯
        for (int i = 0; i < handleSlots.Length; i++)
        {
            handleSlots[i] = handleSlotPanel.GetChild(i).GetComponent<HandleSlot>();
            handleSlots[i].index = i;
            handleSlots[i].ClearSlot();
        }

        model = GameManager.Instance.Inventory;

        GameManager.Instance.OnInventoryChanged += UpdateUI;

        quickSlotUI = UIManager.Instance.Create<QuickSlotUI>() as QuickSlotUI;

        // 인벤토리에서 아이콘에 커서를 갖다 대기 전 나올 아이템의 정보를 클리어
        ClearSelectedItemWindow();

        // 초기 상태를 렌더링
        UpdateUI();
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
    }

    // 인벤토리 아이템 슬롯에 마우스를 올렸을 때 아이템의 설명을 보이기 위한 메서드
    public void MouseOnInventoryItem(int index)
    {
        var slot = slots[index];
        if (slot.item == null) return;
        onMouseItemName.text = slot.item.DisplayName;
        onMouseItemDescription.text = slot.item.Description;
    }

    public void ClearSelectedItemWindow()
    {
        onMouseItemName.text = string.Empty;
        onMouseItemDescription.text = string.Empty;
    }

    private void Update()
    {
        if (!summaryBox.activeSelf) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            summaryBox.transform.parent.GetComponent<RectTransform>(),
            Input.mousePosition,
            null,
            out Vector2 pos);
        summaryBox.transform.localPosition = pos;
    }

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
}
