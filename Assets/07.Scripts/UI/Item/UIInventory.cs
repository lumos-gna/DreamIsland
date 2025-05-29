using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : BaseUI
{
    public ItemSlot[] slots;
    public HandleSlot[] handleSlots;    // 인벤토리 내 퀵슬롯
    public HandleSlot[] quickSlots;     // 게임화면 퀵슬롯

    public GameObject inventoryWindow;
    public GameObject quickSlotWindow;
    public Transform slotPanel;
    public Transform handleSlotPanel;   // 인벤토리 퀵슬롯 부모
    public Transform quickSlotPanel;    // 게임화면 퀵슬롯 부모
    public Transform dropPosition;

    public GameObject summaryBox;
    //public Vector2 summaryBoxOffset = new Vector2(20f, 20f);

    [Header("Select Item")]
    public TextMeshProUGUI onMouseItemName;
    public TextMeshProUGUI onMouseItemDescription;

    // 해당 부분 PlayerController.cs에 인벤토리 로직 추가되면 Test제외하고 사용하세요.
    private TestPlayerController controller;
    //private PlayerController controller;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    private void Update()
    {
        if (!summaryBox.activeSelf) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(summaryBox.transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out pos);
        summaryBox.transform.localPosition = pos;
    }

    public void ClearSelectedItemWindow()
    {
        onMouseItemName.text = string.Empty;
        onMouseItemDescription.text = string.Empty;
    }

    // Inventory창 Open/Close시 호출
    /*public void Toggle()
    {
        if (IsOpenInventory())
        {
            UIManager.Instance.Disable<UIInventory>();
        }
        else
        {
            UIManager.Instance.Enable<UIInventory>();
        }
    }*/

    public bool IsOpenInventory()
    {
        return inventoryWindow.activeInHierarchy;
    }

    private void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;   // player에 맞게 수정 필요

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);

            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;   // player에 맞게 수정 필요
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;   // player에 맞게 수정 필요
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;   // player에 맞게 수정 필요
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].SetSlot();
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        for (int i = 0; i < handleSlots.Length; i++)
        {
            if (handleSlots[i].item != null)
            {
                handleSlots[i].SetSlot();
            }
            else
            {
                handleSlots[i].ClearSlot();
            }
        }
    }

    public ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackCount)
            {
                return slots[i];
            }
        }

        return null;
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    // 아이템 버릴 때 사용할 메서드
    private void ThrowItem(ItemData data)
    {
        Instantiate(data.dropItemPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    // 인벤토리 아이템 슬롯에 마우스를 올렸을 때 아이템의 설명을 보이기 위한 메서드
    public void MouseOnInventoryItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        onMouseItemName.text = selectedItem.displayName;
        onMouseItemDescription.text = selectedItem.description;
    }

    public override void Init()
    {
        // 해당 부분도 Player 로직에 맞게 변경
        controller = CharacterManager.Instance.Player.controller;
        dropPosition = CharacterManager.Instance.Player.dropPosition;
        CharacterManager.Instance.Player.addItem += AddItem;
        //controller.inventory += Toggle;

        inventoryWindow.SetActive(false);
        quickSlotWindow.SetActive(true);
        summaryBox.SetActive(false);

        slots = new ItemSlot[slotPanel.childCount];
        handleSlots = new HandleSlot[handleSlotPanel.childCount];
        //quickSlots = new HandleSlot[quickSlotPanel.childCount];

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

        // 퀵슬롯
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = quickSlotPanel.GetChild(i).GetComponent<HandleSlot>();
            quickSlots[i].index = i;
            quickSlots[i].inventory = this;
            quickSlots[i].ClearSlot();
        }

        //quickSlotPanel.gameObject.SetActive(true);

        // 인벤토리에서 아이콘에 커서를 갖다 대기 전 나올 아이템의 정보를 클리어
        ClearSelectedItemWindow();
    }

    public override void Enable()
    {
        inventoryWindow.SetActive(true);
        quickSlotWindow.SetActive(false);
        //quickSlotPanel.gameObject.SetActive(false);
    }

    public override void Disable()
    {
        if (IsOpenInventory())
        {
            inventoryWindow.SetActive(false);

            // 인벤토리를 닫을 때 게임 화면의 퀵슬롯에 데이터 복사
            //for (int i = 0; i < handleSlots.Length; i++)
            //{
            //    quickSlots[i].item = handleSlots[i].item;
            //    quickSlots[i].quantity = handleSlots[i].quantity;
            //    quickSlots[i].SetSlot();
            //}

            //quickSlotPanel.gameObject.SetActive(false);
        }
    }
}
