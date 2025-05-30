using UnityEngine;

public class CraftingUI : BaseUI
{
    [SerializeField] private CraftingUISlot slotPrefab;
    [SerializeField] private CraftingUIRecipeSlot reicpeSlotPrefab;
    [SerializeField] private RectTransform itemSlotRoot;
    [SerializeField] private RectTransform recipeSlotRoot;
    
    [Space(10f)]
    [SerializeField] private ItemDataTable craftItemDataTable;


    private CraftingUISlot _selectedSlot;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Init();
        Enable();
        
        GameManager.Instance.ToggleCursor(false);
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

    public override void Init()
    {
        var slotPool = PoolManager.Instance.GetPool(slotPrefab);
        var recipePool = PoolManager.Instance.GetPool(reicpeSlotPrefab);
        
        for (int i = 0; i < craftItemDataTable.ItemDatas.Length; i++)
        {
            var targetItem = craftItemDataTable.ItemDatas[i];

            var targetSlot = slotPool.Spawn(itemSlotRoot);
            
            targetSlot.Init(targetItem, () =>
            {
                _selectedSlot = targetSlot;
                
                ShowRecipe(recipePool, targetItem.CraftingInfo.recipes);
            });
        }
    }

    public void ShowRecipe(ObjectPool<CraftingUIRecipeSlot> targetPool, ItemCraftingInfo.Recipe[] recipe)
    {
        targetPool.DespawnAll();
       
        for (int i = 0; i < recipe.Length; i++)
        {
            var targetSlot = targetPool.Spawn(recipeSlotRoot);
            
            targetSlot.Init(recipe[i].data , recipe[i].amount);
            
        }
    }
}
