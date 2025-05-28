using System.Collections.Generic;
using UnityEngine;


public enum UIType { HUD, Popup }

public class UIManager : Singleton<UIManager>
{
    private const string PrefabPath = "UI/";
    private const string CanvasPrefabPath = "UI/Canvas";
    
    private Dictionary<string, BaseUI> _createdUIDict = new();
    
    private Dictionary<UIType, Canvas> _canvasDict = new();

    private List<BaseUI> _enabledPopupList = new();
    
    
    public void Enable<T>() where T : BaseUI
    {
        string targetName = typeof(T).Name;

        BaseUI targetUI = null;
        
        if (_createdUIDict.ContainsKey(targetName))
        {
            targetUI = _createdUIDict[targetName];
        }
        else
        {
            targetUI = CreateUI(targetName);

            if (targetUI == null)
            {
                Debug.LogError($"UIManager: {targetName} UI를 찾을 수 없습니다.");
                
                return;
            }
            
            UIType uiType = targetUI.UIType;
        
            Canvas parentCanvas = _canvasDict.ContainsKey(uiType)
                ? _canvasDict[uiType]
                : CreateCanvas(uiType);

            if (parentCanvas == null)
            {
                Debug.LogError($"UIManager: Canvas를 찾을 수 없습니다.");
                return;
            }
        
            targetUI.transform.SetParent(parentCanvas.transform, false);
        }
        
        
        targetUI.Enable();

        if (targetUI.UIType == UIType.Popup)
        {
            if (!_enabledPopupList.Contains(targetUI))
            {
                _enabledPopupList.Add(targetUI);
            }
        }
    }

    public void Disable<T>() where T : BaseUI
    {
        string uiName = typeof(T).Name;

        if (_createdUIDict.TryGetValue(uiName, out BaseUI ui))
        {
            ui.Disable();
        }
    }
    
    public void DisablePopup()
    {
        if (_enabledPopupList.Count > 0)
        {
            BaseUI target = _enabledPopupList[^1];
            
            target.Disable();

            _enabledPopupList.Remove(target);
        }
    }
    
    private Canvas CreateCanvas(UIType uiType)
    {
        Canvas canvasPrefab = Resources.Load<Canvas>(CanvasPrefabPath);
        Canvas targetCanvas = Instantiate(canvasPrefab);

        targetCanvas.gameObject.name = canvasPrefab.gameObject.name + uiType;
        targetCanvas.sortingOrder = (int)uiType;

        _canvasDict[uiType] = targetCanvas;

        return targetCanvas;
    }

    private BaseUI CreateUI(string targetName)
    {
        BaseUI targetUIPrefab = Resources.Load<BaseUI>($"{PrefabPath}{targetName}");
        
        BaseUI targetUI = Instantiate(targetUIPrefab);
        
        targetUI.Init();

        
        _createdUIDict.Add(targetName, targetUI);

        return targetUI;
    }

    public bool IsUIEnabled<T>() where T : BaseUI
    {
        string targetName = typeof(T).Name;

        if (_createdUIDict.ContainsKey(targetName))
        {
            return _createdUIDict[targetName].gameObject.activeInHierarchy;
        }
        
        return false;
    }

}
