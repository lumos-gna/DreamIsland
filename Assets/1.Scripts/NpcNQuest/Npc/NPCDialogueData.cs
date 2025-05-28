using UnityEngine;





public class NPCDialogueData_RANDOM:NPCData
{
    public override string GetText()
    {
        int random = Random.Range(0, npcDialog.Length);
        return npcDialog[random].text;
    }
    
    public override bool TryGetNextDialog(out NPCDialog dialog)
    {
        int random = Random.Range(0, npcDialog.Length);
        dialog = npcDialog[random];
        return true;
    }
}

public class NPCDialogueData_Normal:NPCData
{
    private int count = -1;  //대화 순서

    public void Reset()      //대화 순서 리셋
    {
        count = -1;
    }
    
    
    
    public override string GetText()
    {
        count++;
        if (count >= npcDialog.Length)
        {
            return null;
        }
        
        return npcDialog[count].text; 
    }
    
    public override bool TryGetNextDialog(out NPCDialog dialog)
    {
        count++;
        if (count >= npcDialog.Length)
        {
            dialog = null;
            return false;
        }

        dialog = npcDialog[count];
        return true;
    }
}

public class NPCDialogueData_QUEST:NPCData
{
    public override string GetText()
    {
        if (QuestManager.Instance.CheckOnOffQuest() != -1)           //수락한 퀘스트가 있으면
        {
            return npcDialog[QuestManager.Instance.CheckOnOffQuest()].text;
        }
        else                                                          //수락한 퀘스트가 없으면
        {
            int quest = Random.Range(0, npcDialog.Length);
            QuestManager.Instance.AcceptQuest(quest);     //퀘스트 수락
            
            return npcDialog[quest].text; 
        }
    }
    
    public override bool TryGetNextDialog(out NPCDialog dialog)
    {
        if (QuestManager.Instance.CheckOnOffQuest() != -1)
        {
            dialog = npcDialog[QuestManager.Instance.CheckOnOffQuest()];
        }
        else
        {
            int quest = Random.Range(0, npcDialog.Length);
            QuestManager.Instance.AcceptQuest(quest);
            dialog = npcDialog[quest];
        }

        return true;
    }
}












[System.Serializable]
public abstract class NPCData
{
    public string buttonName;
    public NPCDialog[] npcDialog;

    public abstract string GetText(); //대화 텍스트 전달

    public int GetNum(int i)
    {
        return npcDialog[i].num;
    }
    
    public abstract bool TryGetNextDialog(out NPCDialog dialog);
}

[System.Serializable]
public class NPCDialog
{
    public int num;
    public string text;
    public bool exitButton;
}