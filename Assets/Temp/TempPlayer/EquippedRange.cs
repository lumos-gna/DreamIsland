using System;
using UnityEngine;

public class EquippedRange : EquippedItem
{
    public override ItemDataSO ItemData => _itemData;
    
    [SerializeField] private Animator animator;

    private WeaponItemDataSO _itemData;
    
    private bool _isFinishDraw;
    private bool _isDrawing;
    
    private readonly int _isDraw = Animator.StringToHash("IsDraw");
    private readonly int _shoot = Animator.StringToHash("Shoot");


    public override void Equip(GameObject user, ItemDataSO itemData)
    {
        _itemData = itemData as WeaponItemDataSO;
    }

    public override void UnEquip()
    {
    }

    public override bool TryUse(EquippedController.InputState inputState)
    {
        switch (inputState)
        {
            case EquippedController.InputState.Down :
                
                if (!_isDrawing)
                {
                    _isDrawing = true;
            
                    animator.SetBool(_isDraw, true);
                }
                
                break;
            
            case EquippedController.InputState.Up :
                
                if (_isFinishDraw)
                {
                    Shoot();
                }
                else
                {
                    animator.SetBool(_isDraw, false);
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


        var prefab = _itemData.ProjectilePrefab;

        var pool = PoolManager.Instance.GetPool(prefab);
        
        pool.Spawn(null).Fire(prefab, transform.position + dir * 0.5f, dir, _itemData.ShootForce);
        
        
        animator.SetTrigger(_shoot);

        _isFinishDraw = false;
    }
   
}