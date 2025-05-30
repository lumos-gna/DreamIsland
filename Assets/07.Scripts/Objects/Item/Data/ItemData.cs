using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/Data/Item Data")]
public class ItemData : ScriptableObject
{
     public string DisplayName => displayName;
     public string Description => description;
     public bool IsStackable => maxStackCount > 1;
     public int MaxStackCount => maxStackCount;
     public Sprite Icon => icon;
     public DroppedItem DropItemPrefab => dropItemPrefab;
     public EquippedItem EquipItemPrefab => equipItemPrefab;

     public bool IsConsumeItem => isConsumeItem;
     public ItemConsumeInfo ConsumeInfo => consumeInfo;
     public bool IsCraftItem => isCraftItem;
     public ItemCraftingInfo CraftingInfo => craftingInfo;
     public bool IsRangeItem => isRangeItem;
     public ItemRangeInfo RangeInfo => rangeInfo;
     public bool IsMeleeItem => isMeleeItem;
     public ItemMeleeInfo MeleeInfo => meleeInfo;
     public bool IsDamageItem => isDamageItem;
     public ItemDamageInfo DamageInfo => damageInfo;
     public bool IsBuildingItem => isBuildingItem;
     public ItemBuildingInfo BuildingInfo => buildingInfo;
     
     
     
     [SerializeField] private string displayName;
     [SerializeField] private string description;
     
     [Space(10f)]
     [SerializeField] private int maxStackCount;
     
     [Space(10f)]
     [SerializeField] private Sprite icon;
     
     [Space(10f)]
     [SerializeField] private DroppedItem dropItemPrefab;
     [SerializeField] private EquippedItem equipItemPrefab;


     [Space(20f)] 
     [SerializeField] private bool isConsumeItem;
     
     [BoolShowIf("isConsumeItem")]
     [SerializeField] private ItemConsumeInfo consumeInfo;
     
     
     [Space(10f)] 
     [SerializeField] private bool isCraftItem;

     [BoolShowIf("isCraftItem")]
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
     [SerializeField] private bool isDamageItem;

     [BoolShowIf("isDamageItem")]
     [SerializeField] private ItemDamageInfo damageInfo;

     
     [Space(10f)] 
     [SerializeField] private bool isBuildingItem;
     
     [BoolShowIf("isBuildingItem")]
     [SerializeField] private ItemBuildingInfo buildingInfo;
}