using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeAnimation : MonoBehaviour
{
    public enum DeltaTimeMode { GameTime, RealTime }
    [SerializeField]
    private DeltaTimeMode _deltaTimeMode;
    [SerializeField]
    private float _fadeTime;
    [SerializeField]
    private TextMeshPro _text;

    private bool _isBlinking;
    private Color _tempColor;

    public event Action onCompletionFadeIn;
    public event Action onCompletionFadeOut;

    public float fadeTime
    {
        get => _fadeTime;
        set => _fadeTime = value;
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
        _tempColor = _text.color;
        _tempColor.a = 1f;
        _text.color = _tempColor;

        // 알파 값이 0이 될 때 까지 알파 값 감소
        while (_text.color.a > 0)
        {
            _tempColor = _text.color;
            DecreaseColorAlpha();
            yield return null;
        }

        if (onCompletionFadeOut != null)
            onCompletionFadeOut();
    }

    private IEnumerator FadeInTextCoroutine()
    {
        // 알파 값을 0으로 설정.
        _tempColor = _text.color;
        _tempColor.a = 0f;
        _text.color = _tempColor;

        // 알파 값이 1이 될 때 까지 알파 값 증가
        while (_text.color.a < 1)
        {
            _tempColor = _text.color;
            IncreaseColorAlpha();
            yield return null;
        }

        if (onCompletionFadeIn != null)
            onCompletionFadeIn();
    }

    private IEnumerator BlinkImageCoroutine()
    {
        // 알파 값을 0으로 설정.
        _tempColor = _text.color;
        _tempColor.a = 0f;
        _text.color = _tempColor;

        // 알파 값이 1이 될 때 까지 알파 값 증가
        while (_text.color.a < 0.2f)
        {
            _tempColor = _text.color;

            // 텍스트가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
            if (!_isBlinking)
            {
                _tempColor.a = 0f;
                _text.color = _tempColor;
                yield break;
            }
            else
                IncreaseColorAlpha();

            yield return null;
        }

        // 알파 값이 0이 될 때 까지 알파 값 감소
        while (_text.color.a > 0f)
        {
            _tempColor = _text.color;

            // 텍스트가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
            if (!_isBlinking)
            {
                _tempColor.a = 0f;
                _text.color = _tempColor;
                yield break;
            }
            else
                DecreaseColorAlpha();

            yield return null;
        }

        // 반복해서 깜빡이도록 하기 위한 코루틴 메소드 재귀호출.
        StartCoroutine(BlinkImageCoroutine());
    }

    private void IncreaseColorAlpha()
    {
        if (_deltaTimeMode == DeltaTimeMode.GameTime)
            _tempColor.a += Time.deltaTime / fadeTime;
        else
            _tempColor.a += Time.unscaledTime / fadeTime;

        _text.color = _tempColor;
    }

    private void DecreaseColorAlpha()
    {
        if (_deltaTimeMode == DeltaTimeMode.GameTime)
            _tempColor.a -= Time.deltaTime / fadeTime;
        else
            _tempColor.a -= Time.unscaledDeltaTime / fadeTime;

        _text.color = _tempColor;
    }
}
