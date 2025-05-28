using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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


    // 슬롯 세팅
    public void SetSlot()
    {
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


    public void OnClickButton()
    {
        // UIInventory 관련 함수 작성
        // ex. SelectItem()
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
