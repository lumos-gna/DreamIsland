using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    LongRange,
    CloseRange
}

public class Weapon : MonoBehaviour, IAttackable
{
    public void CloseRangeAttack(float range, float damage)
    {

    }

    public void LongRangeAttack(float range, float damage)
    {

    }
}
