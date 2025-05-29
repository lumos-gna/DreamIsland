using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableEnemy
{
    void OnSpawn();
    void OnDespawn();
    void Die();
    event Action<IPoolableEnemy> OnDie;
}
