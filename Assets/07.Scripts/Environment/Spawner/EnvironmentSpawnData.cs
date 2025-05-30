// EnvironmentSpawnData.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class EnvironmentSpawnData : MonoBehaviour
{
    // 파괴 정보를 저장하는 struct
    public struct Info
    {
        public int prefabIndex;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    // 파괴된 오브젝트 정보만 모아두는 static 리스트
    public static List<Info> destroyedList = new List<Info>();

    // 이 인스턴스의 스폰 정보
    private int _prefabIndex;
    private Quaternion _rotation;
    private Vector3 _scale;

    /// <summary>
    /// 랜덤스포너에서 호출: 프리팹 인덱스, 위치, 회전, 스케일 전달
    /// </summary>
    public void InitializeAsLanded(int prefabIndex, Vector3 landedPoint, Quaternion rot, Vector3 scale)
    {
        _prefabIndex = prefabIndex;
        _rotation = rot;
        _scale = scale;

        transform.position = landedPoint;
        transform.rotation = rot;
        transform.localScale = scale;

        // Rigidbody 있으면 제거
        var rb = GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);
    }

    // 오브젝트가 Destroy 될 때, 이 정보를 static 리스트에 기록
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
