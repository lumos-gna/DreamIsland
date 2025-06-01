using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : BaseUI
{
    public override bool IsEnabled => _canvasGroup.blocksRaycasts;

    
    [Space(10f)]
    [SerializeField] private CraftingUISlot slotPrefab;
    [SerializeField] private CraftingUIRecipeSlot reicpeSlotPrefab;
   
    [Space(10f)]
    [SerializeField] private RectTransform itemSlotRoot;
    [SerializeField] private RectTransform recipeSlotRoot;
    
    [Space(10f)]
    [SerializeField] private ItemDataTable craftItemDataTable;

    [SerializeField] private Button craftButton;

    
    private CraftingUISlot _selectedSlot;
    
    private CanvasGroup _canvasGroup;

    private Inventory _inventory;


    private ObjectPool<CraftingUIRecipeSlot> _recipePool;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }


    public override void Enable()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;

        if (_selectedSlot != null)
        {
            _selectedSlot.HighLightImage.enabled = false;
            
            _selectedSlot = null;
            
            _recipePool.DespawnAll();
        
            craftButton.gameObject.SetActive(false);
        }
    }

    public override void Disable()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }


    public override void Init()
    {
        var gameManager = GameManager.Instance;
        var slotPool = PoolManager.Instance.GetPool(slotPrefab);
        
        _recipePool = PoolManager.Instance.GetPool(reicpeSlotPrefab);
        
        _inventory = gameManager.Inventory;
        
        craftButton.onClick.AddListener(() =>
        {
            Craft();
            
            InitRecipeInfo(_selectedSlot.Item.CraftingInfo.recipes);
        });
        
        for (int i = 0; i < craftItemDataTable.ItemDatas.Length; i++)
        {
            var targetItem = craftItemDataTable.ItemDatas[i];

            var targetSlot = slotPool.Spawn(itemSlotRoot);
            
            
            targetSlot.Init(targetItem, () =>
            {
                if (_selectedSlot != null)
                {
                    _selectedSlot.HighLightImage.enabled = false;
                }
                
                _selectedSlot = targetSlot;
                
                _selectedSlot.HighLightImage.enabled = true;
                
                InitRecipeInfo(targetItem.CraftingInfo.recipes);
            });
        }
    }

    public void InitRecipeInfo(ItemCraftingInfo.Recipe[] recipe)
    {
        _recipePool.DespawnAll();

        int fullCount = 0;
       
        for (int i = 0; i < recipe.Length; i++)
        {
            var recipeSlot = _recipePool.Spawn(recipeSlotRoot);

            var needItem = recipe[i].data;
            
            int maxAmount = recipe[i].amount;

            int curAmount = 0;
            
            
            var inventorySlot = _inventory.FindSlot((slot) => slot.item == needItem);
            
            if (inventorySlot != null)
            {
                curAmount = inventorySlot.quantity;
            }
            
            recipeSlot.Init(needItem, curAmount, maxAmount, out bool isFull);

            if (isFull) fullCount++;
        }

        bool isCraftable = fullCount == recipe.Length && !_inventory.IsFull;
        
        craftButton.gameObject.SetActive(isCraftable);

        if (isCraftable)
        {
            craftButton.transform.SetAsLastSibling();
        }
    }

    void Craft()
    {
        var recipes = _selectedSlot.Item.CraftingInfo.recipes;
        
        for (int i = 0; i < recipes.Length; i++)
        {
            for (int j = 0; j < recipes[i].amount; j++)
            {
                _inventory.DecreaseItem(recipes[i].data);
            }
        }
        
        _inventory.AddItem(_selectedSlot.Item);
    }
}
