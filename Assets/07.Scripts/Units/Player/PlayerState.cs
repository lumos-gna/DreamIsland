using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


public interface PlayerState
{
    public void UseItem(ref ItemData tool); //아이템을 들고 클릭을하면 실행되는 함수
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
    public void UseItem(ref ItemData tool)
    {
        if (tool == null)
        {
            // 기본 공격 데미지, 사거리
            Debug.Log("맨손 공격");
        }
        else
        {
            // 아이템의 데미지, 사거리를 받아옴
            Debug.Log("무기 공격");
            AttackRange = tool.AttackRange;
            Damage = tool.AttackDamage;
        }
        if (PlayerManager.Instance._Player._ItemEquip.NowAnimator != null && !PlayerManager.Instance._Player._ItemEquip.Attacking)
        {
            PlayerManager.Instance._Player._ItemEquip.NowAnimator.SetTrigger(PlayerConst.AttckTrigger);
            PlayerManager.Instance._Player._ItemEquip.Attacking = true;
            SetTime();

            //공격 or 자원채취
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, AttackRange, PlayerManager.Instance._Player.EnemyLayerMask))
            {
                hit.collider.gameObject.GetComponent<BaseEnemy>().TakeDamage(Damage);
            }
        }
    }

    public void SetTime()
    {
        PlayerManager.Instance._Player._ItemEquip.StartAttackCooldown(attackrate);
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
    public void UseItem(ref ItemData tool)
    {
        if (PlayerManager.Instance._Player._ItemEquip.NowAnimator != null && !PlayerManager.Instance._Player._ItemEquip.Eating)
        {
            PlayerManager.Instance._Player._ItemEquip.NowAnimator.SetTrigger(PlayerConst.EatTrigger);
            PlayerManager.Instance._Player._ItemEquip.Eating = true;
            SetTime();
            //아이템 사용(섭취)
            if (tool.consumetype == ConsumType.health)
            {
                PlayerManager.Instance._Player._PlayerCondition.HealthChange(tool.healamount);
                Debug.Log("체력회복함");
                //체력 회복함
            }
            else if (tool.consumetype == ConsumType.hunger)
            {
                //PlayerManager.Instance._Player._PlayerCondition.HungerChange(tool.healamount);
                Debug.Log("배고픔 회복함");
                //배고픔 회복함
            }
            else if (tool.consumetype == ConsumType.water)
            {
                PlayerManager.Instance._Player._PlayerCondition.WaterChange(tool.healamount);
                Debug.Log("목마름 회복함");
                //목마름 회복함
            }
        }
    }
    private float eatrate = 1.5f;
    public void SetTime()
    {
        PlayerManager.Instance._Player._ItemEquip.StartEatCooldown(eatrate);
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
    public void UseItem(ref ItemData tool)
    {
        //아이템 설치(building)
    }
    public void SetTime()
    {

    }
}


