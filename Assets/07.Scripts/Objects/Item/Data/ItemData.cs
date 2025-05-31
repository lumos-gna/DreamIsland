using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/Data/Item Data")]
public class ItemData : ScriptableObject
{
     public string DisplayName => displayName;
     public string Description => description;
     
     public Sprite Icon => icon;

     

     public bool IsStackable => isStackable;
     public bool IsDroppable => isDroppable;
     public bool IsEquippalbe => isEquippable;
     public bool IsConsumable => isConsumable;
     public bool IsCraftable => isCraftable;
     public bool IsRangeItem => isRangeItem;
     public bool IsMeleeItem => isMeleeItem;
     public bool IsDamageable => isDamageable;
     public bool IsPlaceable => isPlaceable;


     public int MaxStackCount => maxStackCount;
     public DroppedItem DroppedPrefab => droppedPrefab;
     public EquippedItem EquippedPrefab => equippedPrefab;
     public ItemConsumeInfo ConsumeInfo => consumeInfo;
     public ItemCraftingInfo CraftingInfo => craftingInfo;
     public ItemRangeInfo RangeInfo => rangeInfo;
     public ItemMeleeInfo MeleeInfo => meleeInfo;
     public ItemDamageInfo DamageInfo => damageInfo;
     public ItemBuildingInfo BuildingInfo => buildingInfo;


     
     [Space(10f)]     
     [SerializeField] private string displayName;
     [SerializeField] private string description;
     
     [Space(10f)]
     [SerializeField] private Sprite icon;
     
   
     
     [Space(30f)]
     
     
     [Space(10f)]
     [SerializeField] private bool isStackable;
     
     [BoolShowIf("isStackable")]
     [SerializeField] private int maxStackCount;

     [Space(10f)] 
     [SerializeField] private bool isDroppable;
     
     
     [BoolShowIf("isDroppable")]
     [SerializeField] private DroppedItem droppedPrefab;
     
     [Space(10f)] 
     [SerializeField] private bool isEquippable;

     [BoolShowIf("isEquippable")] 
     [SerializeField] private  EquippedItem equippedPrefab;

     [Space(10f)] 
     [SerializeField] private bool isConsumable;
     
     [BoolShowIf("isConsumable")]
     [SerializeField] private ItemConsumeInfo consumeInfo;
     
     [Space(10f)] 
     [SerializeField] private bool isCraftable;

     [BoolShowIf("isCraftable")]
     [SerializeField] private ItemCraftingInfo craftingInfo;
     
     [Space(10f)] 
     [SerializeField] private bool isRangeItem;

     [BoolShowIf("isRangeItem")]
     [SerializeField] private ItemRangeInfo rangeInfo;

     [Space(10f)] 
     [SerializeField] private bool isMeleeItem;

     [BoolShowIf("isMeleeItem")]
     [SerializeField] private ItemMeleeInfo meleeInfo;
     
     [Space(10f)] 
     [SerializeField] private bool isDamageable;

     [BoolShowIf("isDamageable")]
     [SerializeField] private ItemDamageInfo damageInfo;
     
     [Space(10f)] 
     [SerializeField] private bool isPlaceable;
     
     [BoolShowIf("isPlaceable")]
     [SerializeField] private ItemBuildingInfo buildingInfo;
}