using UnityEngine;

public class QuickSlotUI : BaseUI
{
    public override bool IsEnabled => gameObject.activeInHierarchy;

    public HandleSlot[] quickSlots;
    public Transform quickSlotPanel;

    private Inventory _inventory;

    private int _currentHighlightedIndex = -1;

    public override void Init()
    {
        quickSlots = new HandleSlot[quickSlotPanel.childCount];

        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = quickSlotPanel.GetChild(i).GetComponent<HandleSlot>();
            quickSlots[i].index = i;

            quickSlots[i].ClearSlot();
        }

        _inventory = GameManager.Instance.Inventory;

        _inventory.OnChangedInventory += UpdateUIFromData;

        // 시작 시 현재 상태 반영
        UpdateUIFromData();

        gameObject.SetActive(true);
    }

    public override void Enable() => gameObject.SetActive(true);
    public override void Disable() => gameObject.SetActive(false);

    private void UpdateUIFromData()
    {
        var handleSlotData = _inventory.handleSlots;

        for (int i = 0; i < quickSlots.Length && i < handleSlotData.Length; i++)
        {
            quickSlots[i].item = handleSlotData[i].item;
            quickSlots[i].quantity = handleSlotData[i].quantity;
            quickSlots[i].SetSlot();
        }
    }

    public void HighlightSlot(int index)
    {
        if (_currentHighlightedIndex >= 0 && _currentHighlightedIndex < quickSlots.Length)
        {
            quickSlots[_currentHighlightedIndex].SetHighlight(false);
        }

        if (index >= 0 && index < quickSlots.Length)
        {
            quickSlots[index].SetHighlight(true);
            _currentHighlightedIndex = index;
        }
    }
}
