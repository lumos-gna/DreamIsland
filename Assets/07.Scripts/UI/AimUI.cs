using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUI : BaseUI
{
    [SerializeField] Canvas AimUICanvas;
    override public void Init()
    {
        AimUICanvas.gameObject.SetActive(true);
    }

    override public void Enable()
    {
        AimUICanvas.gameObject.SetActive(true);
    }

    override public void Disable()
    {
        AimUICanvas.gameObject.SetActive(false);
    }
}
