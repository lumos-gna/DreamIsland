using System;
using UnityEngine;

public class EquippedConsume : EquippedItem
{
    private static readonly int IsUse = Animator.StringToHash("IsUse");
    private PlayerCondition _targetCondition;
    private bool _isRunning;

    public override void Equip(EquippedController controller, ItemData itemData)
    {
        base.Equip(controller, itemData);

        _targetCondition = controller.GetComponent<PlayerCondition>();
    }

    public override void Use()
    {
        if (!ItemData.IsConsumable) return;

        if (_controller.IsInputDown)
        {
            if (!_isRunning)
            {
                _isRunning = true;

                _animator.SetBool(IsUse, true);
            }
        }

        if (_controller.IsInputUp)
        {
            if (_isRunning)
            {
                _isRunning = false;

                _animator.SetBool(IsUse, false);
            }
        }
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

            switch (info.type)
            {
                case ConditionType.health:
                    _targetCondition.HealthChange(info.value);
                    break;
                case ConditionType.water:
                    _targetCondition.WaterChange(info.value);
                    break;
            }
        }

        _animator.SetBool(IsUse, false);

        _controller.Inventory.DecreaseItem(ItemData);

        if (_controller.CurSlot.quantity == 0)
        {
            UnEquip();
        }

        _isRunning = false;
    }
}
