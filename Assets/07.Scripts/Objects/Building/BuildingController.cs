using System;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    [SerializeField] private float rayDistance;


    private BuildingObject _curBuildingObject;

    private bool _isBuildable;

    public void Enable(BuildingObject buildingObjectPrefab)
    {
        _curBuildingObject = Instantiate(buildingObjectPrefab);

        _curBuildingObject.Init();
    }

    public void Disable() => Destroy(_curBuildingObject.gameObject);


    public bool TryBuild()
    {
        if (_isBuildable && _curBuildingObject != null)
        {
            _curBuildingObject.Built();

            _curBuildingObject = null;

            return true;
        }

        return false;
    }


    public void UpdateBuildingObject()
    {
        if (_curBuildingObject == null)
        {
            return;
        }

        _isBuildable = false;

        Ray ray = targetCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            _curBuildingObject.transform.position = hit.point;

            Snap(hit);

            _isBuildable = true;
        }

        _curBuildingObject.UpdateToBuildingState(_isBuildable);
    }


    void Snap(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out BuildingObject targetObject))
        {
            if (targetObject.IsSnappable && _curBuildingObject.IsSnappable)
            {
                BuildingSnapPoint targetSnapPoint = targetObject.GetSnapPointClosestHit(hit.point);

                BuildingSnapPoint curSnapPoint = _curBuildingObject.GetSnapPointClosestTargetPoint(targetSnapPoint);

                if (curSnapPoint != null)
                {
                    Vector3 offset = targetSnapPoint.transform.position - curSnapPoint.transform.position;

                    _curBuildingObject.transform.position += offset;
                }
            }
        }
    }
}
