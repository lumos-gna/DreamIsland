using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSnapPoint : MonoBehaviour
{
    public enum SnapAxis
    {
        Vertical,
        Horizontal
    }

    public SnapAxis Axis => axis;
    
    [SerializeField] SnapAxis axis;
}
