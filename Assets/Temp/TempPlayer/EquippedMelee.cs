//using UnityEngine;

//public class EquippedMelee : EquippedItem
//{

//    private LayerMask environmentLayer;
//    private LayerMask enemyLayer;
//    public override ItemDataSO ItemData => _itmeData;
    
//    [SerializeField] private Animator animator;

//    private Camera _camera;
    
//    private WeaponItemDataSO _itmeData;
    
//    private bool _isRunning;
    
//    private readonly int _attack = Animator.StringToHash("Attack");

//    void Awake()
//    {
//        environmentLayer = LayerMask.GetMask("Environment");
//        enemyLayer = LayerMask.GetMask("Enemy");
//    }

//    public override void Equip(GameObject user, ItemDataSO itemData)
//    {
//        _camera = Camera.main;
        
//        _itmeData = itemData as WeaponItemDataSO;
//    }

//    public override void UnEquip()
//    {
//    }
    
//    public override bool TryUse(EquippedController.InputState inputState)
//    {
//        switch (inputState)
//        {
//            case EquippedController.InputState.Down :
//                if (!_isRunning)
//                {
//                    _isRunning = true;
//                    animator.SetTrigger(_attack);

//                    return true;
//                }

//                break;
//        }

//        return false;
//    }

    
//    public void OnHit()
//    {
//        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

//        if (Physics.Raycast(ray, out RaycastHit hit, _itmeData.Range))
//        {
//            int hitLayer = hit.collider.gameObject.layer;
//            Debug.Log("Hit");

//            //환경공격
//            if (((1 << hitLayer) & environmentLayer) != 0)
//            {
//                var destructible = hit.collider.GetComponentInParent<DestructibleObject>();
//                if (destructible != null)
//                {
//                    destructible.ObjectTakeDamage(20);
//                    Debug.Log($"Hit ENV {destructible.name}: –{20} HP");
//                }
//            }
//            //에너미공격
//            else if (((1 << hitLayer) & enemyLayer) != 0)
//            {
//                if (hit.collider.TryGetComponent(out BaseEnemy enemy))
//                {
//                    //대상
//                    enemy.TakeDamage(2);
//                }
//            }    
//        }
//    }

//    public void OnFinish()
//    {
//        _isRunning = false;
//    }
//}
