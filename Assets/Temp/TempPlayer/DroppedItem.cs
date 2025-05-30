using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    public ItemDataSO ItemData { get; private set; }
    
    public string GetInteractPrompt()
    {
        string info = $"{ItemData.DisplayName}\n{ItemData.Description}";
        return info;
    }

    public void OnInteract()
    {
        //인벤토리로~
        Destroy(gameObject);
    }
}
