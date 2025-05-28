using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemEquip : MonoBehaviour
{
    [SerializeField] private GameObject[] tempitems = new GameObject[10];
    [SerializeField] private Transform EquipParent;
    private GameObject nowchoiceitem;

    public void ChoiceItem(int slot)// �������� ����� �Լ�?
    {
        if (tempitems[slot] == null)
        {
            Destroy(nowchoiceitem);
            return;
        }
        Destroy(nowchoiceitem);
        nowchoiceitem = Instantiate(tempitems[slot], EquipParent);
    }

    public void OnChoiceitemInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            string keyname = context.control.name;

            if (int.TryParse(keyname, out int selectslot))
            {
                Debug.Log($"{selectslot - 1} ���� ����");
                ChoiceItem(selectslot - 1);
            }
        }
    }
}
