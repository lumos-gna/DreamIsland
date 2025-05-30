using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIRecipeSlot : MonoBehaviour, IPoolable
{
    public ItemData Item { get; private set; }

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;

    private int _maxCount;
    private int _curCount;

    public void Init(ItemData item, int count)
    {
        Item = item;

        icon.sprite = item.Icon;

        _maxCount = count;
        
        countText.text = $"{_curCount}/{_maxCount}";
    }

    public void OnSpawn() =>  gameObject.SetActive(true);

    public void OnDespawn() => gameObject.SetActive(false);
}
