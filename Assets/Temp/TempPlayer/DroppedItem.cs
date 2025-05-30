using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    public ItemDataSO ItemData { get; private set; }

    // 테스트용
    //public ItemDataSO ItemData;

    public string GetInteractPrompt()
    {
        string info = $"{ItemData.DisplayName}\n{ItemData.Description}";
        return info;
    }

    public void OnInteract()
    {
        GameManager.Instance.AddItem(ItemData);
        Destroy(gameObject);
    }
}
