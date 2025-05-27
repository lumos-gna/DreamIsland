using System;
using System.Collections.Generic;
using UnityEngine;


public class BuildingObject : MonoBehaviour
{
   public bool IsSnappable => snapPoints.Length > 0;
   
   [SerializeField] private MeshRenderer meshRenderer;
   [SerializeField] private Material builtMaterial;
   [SerializeField] private Material fadeMaterial;
   
   [Space(10f)]
   [SerializeField] private BuildingSnapPoint[] snapPoints;


   public void Init()
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
