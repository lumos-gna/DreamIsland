using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class EnvironmentSpawnData : MonoBehaviour
{
    public struct Info
    {
        public int prefabIndex;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    public static List<Info> destroyedList = new List<Info>();

    private int _prefabIndex;
    private Quaternion _rotation;
    private Vector3 _scale;

    public int PrefabIndex => _prefabIndex;

    public void InitializeAsLanded(int prefabIndex, Vector3 landedPoint, Quaternion rot, Vector3 scale)
    {
        _prefabIndex = prefabIndex;
        _rotation = rot;
        _scale = scale;

        transform.position = landedPoint;
        transform.rotation = rot;
        transform.localScale = scale;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
            Destroy(rb);
    }

    void OnDestroy()
    {
        destroyedList.Add(new Info
        {
            prefabIndex = _prefabIndex,
            position = transform.position,
            rotation = _rotation,
            scale = _scale
        });
    }
}
