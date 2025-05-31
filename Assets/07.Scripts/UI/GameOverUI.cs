using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : BaseUI
{
    public override bool IsEnabled => gameObject.activeInHierarchy;

    public override void Disable()
    {
        gameObject.SetActive(false);
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
    }

    public override void Init()
    {
        gameObject.SetActive(false);
    }
}
