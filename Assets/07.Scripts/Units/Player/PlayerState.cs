using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


public interface PlayerState
{
    //public void UseItem(ref ItemData tool); //아이템을 들고 클릭을하면 실행되는 함수
    public void SetTime();
}

public class AttackState : PlayerState
{
    private static AttackState instance;

    public static AttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AttackState();
            }
            return instance;
        }
    }

    private float attackrate = 1f;
    private float AttackRange = 3f; // 나중에 attackrange랑 damage전부 아이템에서 받아와야함 일단 임시로 설정 
    private int Damage = 10;
    /*public void UseItem(ref ItemData tool)
    {
      
    }*/

    public void SetTime()
    {
    }
}

public class ConsumeState : PlayerState
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
    /*public void UseItem(ref ItemData tool)
    {
        
    }*/
    private float eatrate = 1.5f;
    public void SetTime()
    {
    }
}

public class BuildingState : PlayerState
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
    /*public void UseItem(ref ItemData tool)
    {
        //아이템 설치(building)
    }*/
    public void SetTime()
    {

    }
}


