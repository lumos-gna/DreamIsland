using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Crafting/Recipe")]
public class CraftingRecipeSO : ScriptableObject
{
    [System.Serializable]
    public class Resource
    {
        public ResourceItemData data;
        public int amount;
    }
    
    public string recipeName;
    public List<Resource> Resources; // 필요 재료 목록
    public ItemData resultItem;          // 결과 아이템
}
