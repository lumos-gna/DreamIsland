using UnityEngine;


public class BuildingObject : MonoBehaviour
{
   public bool IsSnappable => snapPoints.Length > 0;

   
   [SerializeField] private GameObject buildingObj;
   [SerializeField] private GameObject previewObj;
   
   [Space(10f)]
   [SerializeField] private BuildingSnapPoint[] snapPoints;


   public void Init()
   {
      buildingObj.SetActive(false);
      previewObj.SetActive(true);
   }
   
   
   public void UpdateToBuildingState(bool isBuildable) => previewObj.SetActive(isBuildable);
   

   public void Built()
   {
      buildingObj.SetActive(true);
      previewObj.SetActive(false);
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

   
   
   public BuildingSnapPoint GetSnapPointClosestTargetPoint(BuildingSnapPoint targetPoint)
   {
      BuildingSnapPoint tempSnapPoint = null;

      float tempDist = float.MinValue;

      foreach (var item in snapPoints)
      {
         if (item.Axis == BuildingSnapPoint.SnapAxis.All ||
             item.Axis ==  targetPoint.Axis)
         {
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
