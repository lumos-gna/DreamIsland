
using UnityEngine;

interface IObserver
{
    void Update(string message);
}

public class QuestManager : SingleTon<QuestManager>
{
    [SerializeField]
    private QuestData quests;

    protected override void Awake()
    {
        base.Awake();
        quests.Reset();
    }

    public void AcceptQuest(int questID)
    {
        quests.AcceptQuest(questID);
    }

    public void QuestCheck(int questID)
    {
        quests.CountQuest(questID);
    }

    public void QuestClear(int questID)
    { 
        
    }
    
    public int CheckOnOffQuest(){return quests.CheckOnOffQuest();}

    public int CheckClearQuest(){return quests.CheckClearQuest();}
    
    public string GetQuestText(int i){return quests.GetQuestText(i);}



    //테스트용
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     AcceptQuest(0);
        // }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            QuestCheck(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            QuestCheck(1);
        }
    }
}
