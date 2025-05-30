using UnityEngine;

public class EquippedRange : EquippedItem
{
    public override void Equip(GameObject user, ItemDataSO itemData)
    {
    }

    public override void UnEquip()
    {
    }

    public override bool TryUse()
    {
        return false;
    }
}