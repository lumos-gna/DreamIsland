using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class NPCUIController : MonoBehaviour
{
    public NpcData npcData;
    public Canvas uiCanvas;
    public TextMeshProUGUI dialogueText;
    public Button exitButton;
    public ButtonFactory buttonFactory;

    private GameObject[] _buttons;
    private PlayerController _playerController;
    private int _selectedDialogue;
    private string _questName;
    private DialogueType _type;
    private bool _talking;

    void Start()
    {
        npcData.AllReset();

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            _playerController = player.GetComponentInChildren<PlayerController>();
        }

        exitButton.onClick.AddListener(() => OnOff());  //종료 메서드 추가
    }

    private void PlusButton(int length)   //대화 버튼 추가
    {
        _buttons = new GameObject[length];
        for (int i = 0; i < length; i++)
        {
            int index = i;
            _buttons[index] = buttonFactory.CreateButton(index, npcData.npcDialog[index].buttonName);

            Button btn = _buttons[index].GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => LoadDialogue(index));
        }
    }

    private void LoadDialogue(int i)  //대화 가져오기
    {
        _selectedDialogue = i;
        string fullText;

        if (QuestManager.Instance.CheckMainQuest() && npcData.Type(_selectedDialogue) == DialogueType.Quest)
        {
            dialogueText.text = "";
            fullText = "나는 말리지 않을게. 현실은 차갑고 아플 거야. 하지만 선택은… 네 몫이야, 아린.";
            dialogueText.DOText(fullText, 1f).SetUpdate(true).SetEase(Ease.Linear);
            exitButton.gameObject.SetActive(true);
            return;
        }

        dialogueText.text = "";
        fullText = npcData.NextText(_selectedDialogue);
        dialogueText.DOText(fullText, 1f).SetUpdate(true).SetEase(Ease.Linear);

        _type = npcData.npcDialog[_selectedDialogue].type;
        exitButton.gameObject.SetActive(false);

        if (npcData.npcDialog[_selectedDialogue].GetExitButton())
        {
            exitButton.gameObject.SetActive(true);
        }

        for (int j = 0; j < _buttons.Length; j++)
        {
            if (_buttons[j] != null)
                _buttons[j].SetActive(false);
        }
    }

    public void OnOff()  //npc와의 대화 on/off
    {
        uiCanvas.gameObject.SetActive(!uiCanvas.gameObject.activeSelf);

        GameManager.Instance.ToggleCursor(!uiCanvas.gameObject.activeSelf);

        if (!uiCanvas.gameObject.activeSelf)
        {
            GameManager.Instance.OnOffEquipCamera(true);
            //Time.timeScale = 1f;

            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                PlayerController playerScript = playerObj.GetComponent<PlayerController>();
                if (playerScript != null)
                {
                    playerScript.Talking(false);   //플레이어가 자동으로 NPC를 바라보기 해제
                }
            }

            for (int i = 0; i < _buttons.Length; i++)   //버튼 삭제
            {
                if (_buttons[i] != null)
                {
                    Destroy(_buttons[i]);
                    _buttons[i] = null;
                }
            }
            _buttons = null;

            _type = DialogueType.None;
        }
        else
        {
            //Time.timeScale = 0f;
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                PlayerController playerScript = playerObj.GetComponent<PlayerController>();
                if (playerScript != null)
                {
                    playerScript.Talking(true);  //플레이어가 자동으로 NPC를 바라보기
                }
            }
            PlusButton(npcData.npcDialog.Length);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && uiCanvas.gameObject.activeSelf)
        {

            if (_type == DialogueType.Normal || _type == DialogueType.Quest)
            {

                string fullText;

                if (QuestManager.Instance.CheckMainQuest() && _type == DialogueType.Quest)
                {
                    dialogueText.text = "";
                    fullText = "나는 말리지 않을게. 현실은 차갑고 아플 거야. 하지만 선택은… 네 몫이야, 아린.";
                    dialogueText.DOText(fullText, 1f).SetUpdate(true).SetEase(Ease.Linear);
                    exitButton.gameObject.SetActive(true);

                    return;
                }

                fullText = npcData.NextText(_selectedDialogue);
                if (fullText != null)
                {
                    dialogueText.text = "";
                    dialogueText.DOText(fullText, 1f).SetUpdate(true).SetEase(Ease.Linear);
                }
                else
                {
                    exitButton.gameObject.SetActive(true);
                }


            }
        }
    }
}

