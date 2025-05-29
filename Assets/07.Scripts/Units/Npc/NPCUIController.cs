using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class NPCUIController: MonoBehaviour
{
    public NpcData npcData;
    public Canvas uiCanvas;
    public TextMeshProUGUI dialogueText;
    public Button exitButton;
    public ButtonFactory buttonFactory;
    
    private GameObject[] _buttons;
    private PlayerController playerController;
    private int _selectedDialogue;
    private string _questName;
    private DialogueType type;
    
    private float _clickDelay = 0.2f;
    private float _lastClickTime = 0f;
    
    void Start()
    {
        npcData.AllReset();
        
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerController = player.GetComponentInChildren<PlayerController>();
        }

        exitButton.onClick.AddListener(() => OnOff());
        _buttons = new GameObject[npcData.npcDialog.Length];
    }

    private void PlusButton(int length)   //대화 버튼 추가
    {
        for (int i = 0; i < length; i++)
        {
            int index = i;
            _buttons[index] = buttonFactory.CreateButton(index, npcData.npcDialog[index].buttonName);
            
            //버튼 이벤트 초기화
            //_buttons[index].GetComponent<Button>().onClick.AddListener(() => LoadDialogue(index));
            Button btn = _buttons[index].GetComponent<Button>();
            btn.onClick.RemoveAllListeners();  // ⭐ 중복 호출 방지 핵심
            btn.onClick.AddListener(() => LoadDialogue(index));
        }
    }
    
    
    private void LoadDialogue(int i)  //대화 가져오기
    {
        _selectedDialogue = i;

        //dialogueText.text = npcData.NextText(_selectedDialogue);
        
        dialogueText.text = "";
        string fullText = npcData.NextText(_selectedDialogue);
        dialogueText.DOText(fullText, 1f).SetEase(Ease.Linear);
        
        type = npcData.npcDialog[_selectedDialogue].type;
        exitButton.gameObject.SetActive(false);
        
        if (npcData.npcDialog[_selectedDialogue].GetExitButton())
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

            type = DialogueType.NONE;
        }
        else
        {
            PlusButton(npcData.npcDialog.Length);
        }
    }
    
    
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && uiCanvas.gameObject.activeSelf)
        {

            if (type == DialogueType.NORMAL)
            {
                
                string fullText = npcData.NextText(_selectedDialogue);

                if (fullText != null)
                {
                    dialogueText.text = "";
                    dialogueText.DOText(fullText, 1f).SetEase(Ease.Linear);
                }
                else
                {
                    //퀘스트 수락처리, UI끄기 등
                    exitButton.gameObject.SetActive(true);
                }
            }
            else if (type == DialogueType.QUEST)
            {
                if (!exitButton.gameObject.activeSelf)
                {
                    dialogueText.text = "";
                    string fullText = npcData.NextText(_selectedDialogue);
                    dialogueText.DOText(fullText, 1f).SetEase(Ease.Linear);
                
                    exitButton.gameObject.SetActive(true);
                }
            }
            

            
            // string fullText = npcData.NextText();
            // dialogueText.text = "";
            // dialogueText.DOText(fullText, 1f).SetEase(Ease.Linear);
        }
    }
}

