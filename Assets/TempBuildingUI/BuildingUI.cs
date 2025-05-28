using UnityEngine;
using UnityEngine.Events;

public class BuildingUI : BaseUI
{
    [SerializeField] private BuildingUISlot slotPrefab;
    [SerializeField] private RectTransform itemSlotRoot;
    [SerializeField] private RectTransform recipeSlotRoot;
    
    [Space(10f)]
    [SerializeField] private ItemData[] craftingItemDatas;

    
    private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Init()
    {
        for (int i = 0; i < craftingItemDatas.Length; i++)
        {
            //Instantiate(slotPrefab, itemSlotRoot).InitToItemSlot(craftingItemDatas[i]);
        }
    }
    

    public override void Enable()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public override void Disable()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }
}
