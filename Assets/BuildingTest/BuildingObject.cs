using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class BuildingObject : MonoBehaviour
{
   [SerializeField] private Material builtMaterial;
   [SerializeField] private Material fadeMaterial;
   
   [SerializeField] private LayerMask defaultLayer;
   [SerializeField] private LayerMask ignoreLayer;

   [Space(10f)]
   [SerializeField] private Transform[] snapPoints;

   
   private MeshRenderer _meshRenderer;
   
   
   private void Awake()
   {
      _meshRenderer = GetComponent<MeshRenderer>();
   }


   public void InitToBuilding()
   {
      _meshRenderer.material = fadeMaterial;

      gameObject.layer = 1 << ignoreLayer;
   }
   
   public void UpdateToBuildingState(bool isBuildable) => _meshRenderer.enabled = isBuildable;

   public void Built()
   {
      _meshRenderer.material = builtMaterial;
      
      gameObject.layer =  1 << defaultLayer;
   }

   public bool IsSnappable() => snapPoints.Length > 0;
  

   

   public Vector3 GetCloseSnapPoint(Vector3 inputPos)
   {
      int closerIndex = int.MaxValue;
      
      float tempDist = float.MaxValue;

      for (int i = 0; i < snapPoints.Length; i++)
      {
         float compareDist = Vector3.Distance(snapPoints[i].position, inputPos);

         if (compareDist < tempDist)
         {
            tempDist = compareDist;

            closerIndex = i;
         }
      }

      return snapPoints[closerIndex].position;
   }
}
