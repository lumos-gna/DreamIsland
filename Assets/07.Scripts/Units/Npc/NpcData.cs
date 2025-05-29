
using UnityEngine;


[CreateAssetMenu(fileName = "NPC", menuName = "NPC/NPCData")]
public class NpcData : ScriptableObject
{
    public Sprite Sprite;   //NPC 사진
    public string text;     //대화 시작시 텍스트
    public NPCDialog[] npcDialog;
    public Quest[] quests;
    private int _currentQuestIndex;

    public string NextText(int selectedDialogue)   //대화 텍스트 전달
    {

        switch (npcDialog[selectedDialogue].type)
        {
            case dialogueType.NORMAL:
                
                return npcDialog[selectedDialogue].GetText();

            case dialogueType.QUEST:
                
                if (QuestManager.Instance.CheckClearQuest(quests[_currentQuestIndex].name))             //받았던 퀘스트 클리어시
                {
                    return QuestManager.Instance.QuestComplete(quests[_currentQuestIndex].name);
                }
                else if (QuestManager.Instance.CheckOnOffQuest(quests[_currentQuestIndex].name))           //수락한 퀘스트가 있으면
                {
                    return quests[_currentQuestIndex].text;
                }
                else      //수락한 퀘스트가 없으면
                {
                    _currentQuestIndex = Random.Range(0, quests.Length);
                    QuestManager.Instance.AcceptQuest(quests[_currentQuestIndex]);                     //퀘스트 수락

            
                    return quests[_currentQuestIndex].text; 
                }
            
            case dialogueType.RANDOM:
                int random = Random.Range(0, npcDialog[selectedDialogue].npcDialogTexts.Length);
                return npcDialog[selectedDialogue].npcDialogTexts[random].text;
        }
        return null;
    }
}




public enum dialogueType
{
    RANDOM,
    NORMAL,
    QUEST,
    NONE,
}


[System.Serializable]
public class NPCDialog
{
    public dialogueType type;
    public string buttonName;
    //public QuestData questData;
    public NPCDialogText[] npcDialogTexts;
    private int quest;
    
    private int count = -1;  //대화 순서

    public void Reset()  //대화 순서 리셋
    {
        count = -1;
    }

    public string GetText()  //대화 텍스트 전달
    {
        count++;
        if (count >= npcDialogTexts.Length)
        {
            return null;
        }
        
        return npcDialogTexts[count].text; 
    }
}




[System.Serializable]
public class NPCDialogText
{
    public string text;
}