using System;
using System.Collections.Generic;
using UnityEngine;


public class BuildingObject : MonoBehaviour
{
   [SerializeField] private MeshRenderer meshRenderer;
   [SerializeField] private Material builtMaterial;
   [SerializeField] private Material fadeMaterial;
   
   [Space(10f)]
   [SerializeField] private BuildingSnapPoint[] snapPoints;



   public void InitToBuilding()
   {
      meshRenderer.material = fadeMaterial;
      
      gameObject.layer = 2;
   }
   
   public void UpdateToBuildingState(bool isBuildable) => meshRenderer.enabled = isBuildable;

   public void Built()
   {
      meshRenderer.material = builtMaterial;
      
      gameObject.layer =  0;
   }

   public bool IsSnappable() => snapPoints.Length > 0;
  

   

   public BuildingSnapPoint GetClosestSnapPointToHit(Vector3 hitPoint)
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

   public BuildingSnapPoint GetClosestSnapPointToSnapPoint(BuildingSnapPoint targetSnapPoint)
   {
      BuildingSnapPoint tempSnapPoint = null;

      float tempDist = float.MaxValue;

      foreach (var item in snapPoints)
      {
         if (item.Axis == BuildingSnapPoint.SnapAxis.All ||
             item.Axis ==  targetSnapPoint.Axis)
         {
            float compareDist = Vector3.Distance(item.transform.position, targetSnapPoint.transform.position);
            
            if (compareDist < tempDist)
            {
               tempDist = compareDist;

               tempSnapPoint = item;
            }
         }
      }
      
      return tempSnapPoint;
   }
}
