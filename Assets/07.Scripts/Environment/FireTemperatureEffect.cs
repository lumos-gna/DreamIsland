using UnityEngine;

public class FireTemperatureEffect : MonoBehaviour
{
    [Header("ȿ�� ����")]
    [SerializeField]
    private float effectRadius = 5f; // ȿ�� �ݰ� 

    [Header("�µ� ��� �ӵ�(�ʴ���)")]
    [SerializeField]
    private float temperatureChangeRate = 2f;

    private Transform playerTransform;
    private PlayerCondition playerCondition;

    private void Start()
    {
        //  PlayerCondition ĳ��
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

        // �÷��̾�� �� ������Ʈ �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        // ���� �̳��� �µ� ���
        if (distanceToPlayer <= effectRadius)
        {
            float changeAmount = temperatureChangeRate * Time.deltaTime;
            playerCondition.RedTempChange(changeAmount);
        }
    }

    // ����Ʈ �Ÿ� �ð�ȭ(Gizmo)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red * 0.5f;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}
