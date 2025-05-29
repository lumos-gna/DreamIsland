using UnityEngine;

public class EquippedWeapon : EquippedItem
{
    private WeaponItemDataSO _data;
    
    private bool _isRunning;
    
    private static readonly int Attack = Animator.StringToHash("Attack");
    
    
    public override void Init(ItemDataSO itemDataSO)
    {
        if (itemDataSO is WeaponItemDataSO weaponData)
        {
            _data = weaponData;
        }
        else
        {
            Debug.LogError("잘못된 타입");
        }

    }

    public override void Use()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            _animator.SetTrigger(Attack);
        }
    }
    
    public void OnHit()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        
        if (Physics.Raycast(ray, out RaycastHit hit, _data.Range))
        {
            Debug.Log("Hit");
        }
    }

    public void OnFinish()
    {
        _isRunning = false;
    }
}
