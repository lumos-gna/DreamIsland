
using System;
using UnityEngine;

public abstract class EquippedItem : MonoBehaviour
{
    public  ItemData ItemData { get; protected set; }

    protected Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public virtual void Equip(GameObject user, ItemData itemData)
    {
        if (!itemData.IsEquippalbe) 
            return;
        
        ItemData = itemData;
    }
    
    public abstract void UnEquip();
    public abstract bool TryUse(EquippedController.InputState inputState);
    
}
