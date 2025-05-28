using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemData item;

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public UIInventory inventory;

    public int index;
    public bool equiped;
    public int quantity;

    private GameObject dragIcon;
    private RectTransform dragIconRect;

    public static ItemSlot draggedFromSlot; // 드래그 시작 슬롯

    // 슬롯 세팅
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

    // 슬롯 초기화
    public void ClearSlot()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
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

    // 마우스 포인터가 아이콘에서 벗어났을때
    public void OnPointerExit(PointerEventData eventData)
    {
        inventory.summaryBox.SetActive(false);
        inventory.ClearSelectedItemWindow();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        draggedFromSlot = this;

        // 드래그할 아이콘 생성
        dragIcon = new GameObject("DragIcon", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
        dragIcon.transform.SetParent(inventory.transform, false);   // UIInventory 하위에 생성
        dragIcon.transform.SetAsLastSibling();  // 맨 앞에서 렌더링

        dragIconRect = dragIcon.GetComponent<RectTransform>();
        dragIconRect.sizeDelta = icon.rectTransform.sizeDelta;

        var image = dragIcon.GetComponent<Image>();
        image.sprite = icon.sprite;
        image.raycastTarget = false;

        // 투명도 설정
        var group = dragIcon.GetComponent<CanvasGroup>();
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventory.transform as RectTransform, Input.mousePosition, null, out pos);
            dragIconRect.localPosition = pos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggedFromSlot = null;
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (draggedFromSlot == null || draggedFromSlot == this) return;

        SwapWith(draggedFromSlot);
    }

    public void SwapWith(ItemSlot other)
    {
        // 자기 자신에 드롭한 경우는 무시함
        if (this == other) return;

        // 한쪽이 비었을 경우에 이동
        if (this.item == null && other.item != null)
        {
            this.item = other.item;
            this.quantity = other.quantity;

            other.item = null;
            other.quantity = 0;
        }
        else if (this.item != null && other.item == null)
        {
            other.item = this.item;
            other.quantity = this.quantity;

            this.item = null;
            this.quantity = 0;
        }
        else
        {
            // 둘 다 아이템이 있을 경우에는 아이템과 수량 정보 교환
            (this.item, other.item) = (other.item, this.item);
            (this.quantity, other.quantity) = (other.quantity, this.quantity);
        }

        this.SetSlot();
        other.SetSlot();
    }

    public void OnClickButton()
    {
        // UIInventory 관련 함수 작성
        // ex. SelectItem()
    }
}
