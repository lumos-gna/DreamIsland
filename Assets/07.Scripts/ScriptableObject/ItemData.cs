using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Resource,
    Consumable,
    Building
}

public enum ConsumType
{
    health,
    water,
    hunger,
}

public abstract class ItemData : ScriptableObject
{
    [Header("Information")]
    public ItemType type;
    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject dropItemPrefab;

    [Header("Stack")]
    public bool canStack;
    public int maxStackCount;

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Attack")]
    public int AttackDamage;
    public int AttackRange;

    [Header("GatherResource")]
    public int objectDamage;

    [Header("Consume")]
    public ConsumType consumetype;
    public float healamount;

}
