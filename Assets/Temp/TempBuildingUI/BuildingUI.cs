using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : BaseUI
{
    [SerializeField] private BuildingUISlot slotPrefab;
    [SerializeField] private BuildingUIRecipeSlot reicpeSlotPrefab;
    [SerializeField] private RectTransform itemSlotRoot;
    [SerializeField] private RectTransform recipeSlotRoot;
    
    [Space(10f)]
    [SerializeField] private ItemDataTableSO buildingItemDataTable;


    private BuildingUISlot _selectedSlot;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Init();
        Enable();
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
        var slotPool = PoolManager.Instance.CreatePool(slotPrefab);
        var recipePool = PoolManager.Instance.CreatePool(reicpeSlotPrefab);
        
        for (int i = 0; i < buildingItemDataTable.ItemDatas.Length; i++)
        {
            var targetItem = buildingItemDataTable.ItemDatas[i] as BuildingItemDataSO;

            var targetSlot = slotPool.Spawn(itemSlotRoot);
            
            targetSlot.Init(targetItem, () =>
            {
                _selectedSlot = targetSlot;
                
                ShowRecipe(recipePool, targetItem.CraftingRecipe);
            });
        }
    }

    public void ShowRecipe(ObjectPool<BuildingUIRecipeSlot> targetPool, CraftingRecipe recipe)
    {
        targetPool.DespawnAll();
       
        for (int i = 0; i < recipe.neededItem.Count; i++)
        {
            var targetSlot = targetPool.Spawn(recipeSlotRoot);
            
            targetSlot.Init(recipe.neededItem[i].data as BuildingItemDataSO, recipe.neededItem[i].amount);
            
        }
    }
}
