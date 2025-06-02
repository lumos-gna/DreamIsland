using System;
using UnityEngine;

public class EquippedRange : EquippedItem
{
    private static readonly int IsDraw = Animator.StringToHash("IsDraw");
    private static readonly int TriggerFire = Animator.StringToHash("TriggerFire");

    private bool _isFinishDraw;
    private bool _isDrawing;

    public override void Use()
    {
        if (!ItemData.IsRangeItem && !ItemData.IsDamageable)
            return;

        if (_controller.IsInputDown)
        {
            if (_controller.Inventory.FindSlot((slot) => slot.item == ItemData.RangeInfo.projectileItemData) == null)
                return;

            if (!_isDrawing)
            {
                _isDrawing = true;

                _animator.SetBool(IsDraw, true);
            }
        }

        if (_controller.IsInputUp)
        {
            if (_isFinishDraw)
            {
                Fire();
            }
            else
            {
                _animator.SetBool(IsDraw, false);

                _isDrawing = false;
            }
        }
    }

    public void OnFinishDraw() => _isFinishDraw = true;

    public void OnFinishFire()
    {
        _animator.SetBool(IsDraw, false);

        _isDrawing = false;
    }

    void Fire()
    {
        Vector3 camDir = Camera.main.transform.forward;
        var prefab = ItemData.RangeInfo.projectilePrefab;
        var pool = PoolManager.Instance.GetPool(prefab);
        float damage = ItemData.DamageInfo.unitDamage;

        pool.Spawn(null).Fire(prefab, transform.position + camDir, camDir, ItemData.RangeInfo.fireForce, damage);

        _animator.SetTrigger(TriggerFire);
        _isFinishDraw = false;

        _controller.Inventory.DecreaseItem(ItemData.RangeInfo.projectileItemData);
    }

}