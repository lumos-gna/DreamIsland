using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float water;
    [SerializeField] private float stamina;

    public float Health => health;
    public float Water => water;
    public float Stamina => stamina;

    private float waterDecreaseperFrame = 0.001f;
    private float thirstyDecreaseHealth = 0.1f;
    private float minf = 0f;
    private float maxf = 100f;



    private void Update()
    {
        WaterChange(-waterDecreaseperFrame); 
        if (water == minf) 
        {
            HealthChange(-thirstyDecreaseHealth);
        }
    }

    public void HealthChange(float change) 
    {
        health = Mathf.Clamp(health + change, minf, maxf);
    }
    public void WaterChange(float change)
    {
        water = Mathf.Clamp(water + change, minf, maxf);
    }
    public void StaminaChange(float change)
    {
        stamina = Mathf.Clamp(stamina + change, minf, maxf);
    }


}
