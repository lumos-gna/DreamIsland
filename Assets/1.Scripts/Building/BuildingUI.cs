using UnityEngine;

public class BuildingUI : BaseUI
{
    [SerializeField] private ItemData[] buildableItemDatas;

    private CanvasGroup _canvasGroup;

    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Init()
    {
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
