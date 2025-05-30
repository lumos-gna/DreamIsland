using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquippedController : MonoBehaviour
{
    public enum InputState
    {
        Up,
        Down
    }
    
    [SerializeField] private ItemDataSO tempItemData;
    [SerializeField] private Transform equipParent;
    
    public EquippedItem CurEquippedItem { get; set; }

    private InputState _inputState = InputState.Up;

    private void Start()
    {
        //테스트용
        CurEquippedItem = Instantiate(tempItemData.EquipItemPrefab, equipParent);
        CurEquippedItem.Equip(gameObject, tempItemData);
    }

    private void Update()
    {
        if (CurEquippedItem.TryUse(_inputState))
        {
            //후에 차징 (활) 처리를 위해 업데이트처리
            //인벤토리 슬롯 개수 갱신?
        }
    }


    public void OnLeftClickInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _inputState = InputState.Down;
                break;
            case InputActionPhase.Canceled:
                _inputState = InputState.Up;
                break;
        }
    }
}
