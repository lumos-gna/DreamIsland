
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [Header("Info")] 
    public string name;
    public string text;
    
    [Header("Count")] 
    public int goal;
    public int reward;   //나중에 아이템으로 변경

    private int _count = 0;
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
                _clear = true; _onOff = false; _count = 0;
                Reward();
            }
        }
    }
    
    public bool OnOff(){return _onOff;}  //퀘스트 수락상태 리턴

    
    private void Reward()   //아이템 보상 떨굼
    {
        Debug.Log("보상 획득" + reward);
    }
}



[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Quest")]
public class QuestData : ScriptableObject
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

    public void Reset(){foreach (Quest quest in quests){quest.Reset();}}
}
