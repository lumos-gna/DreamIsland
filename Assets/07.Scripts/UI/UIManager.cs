using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class UIManager : Singleton<UIManager>
{
    private const string PrefabPath = "UI/";
    private const string CanvasPrefabPath = "UI/Canvas";

    private Dictionary<string, BaseUI> _createdUIDict = new();

    private Dictionary<UIType, Canvas> _canvasDict = new();

    private BaseUI _curPopupUI;
   

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

        if (_curPopupUI != null)
            return;
       

        targetUI.Enable();
        targetUI.transform.SetAsLastSibling();

        if (targetUI.UIType == UIType.Popup)
        {
            _curPopupUI = targetUI;
        }
      
    }

    public void Disable<T>() where T : BaseUI
    {
        string uiName = typeof(T).Name;

        BaseUI targetUI = null;

        if (_createdUIDict.TryGetValue(uiName, out BaseUI ui))
        {
            targetUI = ui;
        }
        else
        {
            targetUI = Create<T>();
        }
        
        targetUI.Disable();

        if (_curPopupUI == targetUI)
        {
            if (_curPopupUI.UIType == UIType.Popup)
            {
                _curPopupUI = null;
            }
        }
    }

    public void DisablePopup()
    {
        if (_curPopupUI != null)
        {
            _curPopupUI.Disable();

            _curPopupUI = null;
        }
    }
    
    
    
    
    private BaseUI Create<T>() where T : BaseUI
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

        Canvas canvas = _canvasDict.ContainsKey(targetUI.UIType) ?
                _canvasDict[targetUI.UIType] :
                CreateCanvas(targetUI.UIType);
        
        targetUI.transform.SetParent(canvas.transform, false);
        
        return targetUI;
    }

    private Canvas CreateCanvas(UIType uiType)
    {
        Canvas canvasPrefab = Resources.Load<Canvas>(CanvasPrefabPath);
        
        Canvas canvas =  Instantiate(canvasPrefab);

        canvas.sortingOrder = (int)uiType;

        canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width , Screen.height);

        _canvasDict[uiType] = canvas;

        return canvas;
    }

    public bool IsUIEnabled<T>() where T : BaseUI
    {
        string targetName = typeof(T).Name;

        if (_createdUIDict.ContainsKey(targetName))
        {
            return _createdUIDict[targetName].IsEnabled;
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