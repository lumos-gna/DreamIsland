using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Resource,
    Consumable
}

public enum ConsumableType
{
    Health,     // 체력
    Thirsty,    // 목마름
    Stamina,    // 스태미나
}

[SerializeField]
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

    [Header("Consumable data")]
    public ConsumableItem[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}
