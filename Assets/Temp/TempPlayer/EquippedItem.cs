
using System;
using UnityEngine;

public abstract class EquippedItem : MonoBehaviour
{
    public abstract void Equip(ItemDataSO itemData);
    public abstract void UnEquip();
    public abstract void Use();
    
}
