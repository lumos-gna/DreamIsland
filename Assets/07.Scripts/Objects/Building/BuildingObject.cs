using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class BuildingObject : MonoBehaviour
{
   public bool IsSnappable => snapPoints.Length > 0;

   
   [SerializeField] private Collider buildingColl;

   [SerializeField] private GameObject buildingObj;
   [SerializeField] private GameObject previewObj;
   
   [Space(10f)]
   [SerializeField] private BuildingSnapPoint[] snapPoints;


   public void Init()
   {
      buildingColl.enabled = false;
      buildingObj.SetActive(false);
      previewObj.SetActive(true);
   }
   
   
   public void UpdateToBuildingState(bool isBuildable) => previewObj.SetActive(isBuildable);
   

   public void Built()
   {
      Vector3 defalutScale = buildingObj.transform.localScale;

      buildingObj.transform.localScale = Vector3.zero;
      
      buildingObj.SetActive(true);

      buildingObj.transform.DOScale(defalutScale, 0.5f).SetEase(Ease.OutExpo).OnComplete(
            () =>
            {
               buildingColl.enabled = true;
               previewObj.SetActive(false);
            });
   }
   

   
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

   
   
   public BuildingSnapPoint GetSnapPointClosestTargetPoint(BuildingSnapPoint targetPoint, Vector3 lookDir)
   {
      BuildingSnapPoint tempSnapPoint = null;

      float tempDist = float.MinValue;
      
      foreach (var item in snapPoints)
      {
         if (item.Axis ==  targetPoint.Axis)
         {
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
