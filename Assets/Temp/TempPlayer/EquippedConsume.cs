using System;
using UnityEngine;

public class EquippedConsume : EquippedItem
{
    [SerializeField] private Animator animator;
    
    private ConsumeItemDataSO _itmeData;

    private PlayerCondition _targetCondition;
    
    private bool _isRunning;
    
    private readonly int _eating = Animator.StringToHash("Eating");
    
    public override void Equip(GameObject user, ItemDataSO itemData)
    {
        _itmeData = itemData as ConsumeItemDataSO;

        if (user.TryGetComponent(out PlayerCondition condition))
        {
            _targetCondition = condition;
        }
    }

    public override void UnEquip()
    {
    }

    public override void Use()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            animator.SetTrigger(_eating);
        }
    }

    public void EndEating()
    {
        for (int i = 0; i < _itmeData.Infos.Length; i++)
        {
            var info = _itmeData.Infos[i];

            switch (info.consumetype)
            {
                case ConsumType.health :
                    _targetCondition.HealthChange(info.value);
                    break;
                case ConsumType.water :
                    _targetCondition.StaminaChange(info.value);
                    break;
            }
        }

        _isRunning = false;
    }
}
