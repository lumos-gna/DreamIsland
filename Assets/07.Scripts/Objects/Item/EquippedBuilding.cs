using System;
using UnityEngine;


public class EquippedBuilding : EquippedItem
{
    private BuildingSystem _buildingSystem;

    private void Update()
    {
        _buildingSystem.UpdateBuildingObject();

        if (Input.GetKeyDown(KeyCode.R))
        {
            _buildingSystem.Rotation();
        }
    }


    public override void Equip(GameObject user, ItemData itemData)
    {
        ItemData = itemData;
        
        _buildingSystem = new();
        
        _buildingSystem.Create(ItemData.BuildingInfo.buildingObjectPrefab);
    }

    public override void UnEquip()
    {
        _buildingSystem.Destroy();
    }
    
    public override bool TryUse(EquippedController.InputState inputState)
    {
        switch (inputState)
        {
            case EquippedController.InputState.Down :
                if (_buildingSystem.TryBuild())
                {
                    _buildingSystem.Create(ItemData.BuildingInfo.buildingObjectPrefab);

                    return true;
                }
                break;
        }

        return false;
    }
}