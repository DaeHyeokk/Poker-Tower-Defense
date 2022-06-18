using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeAnimation : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _text;

    public event Action onCompletionFadeIn;
    public event Action onCompletionFadeOut;

    private bool _isBlinking;

    public float fadeTime { get; set; }
    public bool isBlinking => _isBlinking;


    public void FadeOutText()
    {
        StartCoroutine(FadeOutImageCoroutine());
    }
    public void FadeInText()
    {
        StartCoroutine(FadeInImageCoroutine());
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

    private IEnumerator FadeOutImageCoroutine()
    {
        // 알파 값을 1로 설정.
        Color color = _text.color;
        color.a = 1f;
        _text.color = color;

        // 알파 값이 0이 될 때 까지 알파 값 감소
        while (_text.color.a > 0)
        {
            color = _text.color;
            color.a -= Time.unscaledDeltaTime / fadeTime;
            _text.color = color;

            yield return null;
        }

        if (onCompletionFadeOut != null)
            onCompletionFadeOut();
    }

    private IEnumerator FadeInImageCoroutine()
    {
        // 알파 값을 0으로 설정.
        Color color = _text.color;
        color.a = 0f;
        _text.color = color;

        // 알파 값이 1이 될 때 까지 알파 값 증가
        while (_text.color.a < 1)
        {
            color = _text.color;
            color.a += Time.unscaledDeltaTime / fadeTime;
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

        // 알파 값이 1이 될 때 까지 알파 값 증가
        while (_text.color.a < 0.2f)
        {
            color = _text.color;

            // 텍스트가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
            if (!_isBlinking)
            {
                color.a = 0f;
                _text.color = color;
                yield break;
            }
            else
            {
                color.a += Time.unscaledDeltaTime / fadeTime;
                _text.color = color;
            }

            yield return null;
        }

        // 알파 값이 0이 될 때 까지 알파 값 감소
        while (_text.color.a > 0f)
        {
            color = _text.color;


            // 텍스트가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
            if (!_isBlinking)
            {
                color.a = 0f;
                _text.color = color;
                yield break;
            }
            else
            {
                color.a -= Time.unscaledDeltaTime / fadeTime;
                _text.color = color;
            }

            yield return null;
        }

        // 반복해서 깜빡이도록 하기 위한 코루틴 메소드 재귀호출.
        StartCoroutine(BlinkImageCoroutine());
    }
}
