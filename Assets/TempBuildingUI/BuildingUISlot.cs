using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingUISlot : MonoBehaviour
{
    public ItemData ItemData { get; private set; }

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button button;

    public void InitToItemSlot(ItemData itemData, UnityAction onBtnClick)
    {
        ItemData = itemData;
        
        nameText.text = itemData.name;
        countText.text = "";
        
        button.onClick.AddListener(onBtnClick);
    }

    public void InitToRecipe(ItemData itemData, int count)
    {
        ItemData = itemData;
        
        nameText.text = itemData.name;
        countText.text = count.ToString();
    }
}
