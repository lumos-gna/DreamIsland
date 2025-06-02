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

    public int index;
    public bool equiped;
    public int quantity;

    [SerializeField] private Image hightLightImage;

    private GameObject dragIcon;
    private RectTransform dragIconRect;
    private Color originalColor;
    private Color originalIconColor;

    private void Start()
    {
        originalColor = hightLightImage.color;
        originalIconColor = Color.white;
    }

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
        icon.sprite = item.Icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    public void SetHighlight(bool active)
    {
        if (hightLightImage != null)
        {
            hightLightImage.color = active ? Color.white : originalColor;
        }
    }

    // 마우스 포인터가 아이콘 위에 있을때 감지
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            var inventoryUI = UIManager.Instance.Get<InventoryUI>();
            inventoryUI.summaryBox.SetActive(true);
            inventoryUI.MouseOnInventoryItem(index);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        draggedFromHandleSlot = this;

        // 드래그 아이콘 생성
        dragIcon = new GameObject("HandleSlotDragIcon", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
        dragIcon.transform.SetParent(UIManager.Instance.Get<InventoryUI>().transform, false);
        dragIcon.transform.SetAsLastSibling();

        dragIconRect = dragIcon.GetComponent<RectTransform>();
        dragIconRect.sizeDelta = icon.rectTransform.sizeDelta;

        Image image = dragIcon.GetComponent<Image>();
        image.sprite = icon.sprite;
        image.raycastTarget = false;

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
                null, out Vector2 pos);
            dragIconRect.localPosition = pos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.color = Color.white;
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }

        draggedFromHandleSlot = null;
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

        // handleSlot에 저장
        if (index >= 0 && index < inventory.handleSlots.Length)
        {
            inventory.handleSlots[index].item = item;
            inventory.handleSlots[index].quantity = quantity;
        }

        // itemSlot에 저장
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

        if (index >= 0 && index < inventory.handleSlots.Length)
        {
            inventory.handleSlots[index].item = item;
            inventory.handleSlots[index].quantity = quantity;
        }

        if (other.index >= 0 && other.index < inventory.handleSlots.Length)
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
