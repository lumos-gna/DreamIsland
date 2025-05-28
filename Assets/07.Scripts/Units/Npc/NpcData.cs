
using UnityEngine;


[CreateAssetMenu(fileName = "NPC", menuName = "NPC/NPCData")]
public class NpcData : ScriptableObject
{
    public Sprite Sprite;   //NPC 사진
    public string text;     //대화 시작시 텍스트
    public NPCDialog[] npcDialog;
    public Quest[] quests;

    public string NextText(int selectedDialogue)   //대화 텍스트 전달
    {
        return npcDialog[selectedDialogue].GetText();
    }

    public dialogueType getDialogueType(int selectedDialogue)
    {
        return npcDialog[selectedDialogue].type;
    }
}




public enum dialogueType
{
    RANDOM,
    Normal,
    QUEST,
}


[System.Serializable]
public class NPCDialog
{
    public dialogueType type;
    public string buttonName;
    public QuestData quests;
    public NPCDialogText[] npcDialogTexts;
    
    private int count = -1;  //대화 순서

    public void Reset()  //대화 순서 리셋
    {
        count = -1;
    }

    public string GetText()  //대화 텍스트 전달
    {
        if (type == dialogueType.Normal)
        {
            count++;
            if (count >= npcDialogTexts.Length)
            {
                return null;
            }
        
            return npcDialogTexts[count].text; 
        }
        else if (type == dialogueType.QUEST)
        {
            if (QuestManager.Instance.CheckClearQuest() != -1)             //받았던 퀘스트 클리어시
            {
                return QuestManager.Instance.GetQuestText(QuestManager.Instance.CheckClearQuest());
            }
            else if (QuestManager.Instance.CheckOnOffQuest() != -1)           //수락한 퀘스트가 있으면
            {
                return npcDialogTexts[QuestManager.Instance.CheckOnOffQuest()].text;
            }
            else      //수락한 퀘스트가 없으면
            {
                int quest = Random.Range(0, npcDialogTexts.Length);
                QuestManager.Instance.AcceptQuest(quest);                     //퀘스트 수락

            
                return npcDialogTexts[quest].text; 
            }
        }
        else
        {
            int random = Random.Range(0, npcDialogTexts.Length);
            return npcDialogTexts[random].text;
        }
    }
}




[System.Serializable]
public class NPCDialogText
{
    public string text;
}