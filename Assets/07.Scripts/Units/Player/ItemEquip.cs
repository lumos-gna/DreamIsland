using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemEquip : MonoBehaviour
{
    [SerializeField] private ItemData[] tempitems = new ItemData[10];
    [SerializeField] private Transform EquipParent;
    private GameObject nowchoiceitem;
    private Animator nowanimator;
    private int nowslot = 8;
    private bool attacking = false;
    private bool eating = false;


    public Animator NowAnimator
    {
        get { return nowanimator; }
    }
    public bool Attacking
    {
        get { return attacking; }
        set { attacking = value; }
    }

    public bool Eating
    {
        get { return eating; }
        set { eating = value; }
    }

    // 슬롯 설정 메서드
    public void SetSlotItem(int index, ItemData item)
    {
        if (index < 0 || index >= tempitems.Length) return;
        tempitems[index] = item;
    }

    public void ChoiceItem(int slot)// 아이템을 고르는 함수?
    {
        if (nowchoiceitem != null)
        {
            Destroy(nowchoiceitem);
        }
        if (tempitems[slot] == null)
        {
            nowchoiceitem = null;
            nowanimator = null;
            PlayerManager.Instance._Player.State = AttackState.Instance;
            return;
        }
        if (tempitems[slot].type == ItemType.Weapon) { PlayerManager.Instance._Player.State = AttackState.Instance; }
        else if (tempitems[slot].type == ItemType.Consumable) { PlayerManager.Instance._Player.State = ConsumeState.Instance; }
        nowchoiceitem = Instantiate(tempitems[slot].equipPrefab, EquipParent);
        nowanimator = nowchoiceitem.GetComponent<Animator>();
    }

    public void OnChoiceitemInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            string keyname = context.control.name;

            if (int.TryParse(keyname, out int selectslot))
            {
                if (nowslot != selectslot)
                {
                    Debug.Log($"{selectslot - 1} 슬롯 선택");
                    ChoiceItem(selectslot - 1);
                    nowslot = selectslot-1;
                }
            }
        }
    }

    public void OnLeftClickInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && PlayerManager.Instance._Player._PlayerController.Canlook)
        {
            Debug.Log(nowslot);
            PlayerManager.Instance._Player.State.UseItem(ref tempitems[nowslot]);
        }
    }

    public void StartAttackCooldown(float duration)
    {
        StartCoroutine(AttackCooldownCoroutine(duration));
    }

    private IEnumerator AttackCooldownCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Attacking = false;
    }

    public void StartEatCooldown(float duration)
    {
        StartCoroutine(EatCooldownCoroutine(duration));
    }

    private IEnumerator EatCooldownCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        eating = false;
    }
}
