// EnvironmentSpawnData.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class EnvironmentSpawnData : MonoBehaviour
{
    // �ı� ������ �����ϴ� struct
    public struct Info
    {
        public int prefabIndex;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    // �ı��� ������Ʈ ������ ��Ƶδ� static ����Ʈ
    public static List<Info> destroyedList = new List<Info>();

    // �� �ν��Ͻ��� ���� ����
    private int _prefabIndex;
    private Quaternion _rotation;
    private Vector3 _scale;

    /// <summary>
    /// ���������ʿ��� ȣ��: ������ �ε���, ��ġ, ȸ��, ������ ����
    /// </summary>
    public void InitializeAsLanded(int prefabIndex, Vector3 landedPoint, Quaternion rot, Vector3 scale)
    {
        _prefabIndex = prefabIndex;
        _rotation = rot;
        _scale = scale;

        transform.position = landedPoint;
        transform.rotation = rot;
        transform.localScale = scale;

        // Rigidbody ������ ����
        var rb = GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);
    }

    // ������Ʈ�� Destroy �� ��, �� ������ static ����Ʈ�� ���
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
