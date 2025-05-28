using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoolAnimal : MonoBehaviour
{
    private float _wanderRadius;
    private Transform _wanderCenter;
    public float GetWanderRadius() => _wanderRadius;
    public Transform GetWanderCenter() => _wanderCenter;
    public Action<PoolAnimal> OnDie; // 죽었을때 호출되는 액션

    public void Init(Transform wanderCenter, float wanderRadius)
    {
        _wanderRadius = wanderRadius;
        _wanderCenter = wanderCenter;
        OnSpawn();
    }

    public void Die()
    {
        OnDespawn();
        OnDie?.Invoke(this);
    }

    public void OnSpawn() => gameObject.SetActive(true);
    public void OnDespawn() => gameObject.SetActive(false);
}
