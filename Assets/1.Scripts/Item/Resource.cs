using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour, IAttackable, IBuildable
{
    public bool CanBuild()
    {
        return true;
    }

    public void CloseRangeAttack(float range, float damage)
    {

    }

    public void LongRangeAttack(float range, float damage)
    {

    }
}
