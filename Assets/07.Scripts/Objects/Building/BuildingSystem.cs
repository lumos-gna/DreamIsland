using System;
using UnityEngine;

public class BuildingSystem
{
    private const float RayDistance = 5f;
    private readonly Camera _camera = Camera.main;

    private BuildingObject _buildingObject;
    private bool _isBuildable;

    public void Create(BuildingObject prefab)
    {
        _buildingObject = GameObject.Instantiate(prefab);
        _buildingObject.Init();
    }

    public void Destroy()
    {
        if (_buildingObject != null)
        {
            GameObject.Destroy(_buildingObject.gameObject);
        }
    }

    public void Rotation()
    {
        if (_buildingObject != null)
        {
            float angle = _buildingObject.transform.eulerAngles.y + 90f;

            angle = Mathf.Repeat(angle, 360f);

            _buildingObject.transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }


    public bool TryBuild()
    {
        if (_isBuildable && _buildingObject != null)
        {
            _buildingObject.Built();

            _buildingObject = null;

            return true;
        }

        return false;
    }

    public void UpdateBuildingObject()
    {
        if (_buildingObject == null)
        {
            return;
        }

        _isBuildable = false;

        Ray ray = _camera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit hit, RayDistance))
        {
            _buildingObject.transform.position = hit.point;

            if (hit.rigidbody != null)
            {
                Snap(hit, ray.direction.normalized);
            }
            _isBuildable = true;
        }
        _buildingObject.UpdateToBuildingState(_isBuildable);
    }


    void Snap(RaycastHit hit, Vector3 rayDirNormalized)
    {
        if (hit.rigidbody.TryGetComponent(out BuildingObject targetObject))
        {
            if (targetObject.IsSnappable && _buildingObject.IsSnappable)
            {
                BuildingSnapPoint targetSnapPoint = targetObject.GetSnapPointClosestHit(hit.point);

                BuildingSnapPoint curSnapPoint
                    = _buildingObject.GetSnapPointClosestTargetPoint(targetSnapPoint, rayDirNormalized);

                if (curSnapPoint != null)
                {
                    Vector3 offset = targetSnapPoint.transform.position - curSnapPoint.transform.position;

                    _buildingObject.transform.position += offset;
                }
            }
        }
    }
}
