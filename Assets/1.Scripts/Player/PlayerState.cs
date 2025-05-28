using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


public interface PlayerState
{
    public void UseItem(); //�������� ��� Ŭ�����ϸ� ����Ǵ� �Լ�
}

public class AttackState : MonoBehaviour, PlayerState
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
    public void UseItem()
    {
        //����
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
        //������ ���(����)
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
        //������ ��ġ(building)
    }
}


