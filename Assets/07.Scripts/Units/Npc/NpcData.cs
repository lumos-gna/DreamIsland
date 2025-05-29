
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

    public string NextText(int selectedDialogue)   //대화 텍스트 전달
    {

        switch (npcDialog[selectedDialogue].type)
        {
            case DialogueType.NORMAL:
                return npcDialog[selectedDialogue].GetText();

            case DialogueType.RANDOMQUEST:
                
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
            
            
            case DialogueType.QUEST:
                if (quests.Length <= _currentQuestIndex)
                {
                    Debug.Log(quests.Length +" / "+ _currentQuestIndex);
                    return "남은 할 일이 없어...";}
                if (QuestManager.Instance.CheckClearQuest(quests[_currentQuestIndex].name))             //받았던 퀘스트 클리어시
                {
                    _currentQuestIndex++; 
                    npcDialog[selectedDialogue].SetCount(0);
                    
                    return QuestManager.Instance.QuestComplete(quests[_currentQuestIndex-1].name);
                }
                else if (QuestManager.Instance.CheckOnOffQuest(quests[_currentQuestIndex].name))           //수락한 퀘스트가 있으면
                {
                    npcDialog[selectedDialogue].SetCount(0);
                    return quests[_currentRandomQuestIndex].text;
                }
                else      //수락한 퀘스트가 없으면
                {
                    QuestManager.Instance.AcceptQuest(quests[_currentQuestIndex]);                     //퀘스트 수락
                    npcDialog[selectedDialogue].SetCount(0);
                    
                    return quests[_currentQuestIndex].text; 
                }
            
            case DialogueType.RANDOM:
                
                int random = Random.Range(0, npcDialog[selectedDialogue].npcDialogTexts.Length);
                npcDialog[selectedDialogue].SetCount(random);
                return npcDialog[selectedDialogue].npcDialogTexts[random].text;
        }
        return null;
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

    public bool GetExitButton()
    {
        if (_count >= npcDialogTexts.Length || _count < 0)
        {
            Debug.Log(_count);
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