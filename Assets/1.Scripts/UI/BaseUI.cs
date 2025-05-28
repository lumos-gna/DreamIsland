using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class BaseUI : MonoBehaviour
{
    public UIType UIType;

    public abstract void Init();

    public abstract void Enable();

    public abstract void Disable();
}
