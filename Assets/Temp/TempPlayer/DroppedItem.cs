using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    public ItemDataSO ItemData { get; private set; }

    public void Init(ItemDataSO data)
    {
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
