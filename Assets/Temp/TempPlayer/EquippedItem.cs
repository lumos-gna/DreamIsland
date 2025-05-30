
using System;
using UnityEngine;

public abstract class EquippedItem : MonoBehaviour
{
    public abstract ItemDataSO ItemData { get; }
    public abstract void Equip(GameObject user, ItemDataSO itemData);
    public abstract void UnEquip();
    public abstract bool TryUse(EquippedController.InputState inputState);
    
}
