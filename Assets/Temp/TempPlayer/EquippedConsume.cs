using System;
using UnityEngine;

public class EquippedConsume : EquippedItem
{
    [SerializeField] private Animator animator;
    
    
    private PlayerCondition _targetCondition;
    
    private bool _isRunning;
    
    private readonly int _consume = Animator.StringToHash("Consume");
    
    public override void Equip(GameObject user, ItemDataSO itemData)
    {
        ItemData = itemData;

        if (user.TryGetComponent(out PlayerCondition condition))
        {
            _targetCondition = condition;
        }
    }

    public override void UnEquip()
    {
    }
    
    public override bool TryUse(EquippedController.InputState inputState)
    {
        if (!ItemData.IsConsumeItem) return false;
        
        switch (inputState)
        {
            case EquippedController.InputState.Down :
                if (!_isRunning)
                {
                    _isRunning = true;
                    animator.SetTrigger(_consume);

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
        
        for (int i = 0; i < states.Length; i++)
        {
            var info = states[i];

            switch (info.consumetype)
            {
                case ConsumType.health :
                    _targetCondition.HealthChange(info.value);
                    break;
                case ConsumType.water :
                    _targetCondition.WaterChange(info.value);
                    break;
            }
        }

        _isRunning = false;
    }
}
