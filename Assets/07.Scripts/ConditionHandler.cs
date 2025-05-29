using System;
using UnityEngine;
using UnityEngine.Events;

public class ConditionHandler : MonoBehaviour
{
    public event UnityAction OnTakeDamage;
    public event UnityAction OnDie;
    public enum ObjectType
    {
        Object,
        Unit
    }
    public ObjectType Type => type;
    public float Maxhealth => maxHealth;

    public float CurHealth { get; private set; }

    [SerializeField] private ObjectType type;

    [SerializeField] private float maxHealth;
    

    private void Awake()
    {
        CurHealth = Maxhealth;
    }

    public void TakeDamage(float damage)
    {
        CurHealth -= damage;
        
        OnTakeDamage?.Invoke();

        if (CurHealth <= maxHealth)
        {
            OnDie?.Invoke();
        }
    }
    
    
}
