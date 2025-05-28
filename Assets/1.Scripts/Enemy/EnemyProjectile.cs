using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private int _damage;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }
    public void SetDamage(int damage)
    {
        _damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 캐릭터 생명력 관련 로직 예시
            /*
            if (other.TryGetComponent<PlayerHandler>(out var player))
            {
                player.TakeDamage(_damage);
            }*/
            Destroy(gameObject);
        }

    }
}
