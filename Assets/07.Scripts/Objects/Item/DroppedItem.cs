using System;
using UnityEngine;


public class DroppedItem : MonoBehaviour, IInteractable
{
    public Outline Outline => _outline;

    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    
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



    public void OnInteract()
    {
        GameManager.Instance.Inventory.AddItem(ItemData);
        
        Destroy(gameObject);
    }
}
