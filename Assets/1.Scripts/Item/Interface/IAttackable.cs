using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public void LongRangeAttack(float range, float damage);
    public void CloseRangeAttack(float range, float damage);
}
