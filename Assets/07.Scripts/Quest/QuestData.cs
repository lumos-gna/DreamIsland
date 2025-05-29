
using UnityEngine;

[System.Serializable]
public class Quest
{
    [Header("Info")] 
    public string name;
    public string text;   //퀘스트 수령, 수락상태일 때 NPC가 말하는 텍스트
    public string uiText;  //UI창에 뜨는 퀘스트 설명
    public string clearText;  //퀘스트 클리어시 텍스트
    
    [Header("Count")] 
    public int goal;      //목표 수치
    public int reward;   //나중에 아이템으로 변경

    private int _count;    //퀘스트 진행도
    private bool _clear;  //클리어 여부


    public void Reset()  //퀘스트 진행사항 리셋
    {
        _count = 0;
        _clear = false;
    }

    public void PlusCount()  //퀘스트 진행도 +1
    {
        if (!_clear)
        {
            _count++;
            Debug.Log(_count + " / " + goal);
                        
            if (_count == goal){ 
                Debug.Log("클리어!");
                _clear = true; _count = 0;
            }
        }
        else{Debug.Log(_clear);}
    }

    public bool Clear(){return _clear;} 
    
    public void Reward()   //아이템 보상 떨굼
    {
        Debug.Log("보상 획득" + reward);
        _clear = false;
    }
}
