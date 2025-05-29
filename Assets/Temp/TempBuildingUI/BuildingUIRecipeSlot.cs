using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingUIRecipeSlot : MonoBehaviour, IPoolable
{
    public ItemInstance Item { get; private set; }

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI countText;

    private int _maxCount;
    private int _curCount;

    public void Init(ItemInstance item, int count)
    {
        Item = item;
        
        nameText.text = item.ItemData.displayName;

        countText.text = count.ToString();
    }

    public void OnSpawn() =>  gameObject.SetActive(true);

    public void OnDespawn() => gameObject.SetActive(false);
}
