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

public abstract class ItemData : ScriptableObject
{
    [Header("Information")]
    public ItemType type;
    public string displayName;
    public string description;
    public GameObject dropItemPrefab;

    [Header("Stack")]
    public bool canStack;
    public int maxStackCount;

    [Header("Equip")]
    public GameObject equipPrefab;
}
