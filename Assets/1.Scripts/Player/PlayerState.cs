using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


public interface PlayerState
{
    public void UseItem(); //아이템을 들고 클릭을하면 실행되는 함수
}

public class AttackState : MonoBehaviour, PlayerState
{
    private static AttackState instance;

    public static AttackState Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new AttackState();
            }
            return instance;
        }
    }
    public void UseItem() 
    {
        //공격
    }
}

public class ConsumeState : MonoBehaviour, PlayerState
{
    private static ConsumeState instance;

    public static ConsumeState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ConsumeState();
            }
            return instance;
        }
    }
    public void UseItem()
    {
        //아이템 사용(섭취)
    }
}

public class BuildingState : MonoBehaviour, PlayerState
{
    private static BuildingState instance;

    public static BuildingState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BuildingState();
            }
            return instance;
        }
    }
    public void UseItem()
    {
        //아이템 설치(building)
    }
}


