using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 건축 가능한 오브젝트의 동작과 상태를 담당하는 클래스.
/// 실제 건물과 프리뷰 오브젝트, 스냅 포인트 관련 로직을 포함함.
/// </summary>
public class BuildingObject : MonoBehaviour
{
    // 이 오브젝트가 스냅 가능한지 여부 (스냅 포인트가 하나 이상 있어야 함)
    public bool IsSnappable => snapPoints.Length > 0;

    [SerializeField] private Collider buildingColl;         // 설치 완료된 건물의 콜라이더
    [SerializeField] private GameObject buildingObj;        // 실제 건축될 오브젝트
    [SerializeField] private GameObject previewObj;         // 배치 프리뷰용 오브젝트

    [Space(10f)]
    [SerializeField] private BuildingSnapPoint[] snapPoints; // 이 오브젝트가 가진 스냅 포인트들

    /// <summary>
    /// 건축 준비 상태로 초기화 (프리뷰 모드 활성화).
    /// </summary>
    public void Init()
    {
        buildingColl.enabled = false;       // 콜라이더 비활성화 (설치 전 충돌 방지)
        buildingObj.SetActive(false);       // 실제 오브젝트 비활성화
        previewObj.SetActive(true);         // 프리뷰 활성화
    }

    /// <summary>
    /// 현재 위치에서 건축이 가능한지에 따라 프리뷰 오브젝트 표시를 조정함.
    /// </summary>
    public void UpdateToBuildingState(bool isBuildable) => previewObj.SetActive(isBuildable);

    /// <summary>
    /// 건축 확정 시 실행되는 로직. 실제 오브젝트 활성화 및 애니메이션 처리.
    /// </summary>
    public void Built()
    {
        Vector3 defalutScale = buildingObj.transform.localScale;
        buildingObj.transform.localScale = Vector3.zero;       // 스케일 0으로 설정 후

        buildingObj.SetActive(true);                           // 실 오브젝트 활성화

        // 설치 애니메이션 (DOTween 사용)
        buildingObj.transform.DOScale(defalutScale, 0.5f)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                buildingColl.enabled = true;     // 콜라이더 활성화
                previewObj.SetActive(false);     // 프리뷰 비활성화
            });
    }

    /// <summary>
    /// 레이캐스트 충돌 지점에서 가장 가까운 스냅 포인트를 반환함.
    /// </summary>
    public BuildingSnapPoint GetSnapPointClosestHit(Vector3 hitPoint)
    {
        BuildingSnapPoint snapPoint = null;
        float tempDist = float.MaxValue;

        foreach (var item in snapPoints)
        {
            float compareDist = Vector3.Distance(item.transform.position, hitPoint);
            if (compareDist < tempDist)
            {
                tempDist = compareDist;
                snapPoint = item;
            }
        }
        return snapPoint;
    }

    /// <summary>
    /// 대상 스냅 포인트와 가장 잘 맞는 현재 오브젝트의 스냅 포인트를 반환.
    /// 축(axis)가 같은 포인트 중 위치가 가장 잘 맞는 포인트를 선택함.
    /// </summary>
    public BuildingSnapPoint GetSnapPointClosestTargetPoint(BuildingSnapPoint targetPoint, Vector3 lookDir)
    {
        BuildingSnapPoint tempSnapPoint = null;
        float tempDist = float.MinValue;

        foreach (var item in snapPoints)
        {
            if (item.Axis == targetPoint.Axis)
            {
                // 수직 스냅 축의 경우, 플레이어 시선 방향에 따라 반대 위치는 제외
                if (item.Axis == BuildingSnapPoint.SnapAxis.Vertical)
                {
                    if (lookDir.y > 0 && item.transform.localPosition.y < 0 ||
                        lookDir.y < 0 && item.transform.localPosition.y > 0)
                        continue;
                }

                float compareDist = Vector3.Distance(item.transform.localPosition, targetPoint.transform.localPosition);

                if (compareDist > tempDist)
                {
                    tempDist = compareDist;
                    tempSnapPoint = item;
                }
            }
        }
        return tempSnapPoint;
    }
}
