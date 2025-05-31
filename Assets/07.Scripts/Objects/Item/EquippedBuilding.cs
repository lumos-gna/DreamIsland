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

    public override void Equip(EquippedController controller, ItemData itemData)
    {
        base.Equip(controller, itemData);

        if (!itemData.IsPlaceable)
            return;
        
        _prefab = ItemData.BuildingInfo.prefab;
        
        _buildingSystem = new();
        
        _buildingSystem.Create(_prefab);
    }

    public override void UnEquip()
    {
        base.UnEquip();
        
        _buildingSystem.Destroy();
    }
    
    public override void Use()
    {
        if (_controller.IsInputDown)
        {
            if (_buildingSystem.TryBuild())
            {
                _buildingSystem.Create(_prefab);
                
                _controller.Inventory.DecreaseItem(ItemData);
            
                if(_controller.CurSlot.quantity == 0)
                {
                    UnEquip();
                }
            }
        }
    }
}