using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDataSO initialData;
    public ItemDataSO ItemData { get; private set; }

    public void Awake()
    {
        // 씬에 직접 배치된 경우 initialData로 자동 초기화
        if (initialData != null)
            Init(initialData);
    }

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
