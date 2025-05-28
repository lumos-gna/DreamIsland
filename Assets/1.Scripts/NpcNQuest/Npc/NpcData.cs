
using UnityEngine;


[CreateAssetMenu(fileName = "NPC", menuName = "NPC/NPCData")]
public class NpcData : ScriptableObject
{
    public Sprite Sprite;   //NPC 사진
    public string text;     //대화 시작시 텍스트
    public NPCData[] npcDatas;

    public string NextText(int selectedDialogue)   //대화 텍스트 전달
    {
        return npcDatas[selectedDialogue].GetText();
    }

    public dialogueType getDialogueType(int selectedDialogue)
    {
        return npcDatas[selectedDialogue].type;
    }
}




public enum dialogueType
{
    RANDOM,
    Normal,
    QUEST,
}


[System.Serializable]
public class NPCData
{
    public dialogueType type;
    public string buttonName;
    public NPCDialog[] npcDialog;
    
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
            if (count >= npcDialog.Length)
            {
                return null;
            }
        
            return npcDialog[count].text; 
        }
        else if (type == dialogueType.QUEST)
        {
            if (QuestManager.Instance.CheckClearQuest() != -1)             //받았던 퀘스트 클리어시
            {
                return QuestManager.Instance.GetQuestText(QuestManager.Instance.CheckClearQuest());
            }
            else if (QuestManager.Instance.CheckOnOffQuest() != -1)           //수락한 퀘스트가 있으면
            {
                return npcDialog[QuestManager.Instance.CheckOnOffQuest()].text;
            }
            else      //수락한 퀘스트가 없으면
            {
                int quest = Random.Range(0, npcDialog.Length);
                QuestManager.Instance.AcceptQuest(quest);                     //퀘스트 수락

            
                return npcDialog[quest].text; 
            }
        }
        else
        {
            int random = Random.Range(0, npcDialog.Length);
            return npcDialog[random].text;
        }
    }
}




[System.Serializable]
public class NPCDialog
{
    public string text;
}