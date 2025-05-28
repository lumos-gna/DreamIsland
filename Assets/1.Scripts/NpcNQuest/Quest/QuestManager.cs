
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

    public int CheckOnOffQuest(){return quests.CheckOnOffQuest();}





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
