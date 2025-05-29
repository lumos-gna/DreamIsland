using UnityEngine;


[CreateAssetMenu(fileName = "ItemDataTable", menuName = "ScriptableObjects/ItemData/ItemData Table")]
public class ItemDataTable : ScriptableObject
{
    public TempItemData[] ItemDatas => itemDatas;
    
    [SerializeField] private TempItemData[] itemDatas;
}