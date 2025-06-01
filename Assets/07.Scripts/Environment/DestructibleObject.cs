using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct DropItem
{
    // 드롭 아이템 정보
    public GameObject prefab;
    [Range(0f, 1f), Tooltip("0~1 사이의 드롭 확률")]
    public float dropChance;
}

[RequireComponent(typeof(Animator))]
public class DestructibleObject : MonoBehaviour
{
    [Header("HP 설정")]
    public float maxHP = 100f;
    public float currentHP;

    [Header("데미지 설정")]
    public int damageAmount = 10;

    [Header("드롭 설정")]
    public DropItem[] dropItems;

    [Header("사운드 설정")]
    [SerializeField] private int TreeSound = 13;
    [SerializeField] private int RockSound = 12;
    [SerializeField] private int MushroomSound = 13;

    private Vector3 _originalScale;
    private Vector3 _originalPosition;
    private Coroutine _damageFeedbackCoroutine;

    void Awake()
    {
        currentHP = maxHP;
        _originalScale = transform.localScale;
        _originalPosition = transform.localPosition;
    }

    public void ObjectTakeDamage(int amount)
    {
        // 사운드 재생
        int sfxIndex = TreeSound;
        string nm = gameObject.name.ToLower();
        if (nm.Contains("rock")) sfxIndex = RockSound;
        else if (nm.Contains("mushroom")) sfxIndex = MushroomSound;
        // 나머지는 TreeSound

        AudioManager.Instance.PlaySFXAtPoint(sfxIndex, transform.position);

        currentHP -= amount;
        if (_damageFeedbackCoroutine != null)
            StopCoroutine(_damageFeedbackCoroutine);

        _damageFeedbackCoroutine = StartCoroutine(DamageFeedback());

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    private IEnumerator DamageFeedback()
    {
        // HP 비율에 따라 크기 비율 계산
        float hpRatio = Mathf.Clamp01(currentHP / maxHP);
        Vector3 targetScale = _originalScale * hpRatio;

        // 1) 펄스 효과
        float pulseFactor = 1.05f; // 5% 확대
        float pulseTime = 0.1f;   // 0.1초 동안
        for (float t = 0; t < pulseTime; t += Time.deltaTime)
        {
            float lerp = t / pulseTime;
            transform.localScale = Vector3.Lerp(targetScale, targetScale * pulseFactor, lerp);
            yield return null;
        }
        for (float t = 0; t < pulseTime; t += Time.deltaTime)
        {
            float lerp = t / pulseTime;
            transform.localScale = Vector3.Lerp(targetScale * pulseFactor, targetScale, lerp);
            yield return null;
        }
        // 최종 크기 적용
        transform.localScale = targetScale;

        // 2) 흔들림 효과 (shake)
        float shakeDuration = 0.2f;
        float shakeMagnitude = 0.05f * hpRatio; // HP 비율에 따라 폭 조절
        for (float t = 0; t < shakeDuration; t += Time.deltaTime)
        {
            float offset = Mathf.Sin(t * Mathf.PI * 10f) * shakeMagnitude;
            transform.localPosition = _originalPosition + Vector3.right * offset;
            yield return null;
        }
        // 위치 복원
        transform.localPosition = _originalPosition;

        _damageFeedbackCoroutine = null;
    }

    private void Die()
    {
        StartCoroutine(HandleDropsAndDestroy());
    }

    /// 아이템 드롭 후 객체 파괴
    private IEnumerator HandleDropsAndDestroy()
    {
        // 아이템 드롭
        foreach (var item in dropItems)
        {
            if (item.prefab == null) continue;
            if (Random.value <= item.dropChance)
            {
                Instantiate(item.prefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
        yield break;
    }
}
