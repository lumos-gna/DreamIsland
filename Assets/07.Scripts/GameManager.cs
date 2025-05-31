using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsLockedCursor { get; private set; }

    public Inventory Inventory { get; private set; }

    private PlayerCondition _playerCondition;



    private void Awake()
    {
        Inventory = new Inventory(itemSlotCount: 21, handleSlotCount: 7);
        IsLockedCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        UIManager.Instance.Enable<QuickSlotUI>();
        UIManager.Instance.Enable<AimUI>();
        UIManager.Instance.Enable<ConditionUI>();

        UIManager.Instance.Disable<InventoryUI>();
        UIManager.Instance.Disable<CraftingUI>();
        UIManager.Instance.Disable<GameOverUI>();
    }

    public void ToggleCursor(bool isLock)
    {
        IsLockedCursor = isLock;
        Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void GameOver()
    {
        Time.timeScale = 0f; // 시간을 멈추고
        ToggleCursor(false); // 커서를 보이게 함
        UIManager.Instance.Get<GameOverUI>()?.Enable(); // 게임종료 UI on
    }
}
