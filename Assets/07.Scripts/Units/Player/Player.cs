using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //private PlayerState state = AttackState.Instance;
    private PlayerController _playerController;
    private PlayerCondition _playerCondition;
    public ItemData itemData;
    //private ItemEquip _itemEquip;
    private LayerMask enemylayermask;
    public Action<ItemData> addItem;

    public PlayerController _PlayerController
    {
        get { return _playerController; }
    }

    public PlayerCondition _PlayerCondition
    {
        get { return _playerCondition; }
    }

    //public ItemEquip _ItemEquip
    //{
    //    get { return _itemEquip; }
    //}

    //public PlayerState State
    //{
    //    get { return state; }
    //    set { state = value; }
    //}

    public LayerMask EnemyLayerMask
    {
        get { return enemylayermask; }
    }

    private void Awake()
    {
        PlayerManager.Instance._Player = this;
        UIManager.Instance.Create<InventoryUI>();
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCondition = GetComponent<PlayerCondition>();
        //_itemEquip = GetComponent<ItemEquip>();
        enemylayermask = 1 << LayerMask.NameToLayer("Enemy");
    }
}
