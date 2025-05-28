using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Rock,
    Wood
}

[Serializable]
public class ResourceItem
{
    public ResourceType Type;
}

[CreateAssetMenu(fileName = "Item_", menuName = "ScriptableObjects/ItemData/Resource")]
public class ResourceItemData : ItemData
{
}
