using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemEquip : MonoBehaviour
{
    [SerializeField] private ItemData[] tempitems = new ItemData[10];
    [SerializeField] private Transform EquipParent;
    private GameObject nowchoiceitem;
    private int nowslot = -1;

    public void ChoiceItem(int slot)// 아이템을 고르는 함수?
    {
        if (tempitems[slot] == null)
        {
            Destroy(nowchoiceitem);
            nowchoiceitem = null;
            return;
        }
        if(nowchoiceitem != null)
        {
            Destroy(nowchoiceitem);
        }
        nowchoiceitem = Instantiate(tempitems[slot].equipPrefab, EquipParent);
    }

    public void OnChoiceitemInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            string keyname = context.control.name;

            if(int.TryParse(keyname, out int selectslot))
            {
                if(nowslot != selectslot)
                {
                    Debug.Log($"{selectslot - 1} 슬롯 선택");
                    ChoiceItem(selectslot - 1);
                    nowslot = selectslot;
                }
            }
        }
    }
}
