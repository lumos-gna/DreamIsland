// EnvironmentSpawnData.cs
using UnityEngine;

public class EnvironmentSpawnData : MonoBehaviour
{
    public Vector3 LandedPosition { get; private set; }
    public float LandedTime { get; private set; }

    public void InitializeAsLanded(Vector3 landedPoint)
    {
        LandedPosition = landedPoint;
        LandedTime = Time.time;

        //리기드바디와 콜라이더 제거
        var rb = GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);
        var col = GetComponent<Collider>();
        if (col != null) Destroy(col);
    }
}
