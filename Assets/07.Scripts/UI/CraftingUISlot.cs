using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftingUISlot : MonoBehaviour, IPoolable
{
    public Image HighLightImage => highLightImage;
    
    public ItemData Item { get; private set; }

    [SerializeField] private Image icon;
    [SerializeField] private Image highLightImage;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;
    

    public void Init(ItemData item, UnityAction onBtnClick)
    {
        Item = item;

        nameText.text = item.DisplayName;
        
        icon.sprite = item.Icon;
        
        button.onClick.AddListener(onBtnClick);
    }


    public void OnSpawn() =>  gameObject.SetActive(true);

    public void OnDespawn() => gameObject.SetActive(false);

}
