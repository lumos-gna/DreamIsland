using System;
using UnityEngine;

public class EquippedRange : EquippedItem
{
    private bool _isFinishDraw;
    private bool _isDrawing;
    
    private readonly int _isDraw = Animator.StringToHash("IsDraw");
    private readonly int _shoot = Animator.StringToHash("Shoot");

    public override void UnEquip()
    {
    }

    public override bool TryUse(EquippedController.InputState inputState)
    {
        if (!ItemData.IsRangeItem) return false;
        
        
        switch (inputState)
        {
            case EquippedController.InputState.Down :
                
                if (!_isDrawing)
                {
                    _isDrawing = true;
            
                    _animator.SetBool(_isDraw, true);
                }
                
                break;
            
            case EquippedController.InputState.Up :
                
                if (_isFinishDraw)
                {
                    Shoot();
                }
                else
                {
                    _animator.SetBool(_isDraw, false);
                }

                _isDrawing = false;

                return true;
        }

        return false;
    }

    public void OnFinishDraw() => _isFinishDraw = true;


    void Shoot()
    {
        Vector3 dir = Camera.main.transform.forward;


        var prefab = ItemData.RangeInfo.projectilePrefab;

        var pool = PoolManager.Instance.GetPool(prefab);
        
        pool.Spawn(null).Fire(prefab, transform.position + dir * 0.5f, dir, ItemData.RangeInfo.fireForce);
        
        
        _animator.SetTrigger(_shoot);

        _isFinishDraw = false;
    }
   
}