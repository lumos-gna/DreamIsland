using System;
using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    //테스트용
    [SerializeField] private ItemData testItemData;

    private void Start()
    {
        Init(testItemData);
    }
    public ItemData ItemData { get; private set; }

    public void Init(ItemData data)
    {
        if (!data.IsDroppable) 
            return;
        
        ItemData = data;
    }

    public string GetInteractPrompt()
    {
        string info = $"{ItemData.DisplayName}\n{ItemData.Description}";
        return info;
    }

    public void OnInteract()
    {
        GameManager.Instance.Inventory.AddItem(ItemData);
        
        Destroy(gameObject);
    }
}
