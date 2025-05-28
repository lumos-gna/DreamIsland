using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerState state;
    private PlayerController _playerController;
    private PlayerCondition _playerCondition;
    private ItemEquip _itemEquip;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCondition = GetComponent<PlayerCondition>();
        _itemEquip = GetComponent<ItemEquip>();
    }
}
