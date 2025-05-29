
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class QuestManager : Singleton<QuestManager>
{
    public GameObject questList;
    public GameObject questCell;
    
    public QuestCellFactory questCellFactory;
    
    private List<Quest> _acceptedQuestList = new();
    private List<GameObject> _questUIList = new();

    private void Start()
    {
        _acceptedQuestList.Clear();
    }

    public void AcceptQuest(Quest questData)  //퀘스트 수락, 리스트에 넣음
    {
        questData.Reset();
        _acceptedQuestList.Add(questData);
        
        GameObject newQuestCell = questCellFactory.CreateQuestCell(questData);
        _questUIList.Add(newQuestCell);
    }

    public void QuestPlusCount(string questName)  //퀘스트 진행도 ++
    {
        if (SearchQuest(questName) != null)
        {
            _acceptedQuestList.Find(q => q.name == questName).PlusCount();
            UpdateQuestUI();
        }
    }

    public string QuestComplete(string questName) //퀘스트 처리 =>리스트에서 삭제
    {
        for (int i = 0; i < _acceptedQuestList.Count; i++)
        {
            if (_acceptedQuestList[i].name == questName)
            {
                Quest questToRemove = _acceptedQuestList.Find(q => q.name == questName);
                string text = questToRemove.clearText;
                
                _acceptedQuestList.RemoveAt(i);
                GameObject questUI = _questUIList[i];
                _questUIList.RemoveAt(i);
                Destroy(questUI);
                UpdateQuestUI();
                
                return text;
            }
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


    private void UpdateQuestUI()
    {
        for (int i = 0; i < _acceptedQuestList.Count; i++)
        {
            Quest quest = _acceptedQuestList[i];
            GameObject questUI = _questUIList[i]; 

            TextMeshProUGUI countText = questUI.transform.Find("Count")?.GetComponent<TextMeshProUGUI>();
            if (countText != null)
            {
                countText.text = $"{quest.Count} / {quest.goal}";
            }
        }
    }

    //테스트용
    void Update()
    {
        //랜덤 퀘스트
        if (Input.GetKeyDown(KeyCode.G))
        {
            QuestPlusCount("학 랜덤 퀘스트 1");QuestPlusCount("토끼 랜덤 퀘스트 1");QuestPlusCount("뱀 랜덤 퀘스트 1");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            QuestPlusCount("학 랜덤 퀘스트 2");QuestPlusCount("토끼 랜덤 퀘스트 2");QuestPlusCount("뱀 랜덤 퀘스트 2");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            QuestPlusCount("학 랜덤 퀘스트 3");QuestPlusCount("토끼 랜덤 퀘스트 3");QuestPlusCount("뱀 랜덤 퀘스트 3");
        }
        
        
        //메인 퀘스트
        if (Input.GetKeyDown(KeyCode.B))
        {
            QuestPlusCount("학 퀘스트1");QuestPlusCount("토끼 퀘스트 1");QuestPlusCount("뱀 랜덤 퀘스트 1");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            QuestPlusCount("학 퀘스트2");QuestPlusCount("토끼 퀘스트 2");QuestPlusCount("뱀 랜덤 퀘스트 2");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            QuestPlusCount("학 퀘스트3");QuestPlusCount("토끼 퀘스트 3");QuestPlusCount("뱀 랜덤 퀘스트 3");
        }
    }
}
