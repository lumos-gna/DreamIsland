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

        if (Physics.Raycast(ray, out RaycastHit hit, ItemData.MeleeInfo.range, LayerMask.GetMask("Enemy", "Environment")))
        {
            if (hit.collider.TryGetComponent(out BaseEnemy enemy))
            {
                //대상
                Debug.Log(enemy.name);
                enemy.GetEnemyHealth().SetDamage(3);
                enemy.TakeDamage(3);
            }
            if (hit.collider.TryGetComponent(out DestructibleObject destruct))
            {
                Debug.Log(destruct.name);
                Debug.Log($"{name} take damage {destruct.maxHP}, currentHP before: {destruct.currentHP}");
                destruct.ObjectTakeDamage(destruct.damageAmount);         // 실제 데미지 적용
            }
        }
    }

    public void OnFinish()
    {
        _isRunning = false;
    }
}
