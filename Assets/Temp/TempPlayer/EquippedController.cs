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
            CurEquippedItem.Use();
        }
       
    }
}
