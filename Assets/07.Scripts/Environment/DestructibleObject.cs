using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[System.Serializable]
public struct DropItem
{
    public GameObject prefab;
    [Range(0f, 1f), Tooltip("0~1 드랍 확률")]
    public float dropChance;
}

[RequireComponent(typeof(Animator))]
public class DestructibleObject : MonoBehaviour
{
    [Header("HP 설정")]
    public float maxHP = 100f;
    public float currentHP;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject _helathBarSprite;

    [Header("데미지 설정")]
    public int damageAmount = 10;

    [Header("드랍 아이템")]
    public ItemData _dropItem;

    [Header("사운드")]
    [SerializeField] private int TreeSound = 13;
    [SerializeField] private int RockSound = 12;
    [SerializeField] private int MushroomSound = 13;

    [SerializeField] private ParticleSystem _damageParticle;

    private Vector3 _originalScale;
    private Vector3 _originalPosition;
    private Coroutine _damageFeedbackCoroutine;

    public event Action<float, float> OnHealthChanged;

    void Awake()
    {
        currentHP = maxHP;
        _originalScale = transform.localScale;
        _originalPosition = transform.localPosition;
    }

    public void ObjectTakeDamage(int amount)
    {
        // 사운드 효과
        int sfxIndex = TreeSound;
        string nm = gameObject.name.ToLower();
        if (nm.Contains("rock")) sfxIndex = RockSound;
        else if (nm.Contains("mushroom")) sfxIndex = MushroomSound;

        AudioManager.Instance.PlaySFXAtPoint(sfxIndex, transform.position);

        currentHP -= amount;

        // 체력바 표시
        if (_healthBar != null)
        {
            _helathBarSprite.gameObject.SetActive(true);
            _healthBar.UpdateHealthBar(maxHP, currentHP);
            _healthBar.DamageText(damageAmount);
        }
        OnHealthChanged?.Invoke(maxHP, currentHP);

        _damageParticle?.Play();

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
        // HP 비율 (최소 0.6 보장)
        float hpRatio = Mathf.Max(0.6f, currentHP / maxHP);

        // 크기 줄어들기 비율 (HP 60% 이상만 줄어들게)
        Vector3 targetScale = _originalScale * hpRatio;

        // 크기 살짝 커졌다 작아지기
        float pulseFactor = 1.05f;
        float pulseTime = 0.1f;
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
        transform.localScale = targetScale;

        // 흔들림 효과
        float shakeDuration = 0.2f;
        float shakeMagnitude = 0.05f * hpRatio;

        Vector3 startPos = _originalPosition;
        for (float t = 0; t < shakeDuration; t += Time.deltaTime)
        {
            float offset = Mathf.Sin(t * Mathf.PI * 10f) * shakeMagnitude;
            transform.localPosition = startPos + Vector3.right * offset;
            yield return null;
        }
        transform.localPosition = startPos;

        _damageFeedbackCoroutine = null;
    }

    private void Die()
    {
        DropItem();
    }

    public void DropItem()
    {
        Instantiate(_dropItem.DroppedPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
