using System;

public interface IPoolableEnemy
{
    void OnSpawn();
    void OnDespawn();
    void Die();
    event Action<IPoolableEnemy> OnRespawn;
}
