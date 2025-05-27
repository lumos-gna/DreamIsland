using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Resource,
    Consumable
}

[Serializable]
public class ConsumableItem
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item_", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Information")]
    public ItemType type;
    public string displayName;
    public string description;
    public float value;
    public GameObject dropItemPrefab;

    [Header("Stack")]
    public bool canStack;
    public int maxStackCount;

    [Header("Consumable")]
    public ConsumableItem[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}
