using UnityEngine;


[CreateAssetMenu(fileName = "ItemDataTable", menuName = "ScriptableObjects/ItemData/ItemData Table")]
public class ItemDataTableSO : ScriptableObject
{
    public ItemDataSO[] ItemDatas => itemDatas;
    
    [SerializeField] private ItemDataSO[] itemDatas;
}