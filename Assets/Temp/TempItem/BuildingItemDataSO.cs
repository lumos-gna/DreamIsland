using UnityEngine;



[CreateAssetMenu(fileName = "BuildingItemData", menuName = "ScriptableObjects/Temp/Building Item Data")]
public class BuildingItemDataSO : ItemDataSO, ICraftable
{
    public CraftingRecipe CraftingRecipe => craftingRecipe;
    

    [SerializeField] private CraftingRecipe craftingRecipe;
}