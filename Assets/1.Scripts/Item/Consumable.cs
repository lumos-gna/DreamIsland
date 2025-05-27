using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    Health,     // 체력
    Thirsty,    // 목마름
    Stamina,    // 스태미나
}

public class Consumable : MonoBehaviour, IConsumable
{
    public void Eating(float amount)
    {

    }

    public void Healing(float amount)
    {

    }
}
