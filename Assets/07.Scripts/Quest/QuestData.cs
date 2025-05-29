
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [Header("Info")] 
    public string name;
    public string text;   //퀘스트 수령, 수락상태일 때 쓰는 텍스트
    public string clearText;  //퀘스트 클리어시 텍스트
    
    [Header("Count")] 
    public int goal;
    public int reward;   //나중에 아이템으로 변경

    private int _count;
    private bool _onOff;
    private bool _clear;


    public void Reset()  //퀘스트 진행사항 리셋
    {
        _count = 0;
        _onOff = false;
        _clear = false;
    }
    
    public int Count{ get; private set; }

    public void AcceptQuest()  //퀘스트 수락처리
    {
        _onOff = true;
        Debug.Log(text);
    }

    public void PlusCount()  //퀘스트 진행도 +1
    {
        if (_onOff)
        {
            _count++;
            Debug.Log(_count + " / " + goal);
            
            if (_count == goal){ 
                Debug.Log("클리어!");
                _clear = true; _onOff = false; _count = 0;
            }
        }
    }
    
    public bool OnOff(){return _onOff;}  //퀘스트 수락상태 리턴

    public bool Clear(){return _clear;} 
    
    public void Reward()   //아이템 보상 떨굼
    {
        Debug.Log("보상 획득" + reward);
        _clear = false;
    }
}




public class QuestData : MonoBehaviour
{
    [Header("Quest")]
    public Quest[] quests;

    public void AcceptQuest(int count){ quests[count].AcceptQuest();}
    
    public void CountQuest(int count){ quests[count].PlusCount(); }
    
    public int CheckOnOffQuest()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            if (quests[i].OnOff()){return i;}
        }
        return -1;
    }

    public int CheckClearQuest()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            if (quests[i].Clear()){return i;}
        }
        return -1;
    }

    public string GetQuestText(int i)
    {
        quests[i].Reward();
        return quests[i].text;
    }
    
    public void Reset(){foreach (Quest quest in quests){quest.Reset();}}
}
