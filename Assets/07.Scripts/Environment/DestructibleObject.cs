using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DropItem
{
    //떨어질 프리팹
    public GameObject prefab;
    [Range(0f, 1f), Tooltip("0~1 사이 확률")]
    public float dropChance;
}

[RequireComponent(typeof(Animator))]
public class DestructibleObject : MonoBehaviour
{
    [Header("HP Settings")]
    public float maxHP = 100f;
    private float currentHP;

    [Header("Damage Settings")]
    public int damageAmount = 10;

    [Header("Drop Settings")]
    public DropItem[] dropItems;

    private Animator _anim;

    void Awake()
    {
        currentHP = maxHP;
        _anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        _anim.SetTrigger("Hit"); // 맞는 애니메이션 재생

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        _anim.SetTrigger("Die");
        StartCoroutine(HandleDropsAndDestroy());
    }

    /// 드랍 아이템 생성 후 본 오브젝트 삭제
    private IEnumerator HandleDropsAndDestroy()
    {
        // 죽는 애니메이션 재생 끝날 때까지 대기 (애니메이션 길이에 맞춰 조정)
        yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length);

        // 드랍 처리
        foreach (var item in dropItems)
        {
            if (item.prefab == null) continue;
            if (Random.value <= item.dropChance)
            {
                Instantiate(item.prefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }

}
