using UnityEngine;



[CreateAssetMenu(fileName = "BuildingItemData", menuName = "ScriptableObjects/Temp/Building Item Data")]
public class BuildingItemDataSO : ItemDataSO
{
    public CraftingRecipe CraftingRecipe => craftingRecipe;
    public BuildingObject BuildingObjectPrefab => buildingObjectPrefab;
    
    
    [SerializeField] private CraftingRecipe craftingRecipe;

    [SerializeField] private BuildingObject buildingObjectPrefab;
}