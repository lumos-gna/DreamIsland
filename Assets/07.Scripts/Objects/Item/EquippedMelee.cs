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
        // 레이어 검사
        int mask = LayerMask.GetMask("Enemy", "Destructible");

        Ray ray = _camera.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, ItemData.MeleeInfo.range, mask))
        {
            // 2) 적(Enemy)일 때 기존 로직
            if (hit.collider.TryGetComponent(out BaseEnemy enemy))
            {
                // 적에게 3 데미지 주는 흐름 그대로
                Debug.Log(enemy.name);
                enemy.GetEnemyHealth().SetDamage(3);
                enemy.TakeDamage(3);
                return; // 적을 때렸으면 바로 리턴해도 되고, 
                        // 파괴 오브젝트도 동시에 때리려면 return을 제거하면 된다.
            }

            // 3) 적이 아니라면 "DestructibleObject" 확인
            if (hit.collider.TryGetComponent(out DestructibleObject destructible))
            {
                // 파괴 가능 오브젝트에 데미지 주기
                destructible.ObjectTakeDamage(3);
            }
        }
    }

    public void OnFinish()
    {
        _isRunning = false;
    }
}
