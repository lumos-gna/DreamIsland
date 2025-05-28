using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class EnvironmentSpawnData : MonoBehaviour
{
    [Header("착지 허용 레이어")]
    public LayerMask landingLayers = ~0;

    [Header("랜덤 스케일 범위")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.5f);

    public Vector3 LandedPosition { get; private set; }
    public float LandedTime { get; private set; }

    private Rigidbody _rb;
    private bool _hasLanded = false;
    private int _groundLayer;

    void Awake()
    {
        // Ground 레이어 인덱스 캐시
        _groundLayer = LayerMask.NameToLayer("Ground");

        // 랜덤 스케일 적용
        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        transform.localScale *= randomScale;

        // Rigidbody 확보
        _rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Collider 확보
        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // 이미 처리된 경우 무시
        if (_hasLanded)
            return;

        // 첫 충돌이 Ground 레이어가 아니라면 객체 파괴
        if (collision.gameObject.layer != _groundLayer)
        {
            Destroy(gameObject);
            return;
        }

        // 충돌 정보 유효성 검사
        if (collision.contacts == null || collision.contacts.Length == 0)
            return;

        // 첫 번째 접촉 지점 기록
        ContactPoint contact = collision.contacts[0];
        LandedPosition = contact.point;
        LandedTime = Time.time;
        _hasLanded = true;

        // 완전 고정
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;

    }
}