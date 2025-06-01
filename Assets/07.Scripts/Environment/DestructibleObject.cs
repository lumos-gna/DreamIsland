using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[System.Serializable]
public struct DropItem
{
    //������ ������
    public GameObject prefab;
    [Range(0f, 1f), Tooltip("0~1 ���� Ȯ��")]
    public float dropChance;
}

[RequireComponent(typeof(Animator))]
public class DestructibleObject : MonoBehaviour
{
    [Header("HP Settings")]
    public float maxHP = 100f;
    public float currentHP;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject _helathBarSprite;
    [Header("Damage Settings")]
    public int damageAmount = 10;

    [Header("Drop Settings")]
    public ItemData _dropItem;

    [Header("Sound Settings")]            // ȿ������ �߰�
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
        //ȿ���� ���
        int sfxIndex = TreeSound;
        string nm = gameObject.name.ToLower();
        if (nm.Contains("rock")) sfxIndex = RockSound;
        else if (nm.Contains("mushroom")) sfxIndex = MushroomSound;
        // else > TreeSound

       // AudioManager.Instance.PlaySFXAtPoint(sfxIndex, transform.position);

        currentHP -= amount;
        // 체력바 표시
        if (_healthBar != null)
        {
            _helathBarSprite.gameObject.SetActive(true);
            _healthBar.UpdateHealthBar(maxHP, currentHP);
            _healthBar.DamageText(damageAmount); // 여기에 플레이어 데미지 넣기
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
        // HP ���� ��� �� ��ǥ ũ�� ����
        float hpRatio = Mathf.Clamp01(currentHP / maxHP);
        Vector3 targetScale = _originalScale * hpRatio;

        // �޽� ����
        float pulseFactor = 1.05f; // 5% Ŀ���ٰ�
        float pulseTime = 0.1f;   // 0.1�� Ű���
        // �޽� ��
        for (float t = 0; t < pulseTime; t += Time.deltaTime)
        {
            float lerp = t / pulseTime;
            transform.localScale = Vector3.Lerp(targetScale, targetScale * pulseFactor, lerp);
            yield return null;
        }
        // �޽� �ٿ�
        for (float t = 0; t < pulseTime; t += Time.deltaTime)
        {
            float lerp = t / pulseTime;
            transform.localScale = Vector3.Lerp(targetScale * pulseFactor, targetScale, lerp);
            yield return null;
        }
        // ���� ũ�� ����
        transform.localScale = targetScale;

        // 3) �¿� ���� (shake)
        float shakeDuration = 0.2f;
        float shakeMagnitude = 0.05f * hpRatio; // HP �������� ��鸲 �۰�
        for (float t = 0; t < shakeDuration; t += Time.deltaTime)
        {
            float offset = Mathf.Sin(t * Mathf.PI * 10f) * shakeMagnitude;
            transform.localPosition = _originalPosition + Vector3.right * offset;
            yield return null;
        }
        // ��ġ ����
        transform.localPosition = _originalPosition;

        _damageFeedbackCoroutine = null;
    }

    private void Die()
    {
        DropItem();
        //_anim.SetTrigger("ObjectHit"); // �״� �ִϸ��̼� ����� ����
    }

    public void DropItem()
    {
        Instantiate(_dropItem.DroppedPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
