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

    public void GameOverBtn()
    {
        GameManager.Instance.GameOver();
    }

    public void GoToStartSceneBtn()
    {
        GameManager.Instance.GoToStartScene();
    }

    public void ReStartBtn()
    {
        GameManager.Instance.ReStart();
    }

    public void ExitBtn()
    {
        GameManager.Instance.Exit();
    }
}
