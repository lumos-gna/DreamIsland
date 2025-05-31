using System;
using UnityEngine;

public class EquippedConsume : EquippedItem
{
    private Inventory _inventory;
    
    private PlayerCondition _targetCondition;
    
    private bool _isRunning;
    
    private readonly int _consume = Animator.StringToHash("Consume");
    
    public override void Equip(GameObject user, ItemData itemData)
    {
        base.Equip(user, itemData);
        
        _inventory = GameManager.Instance.Inventory;
        
        _targetCondition = user.GetComponent<PlayerCondition>();
    }
    

    public override void UnEquip()
    {
    }
    
    
    public override bool TryUse(EquippedController.InputState inputState)
    {
        if (!ItemData.IsConsumable) return false;
        
        switch (inputState)
        {
            case EquippedController.InputState.Down :
                if (!_isRunning)
                {
                    _isRunning = true;
                    _animator.SetTrigger(_consume);

                    return true;
                }
                break;
        }

        return false;
    }
    

    public void StartConsume()
    {
    }
    

    public void FinishConsume()
    {
        var states = ItemData.ConsumeInfo.states;

        bool isSuccess = false;
        
        for (int i = 0; i < states.Length; i++)
        {
            var info = states[i];

            switch (info.type)
            {
                case ConditionType.health :
                    isSuccess = true;
                    _targetCondition.HealthChange(info.value);
                    break;
                case ConditionType.water :
                    isSuccess = true;
                    _targetCondition.WaterChange(info.value);
                    break;
            }
        }

        if (isSuccess)
        {
            _inventory.DecreaseItem(ItemData);
        }

        _isRunning = false;
    }
}
