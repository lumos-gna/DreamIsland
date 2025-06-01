using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlashUI : BaseUI
{

    public override bool IsEnabled => _flashImage.gameObject.activeInHierarchy;

    [SerializeField] private Image _flashImage;
    [SerializeField] private float _flashSpeed = 0.5f;
    [SerializeField] private float _startAlpha = 0.4f; // 알파 기본값
    private Coroutine _flashCoroutine; // 중복 방지용
    
    public override void Init()
    {
        Debug.Log("flahs 초기화");
        if (_flashImage != null)
        {
            Debug.Log("flash image있음");
            Disable();
            Color color = _flashImage.color;
            color.a =01f; // 알파값 0으로
            _flashImage.color = color;
        }
        else
        {
            Debug.Log("flash image없음");
        }
    }
    public override void Disable()
    {
        _flashImage.gameObject.SetActive(false);
    }

    public override void Enable()
    {
        _flashImage.gameObject.SetActive(true);
    }

    public void PlayFlash()
    {
        // 코루틴 실행중이였으면 중단 후 다시 시작
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }

        _flashCoroutine = StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        Enable();

        Color color = new Color(1f, 105f / 255f, 105f / 255f, _startAlpha);
        _flashImage.color = color;

        float alpha = _startAlpha;
  
        while (alpha > 0f)
        {
            alpha -= (_startAlpha / _flashSpeed) * Time.deltaTime;
            color.a = alpha;
            _flashImage.color = color;
            yield return null;
        }
        Disable();
    }
}
