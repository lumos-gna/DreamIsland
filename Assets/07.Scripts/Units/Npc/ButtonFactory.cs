using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonFactory : MonoBehaviour
{
    public GameObject buttonGroup;
    public GameObject buttonPrefab; 

    public GameObject CreateButton(int i, string buttonName)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonGroup.transform);
        int capturedIndex = i;
        newButton.name = "Button_" + capturedIndex;
            
        //버튼 텍스트 초기화
        TextMeshProUGUI btnText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
        {
            btnText.text = buttonName;
        }
            

        //버튼 색상 초기화
        Image btnImage = newButton.GetComponent<Image>();
        if (btnImage != null)
        {
            btnImage.color = new Color32(0x9f, 0x9f, 0x9f, 255);
        }
            
        
        
        return newButton;
    }
}