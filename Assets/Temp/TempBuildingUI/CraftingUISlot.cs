using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftingUISlot : MonoBehaviour, IPoolable
{
    public ItemDataSO Item { get; private set; }

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;

    public void Init(ItemDataSO item, UnityAction onBtnClick)
    {
        Item = item;
        
        nameText.text = item.DisplayName;
        
        button.onClick.AddListener(onBtnClick);
    }


    public void OnSpawn() =>  gameObject.SetActive(true);

    public void OnDespawn() => gameObject.SetActive(false);
}
