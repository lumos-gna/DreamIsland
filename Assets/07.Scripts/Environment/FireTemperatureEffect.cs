using UnityEngine;

public class FireTemperatureEffect : MonoBehaviour
{
    [Header("효과 범위")]
    [SerializeField]
    private float effectRadius = 5f; // 효과 반경 

    [Header("온도 상승 속도(초단위)")]
    [SerializeField]
    private float temperatureChangeRate = 2f;

    private Transform playerTransform;
    private PlayerCondition playerCondition;

    private void Start()
    {
        //  PlayerCondition 캐싱
        playerCondition = FindObjectOfType<PlayerCondition>();
        if (playerCondition != null)
        {
            playerTransform = playerCondition.transform;
        }
    }

    private void Update()
    {
        if (playerCondition == null || playerTransform == null)
            return;

        // 플레이어와 불 오브젝트 거리 계산
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        // 범위 이내만 온도 상승
        if (distanceToPlayer <= effectRadius)
        {
            float changeAmount = temperatureChangeRate * Time.deltaTime;
            playerCondition.RedTempChange(changeAmount);
        }
    }

    // 이펙트 거리 시각화(Gizmo)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red * 0.5f;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}
