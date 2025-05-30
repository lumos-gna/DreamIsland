
using System;
using UnityEngine;

public abstract class EquippedItem : MonoBehaviour
{
    public  ItemData ItemData { get; protected set; }

    public abstract void Equip(GameObject user, ItemData itemData);
    public abstract void UnEquip();
    public abstract bool TryUse(EquippedController.InputState inputState);
    
}
