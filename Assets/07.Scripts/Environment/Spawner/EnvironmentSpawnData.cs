using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnvironmentSpawnData : MonoBehaviour
{
    public Vector3 LandedPosition { get; private set; }
    public float LandedTime { get; private set; }

    public void InitializeAsLanded(Vector3 landedPoint)
    {
        LandedPosition = landedPoint;
        LandedTime = Time.time;

        // Rigidbody있다면 제거
        var rb = GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);

    }
}
