
using System.Collections.Generic;
using UnityEngine;


public class QuestManager : Singleton<QuestManager>
{
    
    private List<Quest> _AcceptedQuestList;

    public void AcceptQuest(Quest questData)  //퀘스트 수락, 리스트에 넣음
    {
        _AcceptedQuestList.Add(questData);
    }

    public void QuestPlusCount(string questName)  //퀘스트 진행도 ++
    {
        if (SearchQuest(questName) != null)
        {
            SearchQuest(questName).PlusCount();
        }
    }

    public string QuestClear(string questName) //퀘스트 처리 =>리스트에서 삭제
    { 
        Quest questToRemove = _AcceptedQuestList.Find(q => q.name == questName);
        if (questToRemove != null)
        {
            string text = questToRemove.clearText;
            _AcceptedQuestList.Remove(questToRemove);
            return text;
        }
        return null;
    }

    private Quest SearchQuest(string questName)  //퀘스트를 찾아 복사본 리턴
    {
        if (_AcceptedQuestList != null)
        {
            foreach (var quest in _AcceptedQuestList)
            {
                if (quest.name == questName)
                {
                    return quest;
                }
            }
        }
        return null;
    }
    
    public bool CheckOnOffQuest(string questName)  // 받은 퀘스트에 그 퀘스트가 있으면 true(이름으로 비교)
    {
        if (SearchQuest(questName) != null)
        {
           return true;
        }
        return false;
    }

    public bool CheckClearQuest(string questName)  //클리어한 퀘스트인지 확인
    {
        if (SearchQuest(questName) != null && SearchQuest(questName).Clear())
        {
            return true;
        }
        return false;
    }



    //테스트용
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     AcceptQuest(0);
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha9))
        // {
        //     QuestCheck();
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha8))
        // {
        //     QuestCheck(1;
        // }
    }
}
