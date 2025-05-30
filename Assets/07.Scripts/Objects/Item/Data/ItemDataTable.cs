using UnityEngine;


[CreateAssetMenu(fileName = "ItemDataTable", menuName = "ScriptableObjects/Data/ItemData Table")]
public class ItemDataTable : ScriptableObject
{
    public ItemData[] ItemDatas => itemDatas;
    
    [SerializeField] private ItemData[] itemDatas;
}