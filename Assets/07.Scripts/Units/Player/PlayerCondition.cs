using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float water;
    [SerializeField] private float hunger;

    private float DecreaseperFrame = 0.001f;
    private float DecreaseHealth = 0.1f;
    private float minf = 0f;
    private float maxf = 100f;


    private void Start()
    {
        health = 100f;
        water = 100f;
        hunger = 100f;
    }

    private void Update()
    {
        WaterChange(-DecreaseperFrame); // 목마름 계속 감소
        HungerChange(-DecreaseperFrame); // 배고픔 계속 감소
        if (water == minf) // 목마름이 0이면, 체력 감소
        {
            HealthChange(-DecreaseHealth);
        }
        if(hunger == minf)
        {
            HealthChange(-DecreaseHealth);
        }
    }

    public void HealthChange(float change) // 각 condition 변화 적용 함수
    {
        health = Mathf.Clamp(health + change, minf, maxf);
    }
    public void WaterChange(float change)
    {
        water = Mathf.Clamp(water + change, minf, maxf);
    }
    public void HungerChange(float change)
    {
        hunger = Mathf.Clamp(hunger + change, minf, maxf);
    }


}
