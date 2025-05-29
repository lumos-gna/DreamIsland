

public class ItemInstance
{
    public TempItemData ItemData { get; private set; }
    
    public ItemInstance(TempItemData itemData)
    {
        ItemData = itemData;
    }


    public CraftingRecipe GetRecipe() => ItemData is ICraftable craftable ? craftable.CraftingRecipe : null;

}