using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandleSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public static HandleSlot draggedFromHandleSlot;
    
    public ItemData item;
    public Image icon;
    public TextMeshProUGUI quantityText;

    public int index;
    public int quantity;

    [SerializeField] private Image hightLightImage;

    private GameObject _dragIcon;
    private RectTransform _dragIconRect;
    private Color _originalColor;

    private void Start()
    {
        _originalColor = hightLightImage.color;
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
            hightLightImage.color = active ? Color.white : _originalColor;
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
        _dragIcon = new GameObject("HandleSlotDragIcon", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
        _dragIcon.transform.SetParent(UIManager.Instance.Get<InventoryUI>().transform, false);
        _dragIcon.transform.SetAsLastSibling();

        _dragIconRect = _dragIcon.GetComponent<RectTransform>();
        _dragIconRect.sizeDelta = icon.rectTransform.sizeDelta;

        Image image = _dragIcon.GetComponent<Image>();
        image.sprite = icon.sprite;
        image.raycastTarget = false;

        _dragIcon.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragIcon != null)
        {
            icon.color = Color.clear;
            quantityText.text = string.Empty;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                UIManager.Instance.Get<InventoryUI>().transform as RectTransform,
                Input.mousePosition,
                null, out Vector2 pos);
            _dragIconRect.localPosition = pos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.color = Color.white;
        if (_dragIcon != null)
        {
            Destroy(_dragIcon);
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
