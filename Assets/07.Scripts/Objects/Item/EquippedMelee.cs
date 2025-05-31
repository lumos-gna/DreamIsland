using UnityEngine;

public class EquippedMelee : EquippedItem
{
    private Camera _camera;
    
    
    private bool _isRunning;
    
    private readonly int _attack = Animator.StringToHash("Attack");
    
    
    public override void Equip(EquippedController controller, ItemData itemData)
    {
        base.Equip(controller, itemData);
        
        _camera = Camera.main;
    }


    public override void Use()
    {
        if (!ItemData.IsMeleeItem) return;

        if (_controller.IsInputDown)
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _animator.SetTrigger(_attack);
            }
        }
    }

    
    public void OnHit()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        
        if (Physics.Raycast(ray, out RaycastHit hit, ItemData.MeleeInfo.range))
        {
            if (hit.collider.TryGetComponent(out BaseEnemy enemy))
            {
                //대상
                enemy.TakeDamage(2);
            }
        }
    }

    public void OnFinish()
    {
        _isRunning = false;
    }
}
