using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftingUISlot : MonoBehaviour, IPoolable
{
    public ItemData Item { get; private set; }

    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public void Init(ItemData item, UnityAction onBtnClick)
    {
        Item = item;
        
        icon.sprite = item.Icon;
        
        button.onClick.AddListener(onBtnClick);
    }


    public void OnSpawn() =>  gameObject.SetActive(true);

    public void OnDespawn() => gameObject.SetActive(false);
}
