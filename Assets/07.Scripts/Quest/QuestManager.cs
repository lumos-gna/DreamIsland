
using System.Collections.Generic;
using UnityEngine;


public class QuestManager : Singleton<QuestManager>
{
    
    private List<Quest> _acceptedQuestList = new();

    public void AcceptQuest(Quest questData)  //퀘스트 수락, 리스트에 넣음
    {
        questData.Reset();
        _acceptedQuestList.Add(questData);
        Debug.Log($"퀘스트 수락됨: {questData.name}");
    }

    public void QuestPlusCount(string questName)  //퀘스트 진행도 ++
    {
        if (SearchQuest(questName) != null)
        {
            _acceptedQuestList.Find(q => q.name == questName).PlusCount();
        }
        else{Debug.Log("NULL");}
    }

    public string QuestComplete(string questName) //퀘스트 처리 =>리스트에서 삭제
    { 
        Quest questToRemove = _acceptedQuestList.Find(q => q.name == questName);
        if (questToRemove != null)
        {
            string text = questToRemove.clearText;
            _acceptedQuestList.Remove(questToRemove);
            return text;
        }
        return null;
    }

    private Quest SearchQuest(string questName)  //퀘스트를 찾아 복사본 리턴
    {
        if (_acceptedQuestList != null)
        {
            foreach (var quest in _acceptedQuestList)
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
        if (Input.GetKeyDown(KeyCode.G))
        {
            QuestPlusCount("학 랜덤 퀘스트 1");QuestPlusCount("토끼 랜덤 퀘스트1");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            QuestPlusCount("학 랜덤 퀘스트 2");QuestPlusCount("토끼 랜덤 퀘스트2");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            QuestPlusCount("학 랜덤 퀘스트 3");QuestPlusCount("토끼 랜덤 퀘스트3");
        }
    }
}
