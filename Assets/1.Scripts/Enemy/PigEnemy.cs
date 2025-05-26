using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PigEnemy : BaseEnemy
{
    private void OnDrawGizmosSelected()
    {
        if (Stats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Stats.DetectDistance);
        }
    }
}
