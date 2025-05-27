using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : BaseEnemy
{
    [SerializeField] private AnimalStats _animalStats;
    public override AnimalStats AnimalStats => _animalStats;
   
    // 움직이는 전체 범위 확인
    private void OnDrawGizmosSelected()
    {
        if (_animalStats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_animalStats.WanderCenter.position, _animalStats.WanderRadius);
        }
    }
}
