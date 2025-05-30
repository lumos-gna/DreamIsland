using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    public ItemData ItemData { get; private set; }

    public void Init(ItemData data)
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
