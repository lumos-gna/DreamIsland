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

    public BaseUI Create<T>() where T : BaseUI
    {
        string targetName = typeof(T).Name;

        if (_createdUIDict.ContainsKey(targetName))
        {
            Debug.LogError($"이미 생성된 UI : {targetName}");
            return null;
        }

        BaseUI targetUIPrefab = Resources.Load<BaseUI>($"{PrefabPath}{targetName}");

        if (targetUIPrefab == null)
        {
            Debug.LogError($"잘못된 프리팹 : {targetName}");
            return null;
        }

        BaseUI targetUI = Instantiate(targetUIPrefab);

        _createdUIDict.Add(targetName, targetUI);

        targetUI.Init();

        UIType uiType = targetUI.UIType;

        Canvas parentCanvas = _canvasDict.ContainsKey(uiType)
            ? _canvasDict[uiType]
            : CreateCanvas(uiType);


        if (parentCanvas == null)
        {
            Debug.LogError("캔버스를 찾지 못함");
            return null;
        }

        if (uiType == UIType.Popup)
            targetUI.Disable();

        targetUI.transform.SetParent(parentCanvas.transform, false);

        return targetUI;
    }

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
            targetUI = Create<T>();

            if (targetUI == null)
            {
                Debug.LogError($"{targetName} UI를 찾을 수 없음.");

                return;
            }
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

    public T Get<T>() where T : BaseUI
    {
        string targetName = typeof(T).Name;

        if (_createdUIDict.TryGetValue(targetName, out BaseUI ui))
        {
            return ui as T;
        }

        Debug.LogWarning($"{targetName} UI가 아직 생성되지 않았습니다.");
        return null;
    }
}
