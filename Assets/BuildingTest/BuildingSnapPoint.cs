using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSnapPoint : MonoBehaviour
{
    public enum SnapDirection
    {
        Top, 
        Bottom,
        Left,
        Right
    }

    public SnapDirection Direction => direction;
    
    [SerializeField] SnapDirection direction;
    
    
    public SnapDirection GetOppositeDir(SnapDirection dir)
    {
        return dir switch
        {
            SnapDirection.Left => SnapDirection.Right,
            SnapDirection.Right => SnapDirection.Left,
            SnapDirection.Top => SnapDirection.Bottom,
            SnapDirection.Bottom => SnapDirection.Top,
            _ => dir
        };
    }
}
