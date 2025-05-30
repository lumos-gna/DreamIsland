
using UnityEngine;


[CreateAssetMenu(fileName = "NPC", menuName = "NPC/NPCData")]
public class NpcData : ScriptableObject
{
    public string text;     //대화 시작시 텍스트
    public NPCDialog[] npcDialog;
    public Quest[] randomQuests;
    public Quest[] quests;
    
    private int _currentQuestIndex = 0;
    private int _currentRandomQuestIndex = 0;

    void OnEnable()
    {
        _currentQuestIndex = 0;}

    public DialogueType Type(int selectedDialogue)
    {
        return npcDialog[selectedDialogue].type;
    }
    
    public string NextText(int selectedDialogue)   //대화 텍스트 전달
    {

        switch (npcDialog[selectedDialogue].type)
        {
            case DialogueType.NORMAL:  //평범한 이어지는 대화
                return npcDialog[selectedDialogue].GetText();

            case DialogueType.RANDOMQUEST:   //랜덤 퀘스트
                
                if (QuestManager.Instance.CheckClearQuest(randomQuests[_currentRandomQuestIndex].name))             //받았던 퀘스트 클리어시
                {
                    npcDialog[selectedDialogue].SetCount(0);
                    return QuestManager.Instance.QuestComplete(randomQuests[_currentRandomQuestIndex].name);
                }
                else if (QuestManager.Instance.CheckOnOffQuest(randomQuests[_currentRandomQuestIndex].name))           //수락한 퀘스트가 있으면
                {
                    npcDialog[selectedDialogue].SetCount(0);
                    return randomQuests[_currentRandomQuestIndex].text;
                }
                else      //수락한 퀘스트가 없으면
                {
                    _currentRandomQuestIndex = Random.Range(0, randomQuests.Length);
                    QuestManager.Instance.AcceptQuest(randomQuests[_currentRandomQuestIndex]);    //퀘스트 수락
                    npcDialog[selectedDialogue].SetCount(0);

            
                    return randomQuests[_currentRandomQuestIndex].text; 
                }
            
            
            case DialogueType.QUEST:     //  메인 퀘스트
                
                
                if (quests.Length <= _currentQuestIndex)  //남은 퀘스트가 없으면
                {
                    return "나는 말리지 않을게. 현실은 차갑고 아플 거야. 하지만 선택은… 네 몫이야, 아린.";
                }
                if (QuestManager.Instance.CheckClearQuest(quests[_currentQuestIndex].name))             //받았던 퀘스트 클리어시
                {
                    _currentQuestIndex++; 
                    npcDialog[selectedDialogue].SetCount(0);
                    
                    return QuestManager.Instance.QuestComplete(quests[_currentQuestIndex-1].name);
                }
                else if (QuestManager.Instance.CheckOnOffQuest(quests[_currentQuestIndex].name))           //수락한 퀘스트가 있으면
                {
                    //npcDialog[selectedDialogue].SetCount(0);
                    //return quests[_currentRandomQuestIndex].text;
                    return npcDialog[selectedDialogue].GetText();
                }
                else      //수락한 퀘스트가 없으면
                {
                    QuestManager.Instance.AcceptQuest(quests[_currentQuestIndex]);                     //퀘스트 수락
                    //npcDialog[selectedDialogue].SetCount(0);
                    
                    //return quests[_currentQuestIndex].text; 
                    return npcDialog[selectedDialogue].GetText();
                }
            
            case DialogueType.RANDOM:  // 랜덤한 대화 출력
                
                int random = Random.Range(0, npcDialog[selectedDialogue].npcDialogTexts.Length);
                npcDialog[selectedDialogue].SetCount(random);
                return npcDialog[selectedDialogue].npcDialogTexts[random].text;
        }
        return null;
    }

    public void AllReset()
    {
        foreach (Quest quest in quests)
        {
            quest.Reset();
        }

        foreach (Quest quest in randomQuests)
        {
            quest.Reset();
        }
    }
}




public enum DialogueType
{
    RANDOM,
    NORMAL,
    QUEST,
    RANDOMQUEST,
    NONE,
}


[System.Serializable]
public class NPCDialog
{
    public DialogueType type;
    public string buttonName;
    public NPCDialogText[] npcDialogTexts;
    
    private int _count = -1;  //대화 순서

    public void Reset()  //대화 순서 리셋
    {
        _count = -1;
    }

    public void SetCount(int i){_count = i;}
    
    public string GetText()  //대화 텍스트 전달
    {
        _count++;
        if (_count >= npcDialogTexts.Length)
        {
            return null;
        }
        
        return npcDialogTexts[_count].text; 
    }

    public bool GetExitButton()  //나가기 버튼 온오프 여부 리턴
    {
        if (_count >= npcDialogTexts.Length || _count < 0)
        {
            return true;
        }
        return npcDialogTexts[_count].exitButton;
    }
}




[System.Serializable]
public class NPCDialogText
{
    public string text;
    public bool exitButton;
}