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
    public float currentHP;

    [Header("Damage Settings")]
    public int damageAmount = 10;

    [Header("Drop Settings")]
    public DropItem[] dropItems;

    [Header("Sound Settings")]            // 효과음용 추가
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
        //효과음 재생
        int sfxIndex = TreeSound;
        string nm = gameObject.name.ToLower();
        if (nm.Contains("rock")) sfxIndex = RockSound;
        else if (nm.Contains("mushroom")) sfxIndex = MushroomSound;
        // else > TreeSound

        AudioManager.Instance.PlaySFXAtPoint(sfxIndex, transform.position );

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
        // HP 비율 계산 및 목표 크기 결정
        float hpRatio = Mathf.Clamp01(currentHP / maxHP);
        Vector3 targetScale = _originalScale * hpRatio;

        // 펄스 설정
        float pulseFactor = 1.05f; // 5% 커졌다가
        float pulseTime = 0.1f;   // 0.1초 키우고
        // 펄스 업
        for (float t = 0; t < pulseTime; t += Time.deltaTime)
        {
            float lerp = t / pulseTime;
            transform.localScale = Vector3.Lerp(targetScale, targetScale * pulseFactor, lerp);
            yield return null;
        }
        // 펄스 다운
        for (float t = 0; t < pulseTime; t += Time.deltaTime)
        {
            float lerp = t / pulseTime;
            transform.localScale = Vector3.Lerp(targetScale * pulseFactor, targetScale, lerp);
            yield return null;
        }
        // 최종 크기 고정
        transform.localScale = targetScale;

        // 3) 좌우 떨림 (shake)
        float shakeDuration = 0.2f;
        float shakeMagnitude = 0.05f * hpRatio; // HP 낮을수록 흔들림 작게
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
        //_anim.SetTrigger("ObjectHit"); // 죽는 애니메이션 만들면 넣자
    }

    /// 드랍 아이템 생성 후 본 오브젝트 삭제
    private IEnumerator HandleDropsAndDestroy()
    {

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
        yield break;
    }

}
