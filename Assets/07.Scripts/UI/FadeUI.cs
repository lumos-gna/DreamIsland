using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : BaseUI
{
    public override bool IsEnabled => _fadeImage.gameObject.activeInHierarchy;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _fadeImage;
    
    private Action _onCompleteFade;
    
    public void PlayFade(float fadeDuration, float stayDuration)
    {
        Enable(); // 먼저 보여야 하니까 켜주고
        StartCoroutine(FadeRoutine(fadeDuration, stayDuration));
    }


    public override void Init()
    {
        if(_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
            Disable();
        }
    }
    public override void Disable()
    {
        _fadeImage.SetActive(false);
    }

    public override void Enable()
    {
        _fadeImage.SetActive(true);
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(DoFade(0f, 1f, duration));
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(DoFade(1f, 0f, duration));
    }

    private IEnumerator DoFade(float from, float to, float duration)
    {
        Enable();
        float time = 0f;
        while (time < duration)
        {
            _canvasGroup.alpha = Mathf.Lerp(from, to, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = to;
        _onCompleteFade?.Invoke();
        _onCompleteFade = null; // 콜백 초기화
    }

    private IEnumerator FadeRoutine(float fadeDuration, float stayDuration)
    {
        yield return StartCoroutine(DoFade(0f, 1f, fadeDuration)); // 페이드 인
        yield return new WaitForSeconds(stayDuration);                 // 유지 시간
        yield return StartCoroutine(DoFade(1f, 0f, fadeDuration)); // 페이드 아웃
        Disable();
    }

    public void RegisteronCompleteFade(Action action)
    {
        _onCompleteFade = action;
    }
}
