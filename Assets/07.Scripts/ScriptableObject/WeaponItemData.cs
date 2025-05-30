using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Ranged,
    Melee
}

[Serializable]
public class WeaponItem
{
    public WeaponType Type;
}

[CreateAssetMenu(fileName = "Item_", menuName = "ScriptableObjects/ItemData/Weapon")]
public class WeaponItemData : ItemData
{
    [Header("Weapon attribute")]
    public WeaponItem[] weapons;

    
    [SerializeField] private CraftingRecipe craftingRecipe;
}
