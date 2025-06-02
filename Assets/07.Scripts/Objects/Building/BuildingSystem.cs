using System;
using UnityEngine;

/// <summary>
/// 건축 시스템을 담당하는 클래스. 건물 배치, 회전, 설치 시 유효성 검사 등을 수행.
/// </summary>
public class BuildingSystem
{
    private const float RayDistance = 5f; 
    private readonly Camera _camera = Camera.main; 

    private BuildingObject _buildingObject; 
    private bool _isBuildable; 

    /// <summary>
    /// 건물 오브젝트를 생성하여 배치를 시작함.
    /// </summary>
    public void Create(BuildingObject prefab)
    {
        _buildingObject = GameObject.Instantiate(prefab); // 프리팹을 인스턴스화
        _buildingObject.Init(); // 초기화 작업 수행
    }

    /// <summary>
    /// 현재 배치 중인 건물 오브젝트를 파괴함.
    /// </summary>
    public void Destroy()
    {
        if (_buildingObject != null)
        {
            GameObject.Destroy(_buildingObject.gameObject);
        }
    }

    /// <summary>
    /// 현재 건물 오브젝트를 Y축 기준으로 90도 회전시킴.
    /// </summary>
    public void Rotation()
    {
        if (_buildingObject != null)
        {
            float angle = _buildingObject.transform.eulerAngles.y + 90f;
            angle = Mathf.Repeat(angle, 360f); // 0~360도 내로 제한
            _buildingObject.transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }

    /// <summary>
    /// 건물을 실제로 설치 시도. 설치 가능할 경우 완료 처리 후 true 반환.
    /// </summary>
    public bool TryBuild()
    {
        if (_isBuildable && _buildingObject != null)
        {
            _buildingObject.Built(); // 건물 설치 완료 처리
            _buildingObject = null; // 현재 배치 중인 오브젝트 해제
            return true;
        }

        return false;
    }

    /// <summary>
    /// 매 프레임마다 호출되어 건물의 위치 및 설치 가능 여부를 갱신함.
    /// </summary>
    public void UpdateBuildingObject()
    {
        if (_buildingObject == null)
            return;

        _isBuildable = false;

        // 화면 중앙에서 레이를 발사하여 충돌 위치를 감지
        Ray ray = _camera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit hit, RayDistance))
        {
            // 충돌 지점으로 건물 위치 이동
            _buildingObject.transform.position = hit.point;

            // 만약 충돌 대상이 Rigidbody를 갖고 있다면 Snap 시도
            if (hit.rigidbody != null)
            {
                Snap(hit, ray.direction.normalized);
            }

            _isBuildable = true;
        }

        // 건물 상태를 설치 가능/불가능 상태로 갱신
        _buildingObject.UpdateToBuildingState(_isBuildable);
    }

    /// <summary>
    /// 건물 스냅 기능: 인접한 건물의 스냅 포인트와 현재 건물의 포인트를 정렬시킴.
    /// </summary>
    void Snap(RaycastHit hit, Vector3 rayDirNormalized)
    {
        if (hit.rigidbody.TryGetComponent(out BuildingObject targetObject))
        {
            if (targetObject.IsSnappable && _buildingObject.IsSnappable)
            {
                // 충돌 지점과 가장 가까운 타겟의 스냅 포인트 찾기
                BuildingSnapPoint targetSnapPoint = targetObject.GetSnapPointClosestHit(hit.point);

                // 현재 건물에서 타겟 스냅 포인트와 가장 잘 맞는 포인트 찾기
                BuildingSnapPoint curSnapPoint
                    = _buildingObject.GetSnapPointClosestTargetPoint(targetSnapPoint, rayDirNormalized);

                if (curSnapPoint != null)
                {
                    // 스냅 포인트 간의 거리만큼 오프셋 이동하여 정렬
                    Vector3 offset = targetSnapPoint.transform.position - curSnapPoint.transform.position;
                    _buildingObject.transform.position += offset;
                }
            }
        }
    }
}
