using UnityEngine;


[CreateAssetMenu(fileName = "WeaponItemData", menuName = "ScriptableObjects/Temp/Weapon Item Data")]
public class WeaponItemDataSO : ItemDataSO
{
    public CraftingRecipe CraftingRecipe => craftingRecipe;
    public Projectile ProjectilePrefab => projectilePrefab;
    
    public float Range => range;
    public float UnitDamage => unitDamage;
    public float ObjectDamage => objectDamage;
    
    public float ShootForce => shootForce;

    
    
    
    [Space(10f)]
    [Header("CraftInfo")]
    [SerializeField] private CraftingRecipe craftingRecipe;
    
    
    
    [Space(10f)]
    [Header("AttackInfo")]
    [SerializeField] private float range;
    
    [SerializeField] private float unitDamage;
    
    [SerializeField] private float objectDamage;

    [SerializeField] private float shootForce;
    
    [SerializeField] private Projectile projectilePrefab;
    
}