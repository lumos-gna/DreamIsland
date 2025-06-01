using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemData item;

    public Image icon;
    public TextMeshProUGUI quantityText;

    public int index;
    public bool equiped;
    public int quantity;

    private GameObject dragIcon;
    private RectTransform dragIconRect;
    private Color originalIconColor;

    public static InventorySlotUI DraggedFromSlotUI; // 드래그 시작 슬롯

    private void Start()
    {
        originalIconColor = Color.white;
    }

    // 슬롯 세팅
    public void SetSlot()
    {
        if (item == null)
        {
            ClearSlot();
            return;
        }

        icon.gameObject.SetActive(true);
        icon.sprite = item.Icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    // 슬롯 초기화
    public void ClearSlot()
    {
        item = null;
        quantity = 0;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    // 마우스 포인터가 아이콘 위에 있을때 감지
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        var inventoryUI = UIManager.Instance.Get<InventoryUI>();
        inventoryUI.summaryBox.SetActive(true);
        inventoryUI.MouseOnInventoryItem(index);
    }

    // 마우스 포인터가 아이콘에서 벗어났을때
    public void OnPointerExit(PointerEventData eventData)
    {
        var inventoryUI = UIManager.Instance.Get<InventoryUI>();
        inventoryUI.summaryBox.SetActive(false);
        inventoryUI.ClearSelectedItemWindow();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        DraggedFromSlotUI = this;

        // 드래그할 아이콘 생성
        dragIcon = new GameObject("DragIcon", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
        dragIcon.transform.SetParent(UIManager.Instance.Get<InventoryUI>().transform, false);   // UIInventory 하위에 생성
        dragIcon.transform.SetAsLastSibling();  // 맨 앞에서 렌더링

        dragIconRect = dragIcon.GetComponent<RectTransform>();
        dragIconRect.sizeDelta = icon.rectTransform.sizeDelta;

        var image = dragIcon.GetComponent<Image>();
        image.sprite = icon.sprite;
        image.raycastTarget = false;

        // 투명도 설정
        dragIcon.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            icon.color = Color.clear;
            quantityText.text = string.Empty;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                UIManager.Instance.Get<InventoryUI>().transform as RectTransform,
                Input.mousePosition,
                null,
                out Vector2 pos);
            dragIconRect.localPosition = pos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DraggedFromSlotUI = null;
        icon.color = Color.white;
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (InventorySlotUI.DraggedFromSlotUI != null)
        {
            SwapWith(InventorySlotUI.DraggedFromSlotUI);
        }
        else if (HandleSlot.draggedFromHandleSlot != null)
        {
            SwapWith(HandleSlot.draggedFromHandleSlot);
        }
    }

    public void SwapWith(InventorySlotUI other)
    {
        // 자기 자신에 드롭한 경우는 무시함
        if (this == other) return;

        bool stacked = TryStack(other.item, other.quantity, other);
        if (!stacked)
        {
            (this.item, other.item) = (other.item, this.item);
            (this.quantity, other.quantity) = (other.quantity, this.quantity);
        }

        SetSlot();
        other.SetSlot();

        var inventory = GameManager.Instance.Inventory;
        if (index >= 0 && index < inventory.itemSlots.Length)
        {
            inventory.itemSlots[index].item = item;
            inventory.itemSlots[index].quantity = quantity;
        }

        if (other.index >= 0 && other.index < inventory.itemSlots.Length)
        {
            inventory.itemSlots[other.index].item = other.item;
            inventory.itemSlots[other.index].quantity = other.quantity;
        }

        inventory.ForceSync();
    }

    public void SwapWith(HandleSlot other)
    {
        var inventory = GameManager.Instance.Inventory;
        if (this == other) return;

        bool stacked = TryStack(other.item, other.quantity, other);
        if (!stacked)
        {
            (this.item, other.item) = (other.item, this.item);
            (this.quantity, other.quantity) = (other.quantity, this.quantity);
        }

        SetSlot();
        other.SetSlot();

        if (index >= 0 && index < inventory.itemSlots.Length)
        {
            inventory.itemSlots[index].item = item;
            inventory.itemSlots[index].quantity = quantity;
        }

        if (other.index >= 0 && other.index < inventory.itemSlots.Length)
        {
            inventory.handleSlots[other.index].item = other.item;
            inventory.handleSlots[other.index].quantity = other.quantity;
        }

        inventory.ForceSync();
    }

    private bool TryStack(ItemData otherItem, int otherQuantity, HandleSlot otherSlot = null)
    {
        if (item != null && otherItem != null && item == otherItem && item.IsStackable)
        {
            int total = quantity + otherQuantity;
            if (total <= item.MaxStackCount)
            {
                quantity = total;
                if (otherSlot != null)
                {
                    otherSlot.ClearSlot();
                }
                return true;
            }
            else
            {
                quantity = item.MaxStackCount;
                if (otherSlot != null)
                {
                    otherSlot.quantity = total - item.MaxStackCount;
                }
                return true;
            }
        }
        return false;
    }

    private bool TryStack(ItemData otherItem, int otherQuantity, InventorySlotUI otherSlotUI)
    {
        if (item != null && otherItem != null && item == otherItem && item.IsStackable)
        {
            int total = quantity + otherQuantity;
            if (total <= item.MaxStackCount)
            {
                quantity = total;
                if (otherSlotUI != null)
                {
                    otherSlotUI.ClearSlot();
                }
                return true;
            }
            else
            {
                quantity = item.MaxStackCount;
                if (otherSlotUI != null)
                {
                    otherSlotUI.quantity = total - item.MaxStackCount;
                }
                return true;
            }
        }
        return false;
    }
}
