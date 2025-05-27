using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    Health,     // 체력
    Thirsty,    // 목마름
    Stamina,    // 스태미나
}

[Serializable]
public class ConsumableItem
{
    public ConsumableType Type;
    public float value;
}

[CreateAssetMenu(fileName = "Item_", menuName = "ScriptableObjects/ItemData/Counsumable")]
public class ConsumableItemData : ItemData
{
    [Header("Consumable attribute")]
    public ConsumableItem[] consumables;
}
