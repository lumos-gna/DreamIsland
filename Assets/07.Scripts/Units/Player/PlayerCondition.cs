using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float water = 100f;
    [SerializeField] private float hunger = 100f;

    [Header("PlayerConditionUI")]
    [SerializeField] private GameObject healthUI;
    [SerializeField] private GameObject HungerUI;
    [SerializeField] private GameObject WaterUI;

    private float DecreaseperFrame = 0.001f;
    private float DecreaseHealth = 0.1f;
    private float minf = 0f;
    private float maxf = 100f;



    private void Update()
    {
        HungerChange(-DecreaseperFrame);
        WaterChange(-DecreaseperFrame);
        if (water == minf) 
        {
            HealthChange(-DecreaseHealth);
        }
        if(hunger == minf)
        {
            HealthChange(-DecreaseHealth);
        }
        UpdatePlayerConditionUI();
    }

    public void HealthChange(float change) 
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

    private void UpdatePlayerConditionUI()
    {
        healthUI.GetComponent<Image>().fillAmount = health / maxf;
        HungerUI.GetComponent<Image>().fillAmount = hunger / maxf;
        WaterUI.GetComponent<Image>().fillAmount = water / maxf;
    }


}
