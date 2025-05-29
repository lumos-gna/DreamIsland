using System;
using UnityEngine;

// Player와 관련된 기능을 모아두는 곳.
// 이곳을 통해 기능에 각각 접근. (ex. CharacterManager.Instance.Player.Controller)
public class TestPlayer : MonoBehaviour
{
    public TestPlayerController controller;
    //public PlayerCondition condition;
    //public Equipment equip;

    public ItemData itemData;
    public Action<ItemData> addItem;

    public Transform dropPosition;

    private void Awake()
    {
        // 싱글톤 매니저에 Player를 참조할 수 있게 데이터를 넘긴다.
        CharacterManager.Instance.Player = this;
        UIManager.Instance.Create<UIInventory>();
        controller = GetComponent<TestPlayerController>();
        //condition = GetComponent<PlayerCondition>();
        //equip = GetComponent<Equipment>();
    }
}