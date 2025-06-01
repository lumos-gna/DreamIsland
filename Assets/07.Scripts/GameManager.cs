using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool IsLockedCursor { get; private set; }

    public Inventory Inventory
    {
        get
        {
            if (_inventory == null)
            {
                _inventory = new Inventory(itemSlotCount: 21, handleSlotCount: 7);
            }

            return _inventory;
        }
    }

    private PlayerCondition _playerCondition;
    private Inventory _inventory;

    private string _startScenename = "StartScene";

    private void Awake()
    {
        IsLockedCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIManager.Instance.ResetDict();
        InitializeUI();
    }

    private void InitializeUI()
    {

        UIManager.Instance.Enable<QuickSlotUI>();
        UIManager.Instance.Enable<AimUI>();
        UIManager.Instance.Enable<ConditionUI>();

        UIManager.Instance.Disable<InventoryUI>();
        UIManager.Instance.Disable<CraftingUI>();
        UIManager.Instance.Disable<GameOverUI>();
        UIManager.Instance.Disable<FadeUI>();
        UIManager.Instance.Disable<FlashUI>();
    }

    public void ToggleCursor(bool isLock)
    {
        IsLockedCursor = isLock;
        Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;

    }

    public void GameOver()
    {
        ToggleCursor(false); // 커서를 보이게 함
        UIManager.Instance.Get<GameOverUI>()?.Enable(); // 게임종료 UI on
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToStartScene()
    {
        SceneManager.LoadScene(_startScenename);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
    
    public void SetCursorLockState(bool isLocked)
    {
        IsLockedCursor = isLocked;
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }
}
