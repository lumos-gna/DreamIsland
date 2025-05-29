using System.Collections.Generic;


[System.Serializable]
public class CraftingRecipe
{
    [System.Serializable]
    public class ItemAmount
    {
        public ItemDataSO data;
        public int amount;
    }
    
    public List<ItemAmount> neededItem;
    
    public ItemDataSO resultItemData;          
}
