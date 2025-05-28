using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item;

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public UIInventory inventory;

    public int index;
    public bool equiped;
    public int quantity;

    public void SetSlot()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    public void ClearSlot()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            inventory.summaryBox.SetActive(true);
            inventory.MouseOnInventoryItem(index);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventory.summaryBox.SetActive(false);
        inventory.ClearSelectedItemWindow();
    }

    public void OnClickButton()
    {
        // UIInventory 관련 함수 작성
        // ex. SelectItem()
    }
}
