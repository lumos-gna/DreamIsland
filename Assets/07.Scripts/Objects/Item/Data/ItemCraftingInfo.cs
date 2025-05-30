using System.Collections.Generic;

[System.Serializable]
public class ItemCraftingInfo
{
    [System.Serializable]
    public struct Recipe
    {
        public ItemData data;
        public int amount;
    }

    
    public Recipe[] recipes;
    
    public ItemData resultItemData;       
}