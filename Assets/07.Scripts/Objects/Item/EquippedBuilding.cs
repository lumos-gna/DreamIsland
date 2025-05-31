using System;
using UnityEngine;


public class EquippedBuilding : EquippedItem
{
    private BuildingSystem _buildingSystem;

    private BuildingObject _prefab;

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
        base.Equip(user, itemData);

        if (!itemData.IsPlaceable)
            return;
        
        _prefab = ItemData.BuildingInfo.prefab;
        
        _buildingSystem = new();
        
        _buildingSystem.Create(_prefab);
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
                    _buildingSystem.Create(_prefab);

                    return true;
                }
                break;
        }

        return false;
    }
}