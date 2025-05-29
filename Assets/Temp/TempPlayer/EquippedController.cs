using System;
using UnityEngine;

public class EquippedController : MonoBehaviour
{
    [SerializeField] private ItemDataSO tempItemData;
    [SerializeField] private Transform equipParent;
    
    public EquippedItem CurEquippedItem { get; set; }

    private void Start()
    {
        CurEquippedItem = Instantiate(tempItemData.EquipItemPrefab, equipParent);
        CurEquippedItem.Equip(gameObject, tempItemData);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (CurEquippedItem.TryUse())
            {
                //인벤토리 슬롯 개수 갱신?
            }
        }
       
    }
}
