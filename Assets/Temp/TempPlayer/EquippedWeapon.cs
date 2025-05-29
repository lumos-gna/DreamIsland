using UnityEngine;

public class EquippedWeapon : EquippedItem
{
    [SerializeField] private Animator animator;

    private Camera _camera;
    
    private WeaponItemDataSO _data;
    
    private bool _isRunning;
    
    private readonly int _attack = Animator.StringToHash("Attack");
    
    
    public override void Equip(GameObject user, ItemDataSO itemData)
    {
        _camera = Camera.main;
        
        _data = itemData as WeaponItemDataSO;
    }

    public override void UnEquip()
    {
        
    }

    public override void Use()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            animator.SetTrigger(_attack);
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
