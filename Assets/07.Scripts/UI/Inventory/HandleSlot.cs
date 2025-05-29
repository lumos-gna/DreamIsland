using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandleSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public static HandleSlot draggedFromHandleSlot;

    public ItemData item;

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public InventoryUI inventory;

    public int index;
    public bool equiped;
    public int quantity;

    private GameObject dragIcon;
    private RectTransform dragIconRect;

    public void ClearSlot()
    {
        item = null;
        quantity = 0;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void SetSlot()
    {
        if (item == null)
        {
            ClearSlot();
            return;
        }

        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    // 마우스 포인터가 아이콘 위에 있을때 감지
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            inventory.summaryBox.SetActive(true);
            inventory.MouseOnInventoryItem(index);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        draggedFromHandleSlot = this;

        // 드래그 아이콘 생성
        dragIcon = new GameObject("HandleSlotDragIcon", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
        dragIcon.transform.SetParent(inventory.transform, false);
        dragIcon.transform.SetAsLastSibling();

        dragIconRect = dragIcon.GetComponent<RectTransform>();
        dragIconRect.sizeDelta = icon.rectTransform.sizeDelta;

        Image image = dragIcon.GetComponent<Image>();
        image.sprite = icon.sprite;
        image.raycastTarget = false;

        CanvasGroup group = dragIcon.GetComponent<CanvasGroup>();
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon == null) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(inventory.transform as RectTransform, Input.mousePosition, null, out pos);
        dragIconRect.localPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }

        draggedFromHandleSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (ItemSlot.draggedFromSlot != null)
        {
            SwapWith(ItemSlot.draggedFromSlot);
        }
        else if (HandleSlot.draggedFromHandleSlot != null)
        {
            SwapWith(HandleSlot.draggedFromHandleSlot);
        }
    }

    public void SwapWith(ItemSlot other)
    {
        if (this == other) return;

        // 병합 로직 : 스택이 가능한, 같은 아이템들 병합
        if (this.item != null && other.item != null && this.item == other.item && item.canStack)
        {
            int total = this.quantity + other.quantity;

            if (total <= item.maxStackCount)
            {
                this.quantity = total;
                other.ClearSlot();  // 병합 완료 후 다른 슬롯 비우기
            }
            else
            {
                this.quantity = item.maxStackCount;
                other.quantity = total - item.maxStackCount;
            }

            this.SetSlot();
            other.SetSlot();
            return;
        }

        if (this.item == null & other.item != null)
        {
            this.item = other.item;
            this.quantity = other.quantity;
            other.ClearSlot();
        }
        else if (this.item != null && other.item == null)
        {
            other.item = this.item;
            other.quantity = this.quantity;
            this.ClearSlot();
        }
        else
        {
            (this.item, other.item) = (other.item, this.item);
            (this.quantity, other.quantity) = (other.quantity, this.quantity);
        }

        this.SetSlot();
        other.SetSlot();

        inventory.UpdateHandleSlotModel(index, item, quantity);
        inventory.UpdateItemSlotModel(other.index, other.item, other.quantity);
    }

    public void SwapWith(HandleSlot other)
    {
        if (this == other) return;

        // 병합 로직 : 스택이 가능한, 같은 아이템들 병합
        if (this.item != null && other.item != null && this.item == other.item && item.canStack)
        {
            int total = this.quantity + other.quantity;

            if (total <= item.maxStackCount)
            {
                this.quantity = total;
                other.ClearSlot();  // 병합 완료 후 다른 슬롯 비우기
            }
            else
            {
                this.quantity = item.maxStackCount;
                other.quantity = total - item.maxStackCount;
            }

            this.SetSlot();
            other.SetSlot();
            return;
        }

        (this.item, other.item) = (other.item, this.item);
        (this.quantity, other.quantity) = (other.quantity, this.quantity);

        this.SetSlot();
        other.SetSlot();

        inventory.UpdateHandleSlotModel(other.index, other.item, other.quantity);
        inventory.UpdateItemSlotModel(index, item, quantity);
    }
}
