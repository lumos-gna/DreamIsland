using UnityEngine;

public abstract class ItemDataSO : ScriptableObject
{
     [SerializeField] private string displayName;
    
     [SerializeField] private string description;
    
     [SerializeField] private int maxStackCount;

     [SerializeField] private Sprite icon;
    
     [SerializeField] private DroppedItem dropItemPrefab;
     
     [SerializeField] private EquippedItem equipItemPrefab;

 
     public string DisplayName => displayName;
     
     public string Description => description;

     public bool IsStackable => maxStackCount > 1;
    
     public int MaxStackCount => maxStackCount;

     public Sprite Icon => icon;
     
     public DroppedItem DropItemPrefab => dropItemPrefab;
     public EquippedItem EquipItemPrefab => equipItemPrefab;
     

}