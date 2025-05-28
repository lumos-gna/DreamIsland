using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
using TMPro;

public class NPCController: MonoBehaviour
{
    public PlayerController playerController;
    public NpcData npcData;
    public Canvas uiCanvas;
    public Image image;
    public TextMeshProUGUI dialogueText;
    public Button exitButton;
    
    public GameObject buttonGroup;
    public GameObject buttonPrefab;  
    
    private GameObject[] _buttons;
    private int _selectedDialogue;
    private int _questNumber;
    private dialogueType type;
    
    void Start()
    {
        exitButton.onClick.AddListener(() => OnOff());
        _buttons = new GameObject[npcData.npcDatas.Length];
    }

    private void PlusButton()   //대화 버튼 추가
    {
        for (int i = 0; i < npcData.npcDatas.Length; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonGroup.transform);
            int capturedIndex = i;
            _buttons[capturedIndex] = newButton;
            newButton.name = "Button_" + capturedIndex;
            
            //버튼 텍스트 초기화
            TextMeshProUGUI btnText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = npcData.npcDatas[capturedIndex].buttonName;
            }
            

            //버튼 색상 초기화
            Image btnImage = newButton.GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.color = new Color32(0x9f, 0x9f, 0x9f, 255);
            }
            
            //버튼 이벤트 초기화
            newButton.GetComponent<Button>().onClick.AddListener(() => LoadDialogue(capturedIndex));
        }
    }
    
    private void LoadDialogue(int i)
    {
        _selectedDialogue = i;
        npcData.npcDatas[_selectedDialogue].Reset();
        dialogueText.text = npcData.NextText(_selectedDialogue);
        type = npcData.npcDatas[_selectedDialogue].type;
        exitButton.gameObject.SetActive(false);
        
        if (type == dialogueType.RANDOM || type == dialogueType.QUEST)
        {
            exitButton.gameObject.SetActive(true);
        }
        for (int j = 0; j < _buttons.Length; j++)
        {
            _buttons[j].SetActive(false);
        }
    }
    
    
    

    public void OnOff()  //npc와의 대화 on/off
    {
        uiCanvas.gameObject.SetActive(!uiCanvas.gameObject.activeSelf);
        playerController.ChangeCursorState(uiCanvas.gameObject.activeSelf);
        
        if (!uiCanvas.gameObject.activeSelf)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i] != null)
                {
                    Destroy(_buttons[i]);
                    _buttons[i] = null; 
                }
            }
        }
        else
        {
            PlusButton();
        }
    }
    
    
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && uiCanvas.gameObject.activeSelf)
        {
            if (type == dialogueType.Normal)
            {
                string temp = npcData.NextText(_selectedDialogue);

                if (temp != null)
                {
                    dialogueText.text = temp;
                }
                else
                {
                    //퀘스트 수락처리, UI끄기 등
                    exitButton.gameObject.SetActive(true);
                }
            }
            else if (type == dialogueType.QUEST)
            {
                if (!exitButton.gameObject.activeSelf)
                {
                    string temp = npcData.NextText(_selectedDialogue);
                
                    exitButton.gameObject.SetActive(true);
                }
            }
            

            
            // string fullText = npcData.NextText();
            // dialogueText.text = "";
            // dialogueText.DOText(fullText, 1f).SetEase(Ease.Linear);
        }
    }
}