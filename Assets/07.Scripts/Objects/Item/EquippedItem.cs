using System;
using UnityEngine;

public abstract class EquippedItem : MonoBehaviour
{
    public  ItemData ItemData { get; protected set; }

    protected Animator _animator;

    protected EquippedController _controller;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public virtual void Equip(EquippedController controller, ItemData itemData)
    {
        if (!itemData.IsEquippalbe) 
            return;

        _controller = controller;
        
        ItemData = itemData;
    }

    public virtual void UnEquip()
    {
        Destroy(gameObject);
    }
    public abstract void Use();
    
}
