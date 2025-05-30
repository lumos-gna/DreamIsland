using System;

public class GameManager : Singleton<GameManager>
{
    public Inventory Inventory { get; private set; }

    private void Awake()
    {
        Inventory = new Inventory(itemSlotCount: 21, handleSlotCount: 7);
    }

    private void Start()
    {
        UIManager.Instance.Create<InventoryUI>();
        UIManager.Instance.Create<QuickSlotUI>();
        UIManager.Instance.Create<AimUI>();
        UIManager.Instance.Create<ConditionUI>();
    }
}
