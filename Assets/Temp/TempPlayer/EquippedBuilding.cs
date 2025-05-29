using System;
using UnityEngine;


public class EquippedBuilding : EquippedItem
{
    private BuildingSystem _buildingSystem;

    private BuildingItemDataSO _itmeData;

    private void Update()
    {
        _buildingSystem.UpdateBuildingObject();

        if (Input.GetKeyDown(KeyCode.R))
        {
            _buildingSystem.Rotation();
        }
    }

    public override void Equip(ItemDataSO itemData)
    {
        _itmeData = itemData as BuildingItemDataSO;
        

        _buildingSystem = new();
        
        _buildingSystem.Create(_itmeData.BuildingObjectPrefab);
    }

    public override void UnEquip()
    {
        _buildingSystem.Destroy();

    }

    public override void Use()
    {
        if (_buildingSystem.TryBuild())
        {
            _buildingSystem.Create(_itmeData.BuildingObjectPrefab);
        }
    }
}