using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUI : BaseUI
{
    public override bool IsEnabled => gameObject.activeInHierarchy;

    override public void Init()
    {
        gameObject.SetActive(true);
    }

    override public void Enable()
    {
        gameObject.SetActive(true);
    }

    override public void Disable()
    {
        gameObject.SetActive(false);
    }
}
