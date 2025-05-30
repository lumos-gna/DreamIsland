using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    private ParticleSystem _dust;

    private void Awake()
    {
        _dust = GetComponentInChildren<ParticleSystem>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<PlayerCondition>(out var condition))
            {
                condition.HealthChange(-damage);
            }
        }

        Vector3 spawnPos = transform.position;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
        {
            spawnPos = hit.point;
        }

        // 먼지 파티클 재생
        if (_dust != null)
        {
            _dust.transform.parent = null; // 부모에서 분리
            _dust.transform.position = spawnPos; // 바닥 위치로 이동
            _dust.gameObject.SetActive(true);
            _dust.Play();

            Destroy(_dust.gameObject, _dust.main.duration + _dust.main.startLifetime.constantMax);
        }

        Destroy(gameObject,1f);
    }
}
