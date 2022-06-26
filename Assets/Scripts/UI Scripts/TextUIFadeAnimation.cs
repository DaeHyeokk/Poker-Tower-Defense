using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUIFadeAnimation : MonoBehaviour
{
    public enum DeltaTimeMode { GameTime, RealTime }
    [SerializeField]
    private DeltaTimeMode _deltaTimeMode;
    [SerializeField]
    private float _lerpSpeed;
    [SerializeField]
    private TextMeshProUGUI _text;

    private bool _isBlinking;

    public event Action onCompletionFadeIn;
    public event Action onCompletionFadeOut;

    public float lerpSpeed
    {
        get => _lerpSpeed;
        set => _lerpSpeed = value;
    }
    public bool isBlinking => _isBlinking;

    public void FadeOutText()
    {
        StartCoroutine(FadeOutTextCoroutine());
    }
    public void FadeInText()
    {
        StartCoroutine(FadeInTextCoroutine());
    }
    public void BlinkText()
    {
        _isBlinking = true;

        StartCoroutine(BlinkImageCoroutine());
    }

    public void StopBlinkText()
    {
        _isBlinking = false;
    }

    private IEnumerator FadeOutTextCoroutine()
    {
        // 알파 값을 1로 설정.
        Color color = _text.color;
        color.a = 1f;
        _text.color = color;

        float currentTime = 0f;
        float percent = 0f;

        // percent가 1이 될 때 까지 증가
        while (percent < 1)
        {
            IncreaseCurrentTime(ref currentTime);
            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(1f, 0f, percent);

            color = _text.color;
            color.a = lerp;
            _text.color = color;

            yield return null;
        }

        if (onCompletionFadeOut != null)
            onCompletionFadeOut();
    }

    private IEnumerator FadeInTextCoroutine()
    {
        // 알파 값을 0으로 설정.
        Color color = _text.color;
        color.a = 0f;
        _text.color = color;

        float currentTime = 0f;
        float percent = 0f;

        // 알파 값이 1이 될 때 까지 알파 값 증가
        while (percent < 1)
        {
            IncreaseCurrentTime(ref currentTime);
            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(0f, 1f, percent);

            color = _text.color;
            color.a = lerp;
            _text.color = color;

            yield return null;
        }

        if (onCompletionFadeIn != null)
            onCompletionFadeIn();
    }

    private IEnumerator BlinkImageCoroutine()
    {
        // 알파 값을 0으로 설정.
        Color color = _text.color;
        color.a = 0f;
        _text.color = color;

        float currentTime = 0f;
        float percent = 0f;

        // 알파 값이 1이 될 때 까지 알파 값 증가
        while (percent < 1)
        {
            // 텍스트가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
            if (!_isBlinking)
            {
                color.a = 0f;
                _text.color = color;
                yield break;
            }

            IncreaseCurrentTime(ref currentTime);
            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(0f, 1f, percent);

            color = _text.color;
            color.a = lerp;
            _text.color = color;

            yield return null;
        }

        // 알파 값이 0이 될 때 까지 알파 값 감소
        while (percent > 0)
        {
            // 텍스트가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
            if (!_isBlinking)
            {
                color.a = 0f;
                _text.color = color;
                yield break;
            }

            IncreaseCurrentTime(ref currentTime);
            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(1f, 0f, percent);

            color = _text.color;
            color.a = lerp;
            _text.color = color;

            yield return null;
        }

        // 반복해서 깜빡이도록 하기 위한 코루틴 메소드 재귀호출.
        StartCoroutine(BlinkImageCoroutine());
    }

    private void IncreaseCurrentTime(ref float currentTime)
    {
        if (_deltaTimeMode == DeltaTimeMode.GameTime)
            currentTime += Time.deltaTime;
        else
            currentTime += Time.unscaledDeltaTime;
    }
}
