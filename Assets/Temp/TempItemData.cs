using UnityEngine;

public abstract class TempItemData : ScriptableObject
{
    public string displayName;
    
    public string description;


    public bool IsStackable => maxStackCount > 1;
    
    public int maxStackCount;

    public Sprite icon;
    
    public GameObject dropItemPrefab;
}